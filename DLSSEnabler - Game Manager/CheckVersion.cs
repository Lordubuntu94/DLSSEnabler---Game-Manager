using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace DLSSEnabler___Game_Manager
{
    internal static class CheckVersion
    {
        // Method to read the version from the text file and update the label
        public static void UpdateVersionLabel(Label dlssEnVerLabel)
        {
            try
            {
                // Get the path of the text file
                string filePath = Path.Combine(Application.StartupPath, "Readme (DLSS enabler).txt");

                // Check if the file exists
                if (File.Exists(filePath))
                {
                    // Read the first line from the text file
                    string firstLine = File.ReadLines(filePath).FirstOrDefault();

                    // Check if the firstLine is not null
                    if (firstLine != null)
                    {
                        // Extract the version from the first line
                        string[] parts = firstLine.Split(new[] { "v" }, StringSplitOptions.RemoveEmptyEntries);
                        if (parts.Length > 1)
                        {
                            string version = parts[1].Trim();
                            Version currentVersion = new Version(version);
                            Version requiredVersion = new Version("2.90.700");  // Min version accepted

                            if (currentVersion < requiredVersion)
                            {
                                MessageBox.Show("The mod version is older than 2.90.700. Download the latest version to continue.", "info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                Application.Exit(); // Esci direttamente dall'applicazione
                            }
                            else
                            {
                                // Update the label with the version
                                dlssEnVerLabel.Text = version;

                                // Save the version for future runs
                                Properties.Settings.Default.DLSSVersion = version;
                                Properties.Settings.Default.Save();
                            }
                        }
                    }
                    else
                    {
                        // If the file exists but is empty, display "N/A" in the label
                        dlssEnVerLabel.Text = "version not found - place the manager near the README file";
                    }
                }
                else
                {
                    // If the file does not exist, display "N/A" in the label
                    dlssEnVerLabel.Text = "version not found - place the manager near the README file";
                }
            }
            catch (Exception ex)
            {
                // Display an error message if an exception occurs
                MessageBox.Show("An error occurred while reading the version: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        // Method to check for a new version based on the version in the text file
        public static void CheckForNewVersion(Label dlssEnVerLabel, ListView listView)
        {
            try
            {
                Functions functions = new Functions();

                // Get the path of the text file
                string filePath = Path.Combine(Application.StartupPath, "Readme (DLSS enabler).txt");

                // Check if the file exists
                if (File.Exists(filePath))
                {
                    // Read all lines from the text file
                    string[] lines = File.ReadAllLines(filePath);

                    // Check if the array is not empty
                    if (lines.Length > 0)
                    {
                        // Get the first line from the text file
                        string firstLine = lines[0];

                        // Extract the version from the first line
                        string[] parts = firstLine.Split(new[] { "v" }, StringSplitOptions.RemoveEmptyEntries);
                        if (parts.Length > 1)
                        {
                            string currentVersion = parts[1].Trim();

                            // Get the last saved version
                            string lastSavedVersion = Properties.Settings.Default.DLSSVersion;

                            Version checkVersion = new Version(currentVersion);
                            Version requiredVersion = new Version("2.90.700");  // Min version accepted

                            if (checkVersion < requiredVersion)
                            {
                                MessageBox.Show("The mod version is older than 2.90.700. Download the latest version to continue.", "info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                Application.Exit(); // Esci direttamente dall'applicazione
                            }
                            else
                            {
                                // Compare the current version with the last saved version
                                if (!string.IsNullOrEmpty(lastSavedVersion) && currentVersion != lastSavedVersion)
                                {

                                    // If a new version is detected, update the label and notify the user

                                    DialogResult result = MessageBox.Show("A new version is installed, would you like to update the games?", "info", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                                    if (result == DialogResult.Yes)
                                    {
                                        dlssEnVerLabel.Text = currentVersion;

                                        functions.UpdateMod(listView);
                                    }
                                    else
                                    {
                                        functions.ScanAndHighlightPaths(listView, true);
                                    }

                                    // You can add code here to notify the user about the new version (e.g., using a notification icon)
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Display an error message if an exception occurs
                MessageBox.Show("An error occurred while checking for a new version: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

    }
}