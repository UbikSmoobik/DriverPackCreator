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
            SourceDirectory = new TextBox();
            BrowseSource = new Button();
            DestinationDirectory = new TextBox();
            BrowseDestination = new Button();
            Extract = new Button();
            lblStatus = new Label();
            lblValid = new Label();
            lblCurrentFile = new Label();
            radExtractFromSystem = new RadioButton();
            SuspendLayout();
            // 
            // SourceDirectory
            // 
            SourceDirectory.Location = new Point(92, 37);
            SourceDirectory.Name = "SourceDirectory";
            SourceDirectory.Size = new Size(260, 23);
            SourceDirectory.TabIndex = 0;
            SourceDirectory.Text = "C:\\Windows\\System32\\DriverStore\\FileRepository";
            // 
            // BrowseSource
            // 
            BrowseSource.Location = new Point(11, 36);
            BrowseSource.Name = "BrowseSource";
            BrowseSource.Size = new Size(75, 23);
            BrowseSource.TabIndex = 1;
            BrowseSource.Text = "Source";
            BrowseSource.UseVisualStyleBackColor = true;
            BrowseSource.Click += btnBrowseSource_Click;
            // 
            // DestinationDirectory
            // 
            DestinationDirectory.Location = new Point(92, 65);
            DestinationDirectory.Name = "DestinationDirectory";
            DestinationDirectory.Size = new Size(260, 23);
            DestinationDirectory.TabIndex = 2;
            // 
            // BrowseDestination
            // 
            BrowseDestination.Location = new Point(11, 65);
            BrowseDestination.Name = "BrowseDestination";
            BrowseDestination.Size = new Size(75, 23);
            BrowseDestination.TabIndex = 3;
            BrowseDestination.Text = "Destination";
            BrowseDestination.UseVisualStyleBackColor = true;
            BrowseDestination.Click += btnBrowseDestination_Click;
            // 
            // Extract
            // 
            Extract.Location = new Point(359, 36);
            Extract.Name = "Extract";
            Extract.Size = new Size(69, 52);
            Extract.TabIndex = 4;
            Extract.Text = "Create";
            Extract.UseVisualStyleBackColor = true;
            Extract.Click += btnExtract_Click;
            // 
            // lblStatus
            // 
            lblStatus.AutoSize = true;
            lblStatus.Font = new Font("Segoe UI", 11.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblStatus.ForeColor = Color.Lime;
            lblStatus.Location = new Point(11, 171);
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
            lblValid.Location = new Point(12, 91);
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
            lblCurrentFile.Location = new Point(12, 200);
            lblCurrentFile.Name = "lblCurrentFile";
            lblCurrentFile.Size = new Size(12, 17);
            lblCurrentFile.TabIndex = 12;
            lblCurrentFile.Text = " ";
            // 
            // radExtractFromSystem
            // 
            radExtractFromSystem.AutoSize = true;
            radExtractFromSystem.Location = new Point(12, 11);
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
            ClientSize = new Size(448, 226);
            Controls.Add(radExtractFromSystem);
            Controls.Add(lblCurrentFile);
            Controls.Add(lblValid);
            Controls.Add(lblStatus);
            Controls.Add(Extract);
            Controls.Add(BrowseDestination);
            Controls.Add(DestinationDirectory);
            Controls.Add(BrowseSource);
            Controls.Add(SourceDirectory);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Name = "Form1";
            Text = "Driver Pack Creator";
            ResumeLayout(false);
            PerformLayout();
        }

        private Label lblStatus;
        private Label lblValid;
        private Label lblCurrentFile;
        private RadioButton radExtractFromSystem;
    }
}
