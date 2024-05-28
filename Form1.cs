using System;
using System.ComponentModel;
using System.IO;
using System.Windows.Forms;
using System.IO.Compression;


namespace DriverPackCreator
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btnBrowseSource_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog fbd = new FolderBrowserDialog())
            {
                if (fbd.ShowDialog() == DialogResult.OK)
                {
                    SourceDirectory.Text = fbd.SelectedPath;
                }
            }
        }

        private void btnBrowseDestination_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog fbd = new FolderBrowserDialog())
            {
                if (fbd.ShowDialog() == DialogResult.OK)
                {
                    DestinationDirectory.Text = fbd.SelectedPath;
                }
            }
        }

        private void GenerateReadme(string destinationDirectory)
        {
            string readmeFilePath = Path.Combine(destinationDirectory, "readme.txt");

            using (StreamWriter writer = new StreamWriter(readmeFilePath))
            {
                // Write header
                writer.WriteLine("Stone Driver Package");
                writer.WriteLine("__________________________________________________________________________________________________________");
                writer.WriteLine();
                writer.WriteLine("Package type:\t\tSCCM driver package");
                writer.WriteLine("Date:\t\t\t" + DateTime.Today.ToString("dd/MM/yyyy"));
                writer.WriteLine("__________________________________________________________________________________________________________");
                writer.WriteLine();
                writer.WriteLine("Intended use:");
                writer.WriteLine();
                writer.WriteLine("This driver package provides the base drivers.");
                writer.WriteLine("System administrators can utilise this to deploy Windows images with Microsoft System Center Configuration Manager");
                writer.WriteLine("(SCCM) by importing the device driver package into their SCCM environment.");
                writer.WriteLine("__________________________________________________________________________________________________________");
                writer.WriteLine();

                // Write driver file details
                foreach (var file in Directory.GetFiles(destinationDirectory, "*.*", SearchOption.AllDirectories))
                {
                    string relativePath = Path.GetRelativePath(destinationDirectory, file);
                    writer.WriteLine("FileName: " + relativePath);
                    // Extract and display additional information if needed (e.g., Class, Provider, Driver Date, Driver Ver)
                }

                // Write limitations
                writer.WriteLine();
                writer.WriteLine("LIMITATIONS");
                writer.WriteLine();
                writer.WriteLine("* Nvidia and ATI drivers are not included in this package because they are not compatible with this method of deployment.");
                writer.WriteLine("  Administrators will need to create a package that performs a silent install using the executable file and");
                writer.WriteLine("  add it to the task sequence separately.");
                writer.WriteLine("__________________________________________________________________________________________________________");
            }
        }

        private void btnExtract_Click(object sender, EventArgs e)
        {
            string sourceDirectory = SourceDirectory.Text;
            string destinationDirectory = DestinationDirectory.Text;

            if (Directory.Exists(sourceDirectory) && Directory.Exists(destinationDirectory))
            {
                // Show the progress bar
                progressBar1.Visible = true;

                // Start the extraction process in a background thread
                BackgroundWorker worker = new BackgroundWorker();
                worker.WorkerReportsProgress = true;
                worker.DoWork += (sender, e) =>
                {
                    ExtractAndOrganizeFiles(sourceDirectory, destinationDirectory, worker);
                };
                worker.ProgressChanged += (sender, e) =>
                {
                    // Cap the progress value at 100 if it exceeds that value
                    progressBar1.Value = Math.Min(e.ProgressPercentage, 100);
                };
                worker.RunWorkerCompleted += (sender, e) =>
                {
                    progressBar1.Visible = false; // Hide the progress bar when extraction is complete
                    GenerateReadme(destinationDirectory);
                    MessageBox.Show("Extraction and organization complete!");
                };
                worker.RunWorkerAsync();
            }
            else
            {
                MessageBox.Show("Please select valid directories.");
            }
        }




        private void ExtractAndOrganizeFiles(string sourceDirectory, string destinationDirectory, BackgroundWorker worker)
        {
            string[] fileExtensions = { ".inf", ".cat", ".dat", ".sys" };

            // Get all files with specified extensions from source directory and subdirectories
            var files = Directory.GetFiles(sourceDirectory, "*.*", SearchOption.AllDirectories)
                                 .Where(file => fileExtensions.Contains(Path.GetExtension(file).ToLower()));

            int totalFiles = files.Count();
            int filesProcessed = 0;

            foreach (var file in files)
            {
                // Get the directory name where the file is located
                string parentDirectory = Path.GetFileName(Path.GetDirectoryName(file));

                // Create new directory name prefixed with "SCCM"
                string newDirectoryName = $"SCCM_{parentDirectory}";
                string newDirectoryPath = Path.Combine(destinationDirectory, newDirectoryName);

                // Create the new directory if it doesn't exist
                if (!Directory.Exists(newDirectoryPath))
                {
                    Directory.CreateDirectory(newDirectoryPath);
                }

                // Copy the file to the new directory
                string newFilePath = Path.Combine(newDirectoryPath, Path.GetFileName(file));
                File.Copy(file, newFilePath, true);

                // Report progress for file extraction
                filesProcessed++;
                int progressPercentage = (int)((float)filesProcessed / totalFiles * 100);
                worker.ReportProgress(progressPercentage);
            }

            // Create a ZIP file containing all the SCCM folders
            string zipFilePath = Path.Combine(destinationDirectory, "DriverPack.Zip");

            // Count the total number of files in all SCCM folders for progress estimation
            int totalZipFiles = Directory.GetFiles(destinationDirectory, "*.*", SearchOption.AllDirectories)
                                         .Where(file => !file.StartsWith(zipFilePath)) // Exclude the ZIP file itself
                                         .Count();

            using (var zipArchive = ZipFile.Open(zipFilePath, ZipArchiveMode.Create))
            {
                // Add each SCCM folder and its contents to the ZIP archive
                var sccmFolders = Directory.GetDirectories(destinationDirectory, "SCCM_*", SearchOption.TopDirectoryOnly);
                foreach (var sccmFolder in sccmFolders)
                {
                    AddFolderToZip(zipArchive, sccmFolder, worker, ref filesProcessed, totalZipFiles);
                }
            }
        }


        private void AddFolderToZip(ZipArchive zipArchive, string folderPath, BackgroundWorker worker, ref int filesProcessed, int totalFiles)
        {
            string folderName = Path.GetFileName(folderPath);

            // Create an entry for the folder
            ZipArchiveEntry folderEntry = zipArchive.CreateEntry(folderName + "/");

            // Add files from the folder to the archive
            foreach (string file in Directory.GetFiles(folderPath))
            {
                string relativePath = Path.Combine(folderName, Path.GetFileName(file));
                zipArchive.CreateEntryFromFile(file, relativePath);

                // Report progress for ZIP file creation
                filesProcessed++;
                int progressPercentage = (int)((float)filesProcessed / totalFiles * 100);
                worker.ReportProgress(progressPercentage);
            }

            // Recursively add subfolders
            foreach (string subfolder in Directory.GetDirectories(folderPath))
            {
                AddFolderToZip(zipArchive, subfolder, worker, ref filesProcessed, totalFiles);
            }
        }



    }
}