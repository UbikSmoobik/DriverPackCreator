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
            progressBar1 = new ProgressBar();
            SuspendLayout();
            // 
            // SourceDirectory
            // 
            SourceDirectory.Location = new Point(209, 91);
            SourceDirectory.Name = "SourceDirectory";
            SourceDirectory.Size = new Size(260, 23);
            SourceDirectory.TabIndex = 0;
            // 
            // BrowseSource
            // 
            BrowseSource.Location = new Point(128, 88);
            BrowseSource.Name = "BrowseSource";
            BrowseSource.Size = new Size(75, 23);
            BrowseSource.TabIndex = 1;
            BrowseSource.Text = "Browse...";
            BrowseSource.UseVisualStyleBackColor = true;
            BrowseSource.Click += btnBrowseSource_Click;
            // 
            // DestinationDirectory
            // 
            DestinationDirectory.Location = new Point(209, 117);
            DestinationDirectory.Name = "DestinationDirectory";
            DestinationDirectory.Size = new Size(260, 23);
            DestinationDirectory.TabIndex = 2;
            // 
            // BrowseDestination
            // 
            BrowseDestination.Location = new Point(128, 117);
            BrowseDestination.Name = "BrowseDestination";
            BrowseDestination.Size = new Size(75, 23);
            BrowseDestination.TabIndex = 3;
            BrowseDestination.Text = "Browse...";
            BrowseDestination.UseVisualStyleBackColor = true;
            BrowseDestination.Click += btnBrowseDestination_Click;
            // 
            // Extract
            // 
            Extract.Location = new Point(264, 179);
            Extract.Name = "Extract";
            Extract.Size = new Size(122, 23);
            Extract.TabIndex = 4;
            Extract.Text = "Extract";
            Extract.UseVisualStyleBackColor = true;
            Extract.Click += btnExtract_Click;
            // 
            // progressBar1
            // 
            progressBar1.Location = new Point(128, 255);
            progressBar1.Name = "progressBar1";
            progressBar1.Size = new Size(341, 23);
            progressBar1.TabIndex = 5;
            progressBar1.Visible = false;
            // 
            // Form1
            // 
            ClientSize = new Size(618, 336);
            Controls.Add(Extract);
            Controls.Add(BrowseDestination);
            Controls.Add(DestinationDirectory);
            Controls.Add(BrowseSource);
            Controls.Add(SourceDirectory);
            Controls.Add(progressBar1);
            Name = "Form1";
            Text = "Driver Pack Creator";
            ResumeLayout(false);
            PerformLayout();
        }

        private ProgressBar progressBar1;
    }
}
