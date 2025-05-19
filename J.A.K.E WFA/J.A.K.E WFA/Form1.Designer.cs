namespace J.A.K.E_WFA
{
    partial class Form1
    {
       
        
        private System.Windows.Forms.DataGridView dataGridViewInventory;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.TextBox txtQuantity;
        private System.Windows.Forms.TextBox txtShelf;
        private System.Windows.Forms.TextBox txtType;
        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.Label lblQuantity;
        private System.Windows.Forms.Label lblShelf;
        private System.Windows.Forms.Label lblType;
        private System.Windows.Forms.Button btnAddPart;
        private System.Windows.Forms.Button btnRemovePart;
        private System.Windows.Forms.Button btnSearchPart;
        private System.Windows.Forms.Button btnUpdatePart;
        private System.Windows.Forms.Button btnCheckInternet;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.Button btnShowCat;
        private System.Windows.Forms.Button btnFemboy;
        private System.Windows.Forms.TextBox txtAsciiOutput;
        private System.Windows.Forms.ProgressBar progressBarInventoryCapacity;
        private System.Windows.Forms.Label lblCapacity;


        private void InitializeComponent()
        {
            this.dataGridViewInventory = new System.Windows.Forms.DataGridView();
            this.txtName = new System.Windows.Forms.TextBox();
            this.txtQuantity = new System.Windows.Forms.TextBox();
            this.txtShelf = new System.Windows.Forms.TextBox();
            this.txtType = new System.Windows.Forms.TextBox();
            this.lblName = new System.Windows.Forms.Label();
            this.lblQuantity = new System.Windows.Forms.Label();
            this.lblShelf = new System.Windows.Forms.Label();
            this.lblType = new System.Windows.Forms.Label();
            this.btnAddPart = new System.Windows.Forms.Button();
            this.btnRemovePart = new System.Windows.Forms.Button();
            this.btnSearchPart = new System.Windows.Forms.Button();
            this.btnUpdatePart = new System.Windows.Forms.Button();
            this.btnCheckInternet = new System.Windows.Forms.Button();
            this.lblStatus = new System.Windows.Forms.Label();
            this.txtAsciiOutput = new System.Windows.Forms.TextBox();
            this.progressBarInventoryCapacity = new System.Windows.Forms.ProgressBar();
            this.lblCapacity = new System.Windows.Forms.Label();

            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewInventory)).BeginInit();
            this.SuspendLayout();

            this.FormBorderStyle = FormBorderStyle.None;

            Panel titleBar = new Panel();
            titleBar.Height = 50;
            titleBar.Dock = DockStyle.Top;
            titleBar.BackColor = Color.FromArgb(0, 80, 160);
            titleBar.MouseDown += (s, e) =>
            {
                if (e.Button == MouseButtons.Left)
                {
                    ReleaseCapture();
                    SendMessage(this.Handle, WM_NCLBUTTONDOWN, HTCAPTION, 0);
                }
            };

            Label lblTitle = new Label();
            lblTitle.Text = "J.A.K.E - Inventory Manager";
            lblTitle.ForeColor = Color.White;
            lblTitle.Font = new Font("Segoe UI Rounded", 10F, FontStyle.Bold);
            lblTitle.AutoSize = true;
            lblTitle.Location = new Point(12, 15);

            Button btnClose = new Button();
            btnClose.Text = "✕";
            btnClose.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnClose.ForeColor = Color.White;
            btnClose.BackColor = Color.FromArgb(220, 30, 30);
            btnClose.FlatStyle = FlatStyle.Flat;
            btnClose.FlatAppearance.BorderSize = 0;
            btnClose.Size = new Size(40, 30);
            btnClose.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnClose.Click += (s, e) => this.Close();
            btnClose.MouseEnter += (s, e) => btnClose.BackColor = Color.Red;
            btnClose.MouseLeave += (s, e) => btnClose.BackColor = Color.FromArgb(220, 30, 30);

            Button btnMinimize = new Button();
            btnMinimize.Text = "─";
            btnMinimize.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnMinimize.ForeColor = Color.White;
            btnMinimize.BackColor = Color.FromArgb(100, 100, 100);
            btnMinimize.FlatStyle = FlatStyle.Flat;
            btnMinimize.FlatAppearance.BorderSize = 0;
            btnMinimize.Size = new Size(40, 30);
            btnMinimize.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnMinimize.Click += (s, e) => this.WindowState = FormWindowState.Minimized;
            btnMinimize.MouseEnter += (s, e) => btnMinimize.BackColor = Color.Gray;
            btnMinimize.MouseLeave += (s, e) => btnMinimize.BackColor = Color.FromArgb(100, 100, 100);

            // Set button positions relative to titleBar width dynamically:
            // Because titleBar is docked, width equals form's client width.
            // Subscribe to titleBar resize to update button positions.
            titleBar.Resize += (s, e) =>
            {
                btnClose.Location = new Point(titleBar.Width - btnClose.Width - 10, 10);
                btnMinimize.Location = new Point(titleBar.Width - btnClose.Width - btnMinimize.Width - 15, 10);
            };

            // Initialize button positions now (in case form is already sized)
            btnClose.Location = new Point(titleBar.Width - btnClose.Width - 10, 10);
            btnMinimize.Location = new Point(titleBar.Width - btnClose.Width - btnMinimize.Width - 15, 10);

            titleBar.Controls.Add(lblTitle);
            titleBar.Controls.Add(btnMinimize);
            titleBar.Controls.Add(btnClose);

            this.Controls.Add(titleBar);
            titleBar.BringToFront();


            // Styling
            Font bubblyFont = new Font("Segoe UI Rounded", 9F, FontStyle.Regular);
            Font bubblyFontBold = new Font("Segoe UI Rounded", 9F, FontStyle.Bold);
            Color jakeOrange = Color.FromArgb(255, 140, 0);
            Color jakeBlue = Color.FromArgb(0, 120, 215);
            Color jakeWhite = Color.White;
            Color jakeGray = Color.LightGray;
            Color jakeDarkBlue = Color.FromArgb(0, 80, 160);
            Color jakeLightBlue = Color.FromArgb(173, 216, 230);
            Color jakeLightOrange = Color.FromArgb(255, 180, 100);

            this.BackColor = jakeLightBlue;
            this.Font = bubblyFont;
            this.ClientSize = new Size(1000, 700);
            this.MinimumSize = new Size(1000, 700);
            this.Name = "Form1";
            this.Text = "J.A.K.E Inventory Manager";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.None;
            this.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, Width, Height, 20, 20));

            this.KeyPreview = true;
            this.KeyDown += Form1_KeyDown;

            // DataGridView
            this.dataGridViewInventory.AllowUserToAddRows = false;
            this.dataGridViewInventory.AllowUserToDeleteRows = false;
            this.dataGridViewInventory.ReadOnly = true;
            this.dataGridViewInventory.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewInventory.BackgroundColor = jakeWhite;
            this.dataGridViewInventory.GridColor = jakeOrange;
            this.dataGridViewInventory.BorderStyle = BorderStyle.FixedSingle;
            this.dataGridViewInventory.RowHeadersVisible = false;
            this.dataGridViewInventory.DefaultCellStyle.Font = bubblyFont;
            this.dataGridViewInventory.DefaultCellStyle.BackColor = jakeWhite;
            this.dataGridViewInventory.AlternatingRowsDefaultCellStyle.BackColor = jakeGray;
            this.dataGridViewInventory.ColumnHeadersDefaultCellStyle.BackColor = jakeBlue;
            this.dataGridViewInventory.ColumnHeadersDefaultCellStyle.ForeColor = jakeWhite;
            this.dataGridViewInventory.ColumnHeadersDefaultCellStyle.Font = bubblyFontBold;
            this.dataGridViewInventory.Columns.AddRange(new DataGridViewColumn[]
            {
        new DataGridViewTextBoxColumn() { HeaderText = "Name", Name = "colName", Width = 150 },
        new DataGridViewTextBoxColumn() { HeaderText = "Quantity", Name = "colQuantity", Width = 80 },
        new DataGridViewTextBoxColumn() { HeaderText = "Shelf", Name = "colShelf", Width = 80 },
        new DataGridViewTextBoxColumn() { HeaderText = "Type", Name = "colType", Width = 150 },
        new DataGridViewTextBoxColumn() { HeaderText = "Added On", Name = "colAddedOn", Width = 120 }
            });
            this.dataGridViewInventory.Location = new Point(20, 70); // moved down
            this.dataGridViewInventory.Size = new Size(600, 500);
            this.dataGridViewInventory.CellDoubleClick += dataGridViewInventory_CellDoubleClick;

            // Input Panel
            Panel panelInputs = new Panel();
            panelInputs.Location = new Point(640, 80); // moved down
            panelInputs.Size = new Size(320, 220);
            panelInputs.BackColor = Color.FromArgb(255, 230, 200);
            panelInputs.Padding = new Padding(10);
            panelInputs.BorderStyle = BorderStyle.FixedSingle;

            // Labels & Textboxes
            Label[] labels = { lblName, lblQuantity, lblShelf, lblType };
            string[] texts = { "Part Code", "Quantity", "Shelf", "Type" };
            TextBox[] inputs = { txtName, txtQuantity, txtShelf, txtType };

            for (int i = 0; i < labels.Length; i++)
            {
                labels[i].Text = texts[i];
                labels[i].AutoSize = true;
                labels[i].Font = bubblyFontBold;
                labels[i].ForeColor = jakeDarkBlue;
                labels[i].Location = new Point(10, 10 + i * 50);

                inputs[i].Size = new Size(270, 25);
                inputs[i].Location = new Point(10, 30 + i * 50);
                inputs[i].Font = bubblyFont;
                inputs[i].BackColor = jakeLightOrange;
                inputs[i].BorderStyle = BorderStyle.FixedSingle;

                panelInputs.Controls.Add(labels[i]);
                panelInputs.Controls.Add(inputs[i]);
            }

            // ASCII Output
            this.txtAsciiOutput.Location = new Point(640, 440);
            this.txtAsciiOutput.Size = new Size(320, 100);
            this.txtAsciiOutput.Font = new Font("Consolas", 9);
            this.txtAsciiOutput.BackColor = Color.Black;
            this.txtAsciiOutput.ForeColor = Color.Lime;
            this.txtAsciiOutput.Multiline = true;
            this.txtAsciiOutput.ScrollBars = ScrollBars.Vertical;

            void StyleBubbleButton(Button btn, string text, int x, int y)
            {
                btn.Text = text;
                btn.Location = new Point(x, y);
                btn.Size = new Size(130, 35);
                btn.BackColor = jakeOrange;
                btn.ForeColor = jakeWhite;
                btn.FlatStyle = FlatStyle.Flat;
                btn.Font = bubblyFontBold;
                btn.FlatAppearance.BorderSize = 0;
                btn.Cursor = Cursors.Hand;

                btn.MouseEnter += (s, e) => btn.BackColor = Color.FromArgb(255, 160, 40);
                btn.MouseLeave += (s, e) => btn.BackColor = jakeOrange;
            }

            Button btnToggleDarkMode = new Button();
            btnToggleDarkMode.Text = "Toggle Dark Mode";
            btnToggleDarkMode.Location = new Point(20, 610);
            btnToggleDarkMode.Size = new Size(200, 30);
            btnToggleDarkMode.BackColor = Color.Gray;
            btnToggleDarkMode.ForeColor = Color.White;
            btnToggleDarkMode.FlatStyle = FlatStyle.Flat;
            btnToggleDarkMode.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnToggleDarkMode.Click += (s, e) =>
            {
                isDarkMode = !isDarkMode;
                if (isDarkMode)
                    EnableDarkMode();
                else
                    EnableLightMode(); // You define this to revert styling
            };
            this.Controls.Add(btnToggleDarkMode);


            void EnableLightMode()
            {
                this.BackColor = Color.FromArgb(173, 216, 230); // jakeLightBlue

                foreach (Control ctrl in this.Controls)
                {
                    if (ctrl is Panel || ctrl is GroupBox)
                    {
                        ctrl.BackColor = Color.FromArgb(255, 230, 200);
                        foreach (Control child in ctrl.Controls)
                        {
                            if (child is Label || child is Button || child is TextBox)
                            {
                                child.ForeColor = Color.Black;
                                child.BackColor = child is TextBox ? Color.White : Color.FromArgb(255, 140, 0);
                            }
                        }
                    }
                    else if (ctrl is Label || ctrl is Button || ctrl is TextBox)
                    {
                        ctrl.ForeColor = Color.White;
                        ctrl.BackColor = ctrl is TextBox ? Color.White : Color.FromArgb(255, 140, 0);
                    }
                }

                dataGridViewInventory.BackgroundColor = Color.White;
                dataGridViewInventory.DefaultCellStyle.BackColor = Color.White;
                dataGridViewInventory.DefaultCellStyle.ForeColor = Color.Black;
                dataGridViewInventory.AlternatingRowsDefaultCellStyle.BackColor = Color.LightGray;
                dataGridViewInventory.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(0, 120, 215);
                dataGridViewInventory.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
                dataGridViewInventory.GridColor = Color.Orange;

                txtAsciiOutput.BackColor = Color.Black;
                txtAsciiOutput.ForeColor = Color.Lime;

                lblCapacity.ForeColor = Color.Black;
                lblStatus.ForeColor = Color.Black;
            }

            Button btnHelp = new Button();
            btnHelp.Text = "Help";
            btnHelp.Location = new Point(320, 610); // Adjust position as needed
            btnHelp.Size = new Size(180, 50); // Bigger size
            btnHelp.BackColor = Color.FromArgb(100, 149, 237); // Cornflower Blue
            btnHelp.ForeColor = Color.White;
            btnHelp.FlatStyle = FlatStyle.Flat;
            btnHelp.Font = bubblyFontBold;
            btnHelp.FlatAppearance.BorderSize = 0;
            btnHelp.Cursor = Cursors.Hand;

            // Hover effect (optional)
            btnHelp.MouseEnter += (s, e) => btnHelp.BackColor = Color.FromArgb(65, 105, 225);
            btnHelp.MouseLeave += (s, e) => btnHelp.BackColor = Color.FromArgb(100, 149, 237);

            btnHelp.Click += (s, e) =>
            {
                string helpText = "🛠 HOW TO USE J.A.K.E\n\n" +
                                   "• The main panle is what is use to interact with buttons.\n\n" +
                                  "  Double clicking in the grid will pop all info into the main panle .\n\n" +
                                  "• Add Part: Enter Name, Quantity, Shelf, Type all into the main panle → Click 'Add Part'.\n" +
                                  "• Remove Part: Select a row (dobule click) → Click 'Remove Part'.\n" +
                                  "• Update Part: Double-click a row → Edit in main panel → Click 'Update Part'.\n" +
                                  "• Search: Add the part code into part code and Click 'Search Part' it will fill in all info on the part if we have it.\n" +
                                  "• Check Internet: Check if connected.\n" +
                                  "• ASCII Output: Displays special messages or logs.\n\n" +
                                  "   .\n\n" +
                                  "For help, contact  Jake 😎" +
                                  "Email me at jakehohn05@gmail.com or call at 920-240-5807";
                MessageBox.Show(helpText, "J.A.K.E Help", MessageBoxButtons.OK, MessageBoxIcon.Information);
            };

            this.Controls.Add(btnHelp);


            void EnableDarkMode()
            {
                Color darkBack = Color.FromArgb(30, 30, 30);
                Color darkFore = Color.White;
                Color darkAccent = Color.FromArgb(60, 120, 200);
                Color darkPanel = Color.FromArgb(45, 45, 48);
                Color gridAltRow = Color.FromArgb(50, 50, 50);

                this.BackColor = darkBack;

                foreach (Control ctrl in this.Controls)
                {
                    if (ctrl is Panel || ctrl is GroupBox)
                    {
                        ctrl.BackColor = darkPanel;
                        foreach (Control child in ctrl.Controls)
                        {
                            if (child is Label || child is Button || child is TextBox)
                            {
                                child.ForeColor = darkFore;
                                child.BackColor = child is TextBox ? Color.FromArgb(60, 60, 60) : darkAccent;
                            }
                        }
                    }
                    else if (ctrl is Label || ctrl is Button || ctrl is TextBox)
                    {
                        ctrl.ForeColor = darkFore;
                        ctrl.BackColor = ctrl is TextBox ? Color.FromArgb(60, 60, 60) : darkAccent;
                    }
                }

                // DataGridView styling
                dataGridViewInventory.BackgroundColor = darkPanel;
                dataGridViewInventory.DefaultCellStyle.BackColor = darkBack;
                dataGridViewInventory.DefaultCellStyle.ForeColor = darkFore;
                dataGridViewInventory.AlternatingRowsDefaultCellStyle.BackColor = gridAltRow;
                dataGridViewInventory.ColumnHeadersDefaultCellStyle.BackColor = darkAccent;
                dataGridViewInventory.ColumnHeadersDefaultCellStyle.ForeColor = darkFore;
                dataGridViewInventory.GridColor = Color.Gray;

                // ASCII box
                txtAsciiOutput.BackColor = Color.Black;
                txtAsciiOutput.ForeColor = Color.Lime;

                // Progress bar tweak if needed
                progressBarInventoryCapacity.ForeColor = darkAccent;

                // Footer
                lblCapacity.ForeColor = darkFore;
                lblStatus.ForeColor = darkFore;
            }


            StyleBubbleButton(btnAddPart, "Add Part", 640, 320);
            StyleBubbleButton(btnRemovePart, "Remove Part", 800, 320);
            StyleBubbleButton(btnSearchPart, "Search Part", 640, 365);
            StyleBubbleButton(btnUpdatePart, "Update Part", 800, 365);

            btnAddPart.Click += btnAddPart_Click;
            btnRemovePart.Click += btnRemovePart_Click;
            btnSearchPart.Click += btnSearchPart_Click;
            btnUpdatePart.Click += btnUpdatePart_Click;

            btnCheckInternet.Text = "Check Internet";
            btnCheckInternet.Location = new Point(650, 600);
            btnCheckInternet.Size = new Size(270, 30);
            btnCheckInternet.BackColor = jakeOrange;
            btnCheckInternet.ForeColor = jakeWhite;
            btnCheckInternet.FlatStyle = FlatStyle.Flat;
            btnCheckInternet.Font = bubblyFontBold;
            btnCheckInternet.FlatAppearance.BorderSize = 0;
            btnCheckInternet.Cursor = Cursors.Hand;
            btnCheckInternet.Click += btnCheckInternet_Click;

            lblStatus.Text = "Status";
            lblStatus.AutoSize = true;
            lblStatus.Location = new Point(20, 650);
            lblStatus.ForeColor = jakeDarkBlue;

            progressBarInventoryCapacity.Location = new Point(100, 100);
            progressBarInventoryCapacity.Size = new Size(600, 30);
            progressBarInventoryCapacity.Maximum = 1000;
            progressBarInventoryCapacity.Value = 0;
            progressBarInventoryCapacity.Style = ProgressBarStyle.Continuous;
            progressBarInventoryCapacity.ForeColor = jakeOrange;
            progressBarInventoryCapacity.BringToFront();
            lblCapacity.Location = new Point(20, 580);
            lblCapacity.Size = new Size(600, 25);
            lblCapacity.Font = bubblyFontBold;
            lblCapacity.Text = "0 / 100 parts used";
            lblCapacity.ForeColor = jakeDarkBlue;

            Label lblFooterCompany = new Label
            {
                Text = "Free to use",
                Font = new Font("Segoe UI Rounded", 8F, FontStyle.Bold),
                ForeColor = jakeDarkBlue,
                AutoSize = true,
                Location = new Point(20, 5)
            };

            Label lblFooterCopyright = new Label
            {
                Text = "© 2025 Jake John Wenninger - v1.2",
                Font = new Font("Segoe UI Rounded", 8F, FontStyle.Bold),
                ForeColor = jakeDarkBlue,
                AutoSize = true,
                Location = new Point(700, 5)
            };

            Panel footerPanel = new Panel
            {
                Height = 30,
                Dock = DockStyle.Bottom,
                BackColor = Color.FromArgb(220, 230, 245)
            };
      

            footerPanel.Controls.Add(lblFooterCompany);
            footerPanel.Controls.Add(lblFooterCopyright);

             


            // Create a label to show the inventory source path
            Label inventorySourceLabel = new Label();
            inventorySourceLabel.Text = "Loading inventory from: C:\\Path\\To\\Inventory.csv";
            inventorySourceLabel.ForeColor = Color.Black; // or Color.Black for light mode
            inventorySourceLabel.BackColor = Color.Transparent;
            inventorySourceLabel.Font = new Font("Segoe UI", 9, FontStyle.Italic);
            inventorySourceLabel.AutoSize = true;
            inventorySourceLabel.Location = new Point(10, 51); // Adjust Y as needed based on your title bar
            string inventoryPath = FindInventoryFilePath();

            if (!string.IsNullOrEmpty(inventoryPath))
            {
                inventorySourceLabel.Text = $"Loading inventory from: {inventoryPath}";
            }
            else
            {
                inventorySourceLabel.Text = "No inventory file loaded.";
            }

            // Add it to the form
            this.Controls.Add(inventorySourceLabel);


            this.Controls.Add(dataGridViewInventory);
            this.Controls.Add(panelInputs);
            this.Controls.Add(btnAddPart);
            this.Controls.Add(btnRemovePart);
            this.Controls.Add(btnSearchPart);
            this.Controls.Add(btnUpdatePart);
            this.Controls.Add(btnCheckInternet);
            this.Controls.Add(txtAsciiOutput);
            this.Controls.Add(progressBarInventoryCapacity);
            this.Controls.Add(lblCapacity);
            this.Controls.Add(lblStatus);
            this.Controls.Add(footerPanel);

            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewInventory)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

    }
}
