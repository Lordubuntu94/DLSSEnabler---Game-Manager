using System;
using System.Configuration;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace DLSSEnabler___Game_Manager
{
    public partial class Main : Form
    {
        // Set flag to check red highlight for games with mod not updated
        readonly bool checkUpdate = false;
        public Main()
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            // Set the ListView to display items in details view
            listView1.View = View.Details;
            // Set the width of the columns in the ListView
            listView1.Columns.Add("Game", -2, HorizontalAlignment.Left);
            // Set the size of icons in the ImageList
            imageList1.ImageSize = new Size(32, 32);
            sortBox.SelectedItem = "Sort A - Z";
            dlssEnVer.Text = Properties.Settings.Default.DLSSVersion;

        }

        // Main
        private void Main_Load(object sender, EventArgs e)
        {
            // Call the function to find GPU and update the GPU label
            GpuInfoManager.FindGpuArchitecture(GPUlabel);
            // Call the function to find Steam games and populate the ListView
            Functions.FindSteamGames(listView1, imageList1);
            // Call the function to find Epic games and populate the ListView
            Functions.FindEpicGamesLauncherGames(listView1, imageList1);
            // Call the function to find GoG games and populate the ListView
            Functions.FindGOGGalaxyGames(listView1, imageList1);
            // Call the function to initialize manually added games
            Functions.InitializeManuallyAddedGames(listView1, imageList1);
            // Call the function to remove sub paths
            Functions functions = new Functions();
            functions.CleanSubPaths(listView1);
            // Add clickable folders
            Functions.AddClickablePaths(listView1);
            // Call function to highlight modded games
            functions.ScanAndHighlightPaths(listView1, checkUpdate);
            // Initialize the game list and search functionality
            functions.InitializeGameListAndSearch(listView1, searchBox);

            // Mod versioning checks
            if ((dlssEnVer.Text == "version not found - place the manager near the README file") || (dlssEnVer.Text == ""))
            {
                CheckVersion.UpdateVersionLabel(dlssEnVer);
            }
            else
            {
                CheckVersion.CheckForNewVersion(dlssEnVer, listView1);
            }
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            // Call the function to install mod files for the selected game
            Functions.ModInstall(listView1, GPUlabel);
        }

        private void Button1_Click_1(object sender, EventArgs e)
        {
            // Call the function to uninstall mod files for the selected game
            Functions.ModUninstall(listView1);
        }

        private void Exit_Click(object sender, EventArgs e)
        {
            // Close the application
            Application.Exit();
        }

        private void Browse_Click(object sender, EventArgs e)
        {
            // Call the function to find games from other sources and add them to the ListView
            Functions.FindOtherGames(listView1, imageList1);
            // Re-initialize the game list and search functionality after adding new games
            Functions functions = new Functions();
            functions.InitializeGameListAndSearch(listView1, searchBox);
            // Call function to highlight modded games
            functions.ScanAndHighlightPaths(listView1, checkUpdate);
        }

        // Handler for opening the Google Sheets document
        private void GoogleDocToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (sender is ToolStripMenuItem item && item.Text == "Compatibility doc")
            {
                string uri = ConfigurationManager.AppSettings["GoogleSheetsURI"];
                Process.Start(new ProcessStartInfo(uri) { UseShellExecute = true });
            }
        }

        // Handler for opening the Discord invite page
        private void DiscordToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (sender is ToolStripMenuItem item && item.Text == "Discord")
            {
                string uri = ConfigurationManager.AppSettings["DiscordURI"];
                Process.Start(new ProcessStartInfo(uri) { UseShellExecute = true });
            }
        }

        // Handler for opening the Nexus Mods page
        private void NexusPageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (sender is ToolStripMenuItem item && item.Text == "Nexus Mods")
            {
                string uri = ConfigurationManager.AppSettings["NexusModsURI"];
                Process.Start(new ProcessStartInfo(uri) { UseShellExecute = true });
            }
        }

        // Handler for opening the Buy Me a Coffee page
        private void BuyCoffee_Click(object sender, EventArgs e)
        {
            string uri = ConfigurationManager.AppSettings["BuyCoffeeURI"];
            Process.Start(new ProcessStartInfo(uri) { UseShellExecute = true });
        }

        // Handler for opening Lordubuntu's GitHub page
        private void GithubToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (sender is ToolStripMenuItem item && item.Text == "GitHub")
            {
                string uri = ConfigurationManager.AppSettings["GithubURI"];
                Process.Start(new ProcessStartInfo(uri) { UseShellExecute = true });
            }
        }



        private void EditIni_Click(object sender, EventArgs e)
        {
            // Call the function to customize DLSS settings
            Functions functions = new Functions();
            functions.CustomizeDLSS(listView1);
        }

        private void SortBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (sortBox.Text.Equals("Sort A - Z"))
            {
                listView1.Sorting = SortOrder.Ascending;
                listView1.Sort();
            }
            else if (sortBox.Text.Equals("Sort Z - A"))
            {
                listView1.Sorting = SortOrder.Descending;
                listView1.Sort();
            }
        }

        private void DLSSEnablerLogToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Check if a game is selected in the ListView
            if (listView1.SelectedItems.Count > 0)
            {
                // Get the selected path in the ListView
                string selectedPath = listView1.SelectedItems[0].SubItems[0].Text;
                try
                {
                    // Path of the log
                    string dlssEnlogPath = Path.Combine(selectedPath, "dlss-enabler.log");

                    // Check if the files exist
                    if (File.Exists(dlssEnlogPath))
                    {
                        // Open the log file
                        Process.Start(dlssEnlogPath);
                    }
                    else
                    {
                        MessageBox.Show("Log file is missing for this game.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred during the log searching: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Select a game from the list before opening it's logfile.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void CheckForUpdatesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult = MessageBox.Show("The application will be restarted. Are you sure?", "Info", MessageBoxButtons.YesNo, MessageBoxIcon.Information);

            if (DialogResult == DialogResult.Yes)
            {
                Application.Restart();
            }

        }
    }
}
