using System;
using System.Collections.Generic;
using System.IO;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace J.A.K.E_WFA
{
    public partial class Form1 : Form
    {
        private bool isDarkMode = false;


        Dictionary<string, Part> inventory = new Dictionary<string, Part>();
        string filePath = FindInventoryFilePath();

        [DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        private static extern IntPtr CreateRoundRectRgn(int nLeftRect, int nTopRect, int nRightRect,
    int nBottomRect, int nWidthEllipse, int nHeightEllipse);

        // Required for dragging the window from a custom panel
        [DllImport("user32.dll")]
        public static extern bool ReleaseCapture();

        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

        private const int WM_NCLBUTTONDOWN = 0xA1;
        private const int HTCAPTION = 0x2;

        static string configPath = Path.Combine(
     Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
     "CASH", "inventory_config.txt"
 );

        static string FindInventoryFilePath()
        {
            // 1. Try loading from config if it exists
            if (File.Exists(configPath))
            {
                string savedPath = File.ReadAllText(configPath);
                if (File.Exists(savedPath))
                    return savedPath;
            }

            // 2. Try auto-detecting the file
            string relativePath = @"My Drive\J.A.K.E FOLDER\J.A.K.E\inventory.txt";

            foreach (var drive in DriveInfo.GetDrives())
            {
                try
                {
                    if (drive.IsReady)
                    {
                        string candidatePath = Path.Combine(drive.RootDirectory.FullName, relativePath);
                        if (File.Exists(candidatePath))
                        {
                            SaveInventoryConfig(candidatePath);
                            return candidatePath;
                        }
                    }
                }
                catch { /* Ignore drives that throw */ }
            }

            // 3. Prompt user to find or create the file
            using (var dlg = new OpenFileDialog())
            {
                dlg.Title = "Locate or create inventory.txt";
                dlg.FileName = "inventory.txt";
                dlg.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*";
                dlg.CheckFileExists = false;

                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    string selectedPath = dlg.FileName;

                    // Create the file if it doesn't exist
                    if (!File.Exists(selectedPath))
                        File.WriteAllText(selectedPath, ""); // Create empty inventory file

                    SaveInventoryConfig(selectedPath);
                    return selectedPath;
                }
            }

            // 4. User cancelled - return null or fallback
            return null;
        }

        static void SaveInventoryConfig(string path)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(configPath));
            File.WriteAllText(configPath, path);
        }


        public Form1()
        {
            InitializeComponent();
            LoadInventory();
            UpdateInventoryGrid();
            SetupInventoryTimer();
            InitializeNotifyIcon();
        }

        public class Part
        {
            public string Name { get; set; }
            public int Quantity { get; set; }
            public string ShelfNumber { get; set; }
            public string CarPartType { get; set; }
            public string AddedOn { get; set; }
        }

        void LoadInventory()
        {
            inventory.Clear();
            if (File.Exists(filePath))
            {
                var lines = File.ReadAllLines(filePath);
                foreach (var line in lines)
                {
                    var parts = line.Split(',');
                    if (parts.Length == 5)
                    {
                        string name = parts[0].Trim();
                        int quantity = int.TryParse(parts[1].Trim(), out int q) ? q : 0;
                        string shelf = parts[2].Trim();
                        string type = parts[3].Trim();
                        string addedOn = parts[4].Trim();

                        inventory[name] = new Part
                        {
                            Name = name,
                            Quantity = quantity,
                            ShelfNumber = shelf,
                            CarPartType = type,
                            AddedOn = addedOn
                        };
                    }
                }
            }
        }
         
        private void dataGridViewInventory_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            // Make sure a valid row is clicked (e.RowIndex is not header or invalid)
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridViewInventory.Rows[e.RowIndex];

                txtName.Text = row.Cells["colName"].Value?.ToString() ?? "";
                txtQuantity.Text = row.Cells["colQuantity"].Value?.ToString() ?? "";
                txtShelf.Text = row.Cells["colShelf"].Value?.ToString() ?? "";
                txtType.Text = row.Cells["colType"].Value?.ToString() ?? "";
            }
        }

        void SaveInventory()
        {
            using (StreamWriter sw = new StreamWriter(filePath))
            {
                foreach (var p in inventory.Values)
                    sw.WriteLine($"{p.Name}, {p.Quantity}, {p.ShelfNumber}, {p.CarPartType}, {p.AddedOn}");
            }
        }

        void UpdateInventoryGrid()
        {
            // Save scroll position and current cell (if any)
            int? firstDisplayed = dataGridViewInventory.FirstDisplayedScrollingRowIndex;
            var currentCell = dataGridViewInventory.CurrentCell;

            // Clear and repopulate rows
            dataGridViewInventory.Rows.Clear();
            foreach (var part in inventory.Values)
            {
                dataGridViewInventory.Rows.Add(part.Name, part.Quantity, part.ShelfNumber, part.CarPartType, part.AddedOn);
            }

            // Restore scroll position
            if (firstDisplayed.HasValue && firstDisplayed.Value >= 0 && firstDisplayed.Value < dataGridViewInventory.RowCount)
            {
                dataGridViewInventory.FirstDisplayedScrollingRowIndex = firstDisplayed.Value;
            }

            // Restore current cell only if grid does NOT have focus (to prevent jump)
            if (!dataGridViewInventory.Focused && currentCell != null)
            {
                int row = currentCell.RowIndex;
                int col = currentCell.ColumnIndex;

                if (row >= 0 && row < dataGridViewInventory.RowCount &&
                    col >= 0 && col < dataGridViewInventory.ColumnCount)
                {
                    dataGridViewInventory.CurrentCell = dataGridViewInventory.Rows[row].Cells[col];
                }
            }
        }

        private void btnRemovePartCon_Click(object sender, EventArgs e)
        {
            string name = txtName.Text.Trim();
            if (string.IsNullOrEmpty(name))
            {
                lblStatus.Text = "Please enter the name of the part to remove.";
                lblStatus.ForeColor = System.Drawing.Color.Red;
                return;
            }

            if (inventory.ContainsKey(name))
            {
                // Ask user for confirmation before deleting
                var result = MessageBox.Show(
                    $"Are you sure you want to remove part '{name}'?",
                    "Confirm Delete",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning
                );

                if (result == DialogResult.Yes)
                {
                    inventory.Remove(name);
                    SaveInventory();
                    UpdateInventoryGrid();

                    lblStatus.Text = $"Removed part '{name}'.";
                    lblStatus.ForeColor = System.Drawing.Color.Green;
                }
                else
                {
                    lblStatus.Text = "Delete cancelled.";
                    lblStatus.ForeColor = System.Drawing.Color.Gray;
                }
            }
            else
            {
                lblStatus.Text = $"Part '{name}' not found.";
                lblStatus.ForeColor = System.Drawing.Color.Red;
            }
        }


        bool IsInternetAvailable()
        {
            try
            {
                using (var ping = new Ping())
                {
                    var reply = ping.Send("8.8.8.8", 1000);
                    return reply.Status == IPStatus.Success;
                }
            }
            catch
            {
                return false;
            }
        }
        private NotifyIcon notifyIcon3;

        private void InitializeNotifyIcon()
        {
            notifyIcon3 = new NotifyIcon
            {
                Visible = true,
                Icon = SystemIcons.Information, // Or load your own icon
                BalloonTipTitle = "C.A.S.H. Inventory",
            };
        }

        private void btnAddPart_Click(object sender, EventArgs e)
        {
            if (!IsInternetAvailable())
            {
                lblStatus.Text = "No internet connection, cannot add part.";
                lblStatus.ForeColor = Color.Red;
                return;
            }

            string name = txtName.Text.Trim();
            if (string.IsNullOrEmpty(name))
            {
                lblStatus.Text = "Part name cannot be empty.";
                lblStatus.ForeColor = Color.Red;
                return;
            }

            if (!int.TryParse(txtQuantity.Text.Trim(), out int quantity))
            {
                lblStatus.Text = "Invalid quantity.";
                lblStatus.ForeColor = Color.Red;
                return;
            }

            if (inventory.ContainsKey(name))
            {
                System.Media.SystemSounds.Exclamation.Play(); // Play warning sound
                ShowCornerNotification($"Part '{name}' is already in the system. Not added again.");
                lblStatus.Text = $"Part '{name}' already exists.";
                lblStatus.ForeColor = Color.OrangeRed;
                return;
            }


            string shelf = txtShelf.Text.Trim();
            string type = txtType.Text.Trim();
            string addedOn = DateTime.Now.ToString("g");

            inventory[name] = new Part
            {
                Name = name,
                Quantity = quantity,
                ShelfNumber = shelf,
                CarPartType = type,
                AddedOn = addedOn
            };

            SaveInventory();
            UpdateInventoryGrid();

            lblStatus.Text = $"Part '{name}' has been added.";
            lblStatus.ForeColor = Color.Green;
        }

        private void ShowCornerNotification(string message)
        {
            notifyIcon3.BalloonTipText = message;
            notifyIcon3.ShowBalloonTip(3000); // Show for 3 seconds
        }

        private void btnRemovePart_Click(object sender, EventArgs e)
        {
            string name = txtName.Text.Trim();
            if (string.IsNullOrEmpty(name))
            {
                lblStatus.Text = "Please enter the name of the part to remove.";
                lblStatus.ForeColor = System.Drawing.Color.Red;
                return;
            }

            if (inventory.ContainsKey(name))
            {
                inventory.Remove(name);
                SaveInventory();
                UpdateInventoryGrid();

                lblStatus.Text = $"Removed part '{name}'.";
                lblStatus.ForeColor = System.Drawing.Color.Green;
            }
            else
            {
                lblStatus.Text = $"Part '{name}' not found.";
                lblStatus.ForeColor = System.Drawing.Color.Red;
            }
        }

        private void btnSearchPart_Click(object sender, EventArgs e)
        {
            string name = txtName.Text.Trim();
            if (string.IsNullOrEmpty(name))
            {
                lblStatus.Text = "Please enter part of the name to search.";
                lblStatus.ForeColor = System.Drawing.Color.Red;
                return;
            }

            bool found = false;

            // Try exact match first
            foreach (DataGridViewRow row in dataGridViewInventory.Rows)
            {
                if (row.Cells[0].Value != null &&
                    row.Cells[0].Value.ToString().Equals(name, StringComparison.OrdinalIgnoreCase))
                {
                    SelectAndLoadRow(row);
                    lblStatus.Text = $"Part '{name}' found and loaded.";
                    lblStatus.ForeColor = System.Drawing.Color.Green;
                    return;
                }
                else
                {
                    row.Selected = false;
                }
            }

            // Try partial match (contains)
            foreach (DataGridViewRow row in dataGridViewInventory.Rows)
            {
                if (row.Cells[0].Value != null &&
                    row.Cells[0].Value.ToString().IndexOf(name, StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    SelectAndLoadRow(row);
                    lblStatus.Text = $"Closest match found: '{row.Cells[0].Value}'.";
                    lblStatus.ForeColor = System.Drawing.Color.Orange;
                    return;
                }
            }

            lblStatus.Text = $"No part matching '{name}' was found.";
            lblStatus.ForeColor = System.Drawing.Color.Red;
        }

        // Helper to select and populate form
        private void SelectAndLoadRow(DataGridViewRow row)
        {
            row.Selected = true;
            dataGridViewInventory.FirstDisplayedScrollingRowIndex = row.Index;

            txtName.Text = row.Cells[0].Value.ToString();
            txtQuantity.Text = row.Cells[1].Value.ToString();
            txtShelf.Text = row.Cells[2].Value.ToString();
            txtType.Text = row.Cells[3].Value.ToString();
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            // Ctrl + Shift + S triggers ShowCatImage
            if (e.Control && e.Shift && e.KeyCode == Keys.S)
            {
                ShowCatImage();
            }

            // Alt + F triggers Femboy
            if (e.Alt && e.KeyCode == Keys.F)
            {
                Femboy();
            }
        }

        private void ShowCatImage()
        {
            string catArt = "SKIPPER!!!!!\n  /\\_/\\\n ( o.o )\n  > ^ <";
            MessageBox.Show(catArt, "Secret Cat", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void Femboy()
        {
            string msg = "I FUCKING LOVE FEMBOYSSS!!!!!!!!!\n\n(@\"⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣧⡿⢋⣥⣴⣶⣦⣍⠻⣿⣿⣿⣿\r\n⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⡏⣴⡟⡿⡿⣻⠿⠻⢷⡜⣿⣿\r\n⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⠸⡿⣿⢿⡏⠁⠉⠠⠀⢇⢹⣿\r\n⣿⣿⣿⣿⣿⣿⣾⣿⣿⣿⣿⣿⠸⢇⡟⠀⠁⢠⣦⣌⠚⣦⣦⢻\r\n⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣯⣤⣄ ⠀⠀⣬⣑⠟⠁⣰⣿⣿⡇\r\n⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣯⠄⣾⣿⣿⣿⣦⠸⣿⣿⣿\r\n⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⠟⠁⢰⣿⣿⡛⢿⣿⣆⠹⣿⣿\r\n⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⠿⢃⣐⣛⡚⠿⣿⣧⣄⢻⣿⣧⠙⣿\r\n⣿⣿⣿⣿⠿⢛⣛⢛⣛⣡⣾⣿⣿⣿⣿⣷⣮⡙⢿⣦⠹⣿⣷⠘\r\n⣿⣿⡿⢡⣾⣿⣿⣿⣿⢿⣿⣿⣿⣿⣿⣿⣿⣿⣦⠁⠐⠚⠻⣿⠀\r\n⣿⡟⣰⣿⣿⣿⣿⣿⣿⣶⣿⣿⣿⣿⣿⣿⣿⣿⣾⣷⢸⣿⣿⣿\r\n⣿⢠⣿⣿⣿⣿⣿⣿⣿⢿⣿⣟⢿⣿⣿⣿⣿⣿⣿⣿⡄⣿⣿⣿\r\n⣿⢸⣿⣿⣿⣿⣿⣿⢡⣿⣿⣿⠀⣿⣿⣿⣿⣿⣿⣿⠃⣿⣿⣿\r\n⣿⠀⠻⢿⣿⣿⣿⣿⣌⠛⠚⠋⡀⠿⣿⠿⠿⠛⠋⠅⠀⣿⣿⣿\r\n⣿⠀⠀⠀⠀⠈⠉⠉⠁⠀⣧⠰⡇⠀⠀⠀⠀⠀⠀⠀⠀⠀⣿⣿\r\n⣿⡄⠀⠀⠀⠀⠀⠀⠀⢠⣿⡄⠁⠀⠀⠀⠀⠀⠀⠀⠀⣿⣿⣿\r\n⣿⣷⠀⠀⠀⠀⠀⠀⠀⣿⣿⣿⣶⠀⠀⠀⠀⠀⠀⠀⢰⣿⣧⣿\")";
            MessageBox.Show(msg, "Passion Unleashed", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
             
        }
        



        private void btnUpdatePart_Click(object sender, EventArgs e)
        {
            string name = txtName.Text.Trim();
            if (string.IsNullOrEmpty(name))
            {
                lblStatus.Text = "Part name cannot be empty.";
                lblStatus.ForeColor = System.Drawing.Color.Red;
                return;
            }

            if (!inventory.ContainsKey(name))
            {
                lblStatus.Text = $"Part '{name}' does not exist. Use Add to create it.";
                lblStatus.ForeColor = System.Drawing.Color.Red;
                return;
            }

            if (!int.TryParse(txtQuantity.Text.Trim(), out int quantity))
            {
                lblStatus.Text = "Invalid quantity.";
                lblStatus.ForeColor = System.Drawing.Color.Red;
                return;
            }

            string shelf = txtShelf.Text.Trim();
            string type = txtType.Text.Trim();
            string addedOn = inventory[name].AddedOn; // keep original addedOn date

            inventory[name] = new Part
            {
                Name = name,
                Quantity = quantity,
                ShelfNumber = shelf,
                CarPartType = type,
                AddedOn = addedOn
            };

            SaveInventory();
            UpdateInventoryGrid();

            lblStatus.Text = $"Updated part '{name}'.";
            lblStatus.ForeColor = System.Drawing.Color.Green;
        }
        private int maxCapacity = 1000;  // Set your max capacity here

        private void UpdateInventoryCapacity(int currentCount)
        {
            if (currentCount > maxCapacity)
                currentCount = maxCapacity;

            progressBarInventoryCapacity.Maximum = maxCapacity;
            progressBarInventoryCapacity.Value = currentCount;
            lblCapacity.Text = $"{currentCount} / {maxCapacity} parts used";
        }


        private System.Windows.Forms.Timer inventoryTimer;

        private void SetupInventoryTimer()
        {
            inventoryTimer = new System.Windows.Forms.Timer();
            inventoryTimer.Interval = 1000;  // refresh every 1 second (1000 ms)
            inventoryTimer.Tick += InventoryTimer_Tick;
            inventoryTimer.Start();
            
        }

        private void InventoryTimer_Tick(object sender, EventArgs e)
        {
            LoadInventory();
            UpdateInventoryGrid();
            int currentCount = dataGridViewInventory.Rows.Count;
            UpdateInventoryCapacity(currentCount);

            bool internet = IsInternetAvailable();
            lblStatus.Text = internet ? "Internet is available. Full access granted." : "Offline: Only search available.";
            lblStatus.ForeColor = internet ? System.Drawing.Color.Orange : System.Drawing.Color.LightGray;

            // Update Add button
            btnAddPart.Enabled = internet;
            btnAddPart.Text = internet ? "Add Part" : "Add (Disabled)";
            btnAddPart.BackColor = internet ? System.Drawing.Color.Blue : System.Drawing.Color.LightGray;

            // Update Update button
            btnUpdatePart.Enabled = internet;
            btnUpdatePart.Text = internet ? "Update Part" : "Update (Disabled)";
            btnUpdatePart.BackColor = internet ? System.Drawing.Color.Black : System.Drawing.Color.LightGray;

            // Update Remove button
            btnRemovePart.Enabled = internet;
            btnRemovePart.Text = internet ? "Remove Part" : "Remove (Disabled)";
            btnRemovePart.BackColor = internet ? System.Drawing.Color.Red : System.Drawing.Color.LightGray;

            // Always allow search
            btnSearchPart.Enabled = true;
            btnSearchPart.Text = "Search Part";
            btnSearchPart.BackColor = System.Drawing.Color.Orange;
        }




        private void btnCheckInternet_Click(object sender, EventArgs e)
        {
            bool internet = IsInternetAvailable();
             lblStatus.Text = internet ? "Internet is available." : "No internet connection.";
            lblStatus.ForeColor = internet ? System.Drawing.Color.Green : System.Drawing.Color.White;

            if (internet)
            {
                this.BackColor = Color.FromArgb(173, 216, 230); // jakeLightBlue
            }
            else
            {
                this.BackColor = Color.DarkRed; // Full red background
            }
        }

    }
}
