using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
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
            string readmeFilePath = Path.Combine(destinationDirectory, "README.txt");
            string readmeContent = "This folder contains SCCM driver packs extracted using DriverPackCreator.";
            File.WriteAllText(readmeFilePath, readmeContent);
        }

        private void btnExtract_Click(object sender, EventArgs e)
        {
            lblValid.Text = " ";

            string sourceDirectory = SourceDirectory.Text;
            string destinationDirectory = DestinationDirectory.Text;

            if (!IsValidDirectory(destinationDirectory))
            {
                lblValid.Text = "Please select a valid destination directory.";
                return;
            }

            lblCurrentFile.Visible = true;

            if (radExtractFromSystem.Checked)
            {
                if (!string.IsNullOrEmpty(destinationDirectory))
                {
                    lblCurrentFile.Text = "Running DISM...";

                    BackgroundWorker dismWorker = new BackgroundWorker();
                    dismWorker.WorkerReportsProgress = true;
                    dismWorker.DoWork += (s, args) =>
                    {
                        ExtractDriversFromSystem(destinationDirectory, dismWorker);
                    };
                    dismWorker.RunWorkerCompleted += (s, args) =>
                    {
                        lblCurrentFile.Visible = false;
                        GenerateReadme(destinationDirectory);
                        MoveDriverPackZip(destinationDirectory);
                        MoveFilesToReadyForDriverfinder(destinationDirectory);
                        DeleteSCCMFolders(destinationDirectory);
                        lblStatus.Text = "Extraction and organization complete!";
                        OpenDestinationFolder(destinationDirectory);
                    };
                    dismWorker.RunWorkerAsync();
                }
                else
                {
                    lblValid.Text = "Please select a destination directory.";
                    return;
                }
            }
            else
            {
                if (!IsValidDirectory(sourceDirectory))
                {
                    lblValid.Text = "Please select a valid source directory.";
                    return;
                }

                BackgroundWorker worker = new BackgroundWorker();
                worker.WorkerReportsProgress = true;
                worker.DoWork += (s, args) =>
                {
                    ExtractAndOrganizeFiles(sourceDirectory, destinationDirectory, worker);
                };
                worker.ProgressChanged += (s, args) =>
                {
                    if (args.UserState is string currentFile)
                    {
                        lblCurrentFile.Text = $"Current File: {currentFile}";
                    }
                };
                worker.RunWorkerCompleted += (s, args) =>
                {
                    lblCurrentFile.Visible = false;
                    GenerateReadme(destinationDirectory);
                    MoveDriverPackZip(destinationDirectory);
                    MoveFilesToReadyForDriverfinder(destinationDirectory);
                    DeleteSCCMFolders(destinationDirectory);
                    lblStatus.Text = "Extraction and organization complete!";
                    OpenDestinationFolder(destinationDirectory);
                };
                worker.RunWorkerAsync();
            }
        }

        private bool IsValidDirectory(string directoryPath)
        {
            return !string.IsNullOrEmpty(directoryPath) && Directory.Exists(directoryPath);
        }

        private void ExtractDriversFromSystem(string destinationDirectory, BackgroundWorker dismWorker)
        {
            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                FileName = "dism.exe",
                Arguments = $"/Online /Export-Driver /Destination:\"{destinationDirectory}\"",
                UseShellExecute = false,
                RedirectStandardOutput = true,
                CreateNoWindow = true
            };

            using (Process process = new Process { StartInfo = startInfo })
            {
                process.OutputDataReceived += (sender, e) =>
                {
                    if (!string.IsNullOrEmpty(e.Data) && !e.Data.Contains("Version"))
                    {
                        Invoke((Action)(() =>
                        {
                            lblCurrentFile.Text = e.Data;
                        }));
                    }
                };

                process.Start();
                process.BeginOutputReadLine();
                process.WaitForExit();
                DismOrganizeFiles(destinationDirectory, dismWorker);
            }

            lblCurrentFile.Invoke((MethodInvoker)delegate {
                lblCurrentFile.Text = " ";
            });
        }

        private void radExtractFromSystem_CheckedChanged(object sender, EventArgs e)
        {
            if (radExtractFromSystem.Checked)
            {
                SourceDirectory.Enabled = false;
                SourceDirectory.Text = string.Empty;
                BrowseSource.Enabled = false;
            }
            else
            {
                SourceDirectory.Enabled = true;
                BrowseSource.Enabled = true;
            }
        }

        private void ExtractAndOrganizeFiles(string sourceDirectory, string destinationDirectory, BackgroundWorker worker)
        {
            string[] fileExtensions = { ".inf", ".cat", ".dat", ".sys", ".cab" };

            var files = Directory.GetFiles(sourceDirectory, "*.*", SearchOption.AllDirectories)
                                 .Where(file => fileExtensions.Contains(Path.GetExtension(file).ToLower()));

            int totalFiles = files.Count();
            int filesProcessed = 0;

            foreach (var file in files)
            {
                string parentDirectory = Path.GetFileName(Path.GetDirectoryName(file));
                string newDirectoryName = $"SCCM_{parentDirectory}";
                string newDirectoryPath = Path.Combine(destinationDirectory, newDirectoryName);

                if (!Directory.Exists(newDirectoryPath))
                {
                    Directory.CreateDirectory(newDirectoryPath);
                }

                string newFilePath = Path.Combine(newDirectoryPath, Path.GetFileName(file));
                File.Copy(file, newFilePath, true);

                filesProcessed++;
                int progressPercentage = (int)((float)filesProcessed / totalFiles * 100);
                worker.ReportProgress(progressPercentage, Path.GetFileName(file));

                lblCurrentFile.Invoke((MethodInvoker)delegate {
                    lblCurrentFile.Text = $"Preparing folder for driver pack: {newDirectoryName}";
                });
            }

            string zipFilePath = Path.Combine(destinationDirectory, "DriverPack.Zip");
            if (!File.Exists(zipFilePath))
            {
                using (var zipArchive = ZipFile.Open(zipFilePath, ZipArchiveMode.Create))
                {
                    var sccmFolders = Directory.GetDirectories(destinationDirectory, "SCCM_*", SearchOption.TopDirectoryOnly);
                    foreach (var sccmFolder in sccmFolders)
                    {
                        AddSCCMFoldersToZip(zipArchive, sccmFolder, worker, ref filesProcessed, totalFiles);
                    }
                }
            }

            var sourceFolders = Directory.GetDirectories(sourceDirectory);
            foreach (var folder in sourceFolders)
            {
                string individualZipFilePath = Path.Combine(destinationDirectory, $"{Path.GetFileName(folder)}.zip");
                using (var individualZipArchive = ZipFile.Open(individualZipFilePath, ZipArchiveMode.Create))
                {
                    AddFolderContentsToZip(individualZipArchive, folder, worker, ref filesProcessed, totalFiles);
                }
            }
        }

        private void AddSCCMFoldersToZip(ZipArchive zipArchive, string folderPath, BackgroundWorker worker, ref int filesProcessed, int totalFiles)
        {
            string folderName = Path.GetFileName(folderPath);
            ZipArchiveEntry folderEntry = zipArchive.CreateEntry(folderName + "/");

            foreach (string file in Directory.GetFiles(folderPath))
            {
                lblCurrentFile.Invoke((MethodInvoker)delegate
                {
                    lblCurrentFile.Text = $"Adding folder to Driver Pack: {folderPath}";
                });

                string relativePath = Path.Combine(folderName, Path.GetFileName(file));
                zipArchive.CreateEntryFromFile(file, relativePath);

                filesProcessed++;
                int progressPercentage = (int)((float)filesProcessed / totalFiles * 100);
                worker.ReportProgress(progressPercentage, Path.GetFileName(file));
            }
        }

        private void AddFolderContentsToZip(ZipArchive zipArchive, string folderPath, BackgroundWorker worker, ref int filesProcessed, int totalFiles)
        {
            foreach (string file in Directory.GetFiles(folderPath))
            {
                string relativePath = Path.GetFileName(file);
                zipArchive.CreateEntryFromFile(file, relativePath);

                filesProcessed++;
                int progressPercentage = (int)((float)filesProcessed / totalFiles * 100);
                worker.ReportProgress(progressPercentage, Path.GetFileName(file));
            }

            foreach (string subfolder in Directory.GetDirectories(folderPath))
            {
                AddFolderContentsToZip(zipArchive, subfolder, worker, ref filesProcessed, totalFiles);
            }
        }

        private void DismOrganizeFiles(string destinationDirectory, BackgroundWorker dismWorker)
        {
            var files = Directory.GetFiles(destinationDirectory, "*.*", SearchOption.AllDirectories)
                                 .Where(file => !file.EndsWith("DriverPack.Zip"));

            int totalFiles = files.Count();
            int filesProcessed = 0;

            foreach (var file in files)
            {
                string parentDirectory = Path.GetFileName(Path.GetDirectoryName(file));
                string newDirectoryName = $"SCCM_{parentDirectory}";
                string newDirectoryPath = Path.Combine(destinationDirectory, newDirectoryName);

                if (!Directory.Exists(newDirectoryPath))
                {
                    Directory.CreateDirectory(newDirectoryPath);
                }

                string newFilePath = Path.Combine(newDirectoryPath, Path.GetFileName(file));
                File.Copy(file, newFilePath, true);

                filesProcessed++;
                int progressPercentage = (int)((float)filesProcessed / totalFiles * 100);
                dismWorker.ReportProgress(progressPercentage, Path.GetFileName(file));

                lblCurrentFile.Invoke((MethodInvoker)delegate {
                    lblCurrentFile.Text = $"Preparing folder for driver pack: {newDirectoryName}";
                });
            }

            string zipFilePath = Path.Combine(destinationDirectory, "DriverPack.Zip");
            if (!File.Exists(zipFilePath))
            {
                using (var zipArchive = ZipFile.Open(zipFilePath, ZipArchiveMode.Create))
                {
                    var sccmFolders = Directory.GetDirectories(destinationDirectory, "SCCM_*", SearchOption.TopDirectoryOnly);
                    foreach (var sccmFolder in sccmFolders)
                    {
                        AddFolderToZip(zipArchive, sccmFolder, dismWorker, ref filesProcessed, totalFiles);
                    }
                }
            }
        }

        private void AddFolderToZip(ZipArchive zipArchive, string folderPath, BackgroundWorker worker, ref int filesProcessed, int totalFiles)
        {
            string folderName = Path.GetFileName(folderPath);
            ZipArchiveEntry folderEntry = zipArchive.CreateEntry(folderName + "/");

            foreach (string file in Directory.GetFiles(folderPath))
            {
                lblCurrentFile.Invoke((MethodInvoker)delegate {
                    lblCurrentFile.Text = $"Adding folder to Driver Pack: {folderPath}";
                });

                string relativePath = Path.Combine(folderName, Path.GetFileName(file));
                zipArchive.CreateEntryFromFile(file, relativePath);

                filesProcessed++;
                int progressPercentage = (int)((float)filesProcessed / totalFiles * 100);
                worker.ReportProgress(progressPercentage, Path.GetFileName(file));
            }

            foreach (string subfolder in Directory.GetDirectories(folderPath))
            {
                AddFolderToZip(zipArchive, subfolder, worker, ref filesProcessed, totalFiles);
            }
        }

        private void MoveDriverPackZip(string destinationDirectory)
        {
            string readyFolder = Path.Combine(destinationDirectory, "ReadyForDriverfinder");

            if (!Directory.Exists(readyFolder))
            {
                Directory.CreateDirectory(readyFolder);
            }

            string zipFilePath = Path.Combine(destinationDirectory, "DriverPack.Zip");
            if (File.Exists(zipFilePath))
            {
                string destinationFilePath = Path.Combine(readyFolder, "DriverPack.Zip");
                File.Move(zipFilePath, destinationFilePath);
            }
        }

        private void MoveFilesToReadyForDriverfinder(string destinationDirectory)
        {
            string readyFolder = Path.Combine(destinationDirectory, "ReadyForDriverfinder");

            if (!Directory.Exists(readyFolder))
            {
                Directory.CreateDirectory(readyFolder);
            }

            var filesToMove = Directory.GetFiles(destinationDirectory, "*.*", SearchOption.TopDirectoryOnly)
                                       .Where(file => file.EndsWith(".zip") || file.EndsWith(".txt"));

            foreach (var file in filesToMove)
            {
                string destinationFilePath = Path.Combine(readyFolder, Path.GetFileName(file));
                File.Move(file, destinationFilePath);
            }

            // Check if DriverPack.zip was moved successfully
            if (!File.Exists(Path.Combine(readyFolder, "DriverPack.Zip")))
            {
                MessageBox.Show("DriverPack.Zip was not moved to the ReadyForDriverfinder folder.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void DeleteSCCMFolders(string destinationDirectory)
        {
            var sccmFolders = Directory.GetDirectories(destinationDirectory, "SCCM_*", SearchOption.TopDirectoryOnly);
            foreach (var folder in sccmFolders)
            {
                Directory.Delete(folder, true);
            }
        }

        private void OpenDestinationFolder(string destinationDirectory)
        {
            Process.Start("explorer.exe", destinationDirectory);
        }

        private void Exit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
