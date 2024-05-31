namespace DriverPackCreator
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.TextBox SourceDirectory;
        private System.Windows.Forms.Button BrowseSource;
        private System.Windows.Forms.TextBox DestinationDirectory;
        private System.Windows.Forms.Button BrowseDestination;
        private System.Windows.Forms.Button Extract;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            SourceDirectory = new TextBox();
            BrowseSource = new Button();
            DestinationDirectory = new TextBox();
            BrowseDestination = new Button();
            Extract = new Button();
            Exit = new Button();
            pictureBox1 = new PictureBox();
            lblStatus = new Label();
            lblValid = new Label();
            lblCurrentFile = new Label();
            radExtractFromSystem = new RadioButton();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            SuspendLayout();
            // 
            // SourceDirectory
            // 
            SourceDirectory.Location = new Point(253, 12);
            SourceDirectory.Name = "SourceDirectory";
            SourceDirectory.Size = new Size(260, 23);
            SourceDirectory.TabIndex = 0;
            SourceDirectory.Text = "C:\\Windows\\System32\\DriverStore\\FileRepository";
            // 
            // BrowseSource
            // 
            BrowseSource.Location = new Point(172, 11);
            BrowseSource.Name = "BrowseSource";
            BrowseSource.Size = new Size(75, 23);
            BrowseSource.TabIndex = 1;
            BrowseSource.Text = "Source";
            BrowseSource.UseVisualStyleBackColor = true;
            BrowseSource.Click += btnBrowseSource_Click;
            // 
            // DestinationDirectory
            // 
            DestinationDirectory.Location = new Point(253, 64);
            DestinationDirectory.Name = "DestinationDirectory";
            DestinationDirectory.Size = new Size(260, 23);
            DestinationDirectory.TabIndex = 2;
            // 
            // BrowseDestination
            // 
            BrowseDestination.Location = new Point(172, 64);
            BrowseDestination.Name = "BrowseDestination";
            BrowseDestination.Size = new Size(75, 23);
            BrowseDestination.TabIndex = 3;
            BrowseDestination.Text = "Destination";
            BrowseDestination.UseVisualStyleBackColor = true;
            BrowseDestination.Click += btnBrowseDestination_Click;
            // 
            // Extract
            // 
            Extract.Location = new Point(519, 35);
            Extract.Name = "Extract";
            Extract.Size = new Size(65, 23);
            Extract.TabIndex = 4;
            Extract.Text = "Create";
            Extract.UseVisualStyleBackColor = true;
            Extract.Click += btnExtract_Click;
            // 
            // Exit
            // 
            Exit.Location = new Point(548, 168);
            Exit.Name = "Exit";
            Exit.Size = new Size(36, 23);
            Exit.TabIndex = 8;
            Exit.Text = "Exit";
            Exit.UseVisualStyleBackColor = true;
            Exit.Click += Exit_Click;
            // 
            // pictureBox1
            // 
            pictureBox1.Image = (Image)resources.GetObject("pictureBox1.Image");
            pictureBox1.Location = new Point(12, 12);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(100, 179);
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox1.TabIndex = 9;
            pictureBox1.TabStop = false;
            // 
            // lblStatus
            // 
            lblStatus.AutoSize = true;
            lblStatus.Font = new Font("Segoe UI", 11.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblStatus.ForeColor = Color.Lime;
            lblStatus.Location = new Point(172, 138);
            lblStatus.Name = "lblStatus";
            lblStatus.Size = new Size(13, 20);
            lblStatus.TabIndex = 10;
            lblStatus.Text = " ";
            // 
            // lblValid
            // 
            lblValid.AutoSize = true;
            lblValid.Font = new Font("Segoe UI", 14.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblValid.ForeColor = Color.Red;
            lblValid.Location = new Point(159, 113);
            lblValid.Name = "lblValid";
            lblValid.Size = new Size(17, 25);
            lblValid.TabIndex = 11;
            lblValid.Text = " ";
            // 
            // lblCurrentFile
            // 
            lblCurrentFile.AutoSize = true;
            lblCurrentFile.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblCurrentFile.ForeColor = Color.Yellow;
            lblCurrentFile.Location = new Point(12, 199);
            lblCurrentFile.Name = "lblCurrentFile";
            lblCurrentFile.Size = new Size(12, 17);
            lblCurrentFile.TabIndex = 12;
            lblCurrentFile.Text = " ";
            // 
            // radExtractFromSystem
            // 
            radExtractFromSystem.AutoSize = true;
            radExtractFromSystem.Location = new Point(173, 39);
            radExtractFromSystem.Name = "radExtractFromSystem";
            radExtractFromSystem.Size = new Size(168, 19);
            radExtractFromSystem.TabIndex = 13;
            radExtractFromSystem.TabStop = true;
            radExtractFromSystem.Text = "Extract drivers from system";
            radExtractFromSystem.UseVisualStyleBackColor = true;
            radExtractFromSystem.CheckedChanged += radExtractFromSystem_CheckedChanged;
            // 
            // Form1
            // 
            BackColor = SystemColors.ControlDarkDark;
            ClientSize = new Size(595, 226);
            Controls.Add(radExtractFromSystem);
            Controls.Add(lblCurrentFile);
            Controls.Add(lblValid);
            Controls.Add(lblStatus);
            Controls.Add(pictureBox1);
            Controls.Add(Exit);
            Controls.Add(Extract);
            Controls.Add(BrowseDestination);
            Controls.Add(DestinationDirectory);
            Controls.Add(BrowseSource);
            Controls.Add(SourceDirectory);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Name = "Form1";
            Text = "Driver Pack Creator";
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        private Button Exit;
        private PictureBox pictureBox1;
        private Label lblStatus;
        private Label lblValid;
        private Label lblCurrentFile;
        private RadioButton radExtractFromSystem;
    }
}
