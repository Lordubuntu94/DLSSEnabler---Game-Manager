using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DLSSEnabler___Game_Manager
{
    public partial class Main : Form
    {
        // Flag to check red highlight for games with mod not updated
        readonly bool checkUpdate = false;
        public Main()
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            listView1.View = View.Details;
            listView1.Columns.Add("Game", -2, HorizontalAlignment.Left);
            imageList1.ImageSize = new Size(32, 32);
            sortBox.SelectedItem = "Sort A - Z";
            dlssEnVer.Text = Properties.Settings.Default.DLSSVersion;
        }

        private async Task LoadGamesAsync()
        {
            await LoadSteamGamesAsync();
            await LoadEpicGamesLauncherGamesAsync();
            await LoadGOGGalaxyGamesAsync();
            await LoadEAGamesAsync();
            await LoadUbisoftGamesAsync();
            await LoadBattleNetGamesAsync();
        }

        private async Task LoadSteamGamesAsync()
        {
            await Functions.FindSteamGames(listView1, imageList1);
            this.Invoke((Action)(() =>
            {
                listView1.Refresh(); // Update the UI after loading Steam games
            }));
        }

        private async Task LoadEpicGamesLauncherGamesAsync()
        {
            await Functions.FindEpicGamesLauncherGames(listView1, imageList1);
            this.Invoke((Action)(() =>
            {
                listView1.Refresh(); // Update the UI after loading Epic games
            }));
        }

        private async Task LoadGOGGalaxyGamesAsync()
        {
            await Functions.FindGOGGalaxyGames(listView1, imageList1);
            this.Invoke((Action)(() =>
            {
                listView1.Refresh(); // Update the UI after loading GOG games
            }));
        }

        private async Task LoadEAGamesAsync()
        {
            await Functions.FindEAGames(listView1, imageList1);
            this.Invoke((Action)(() =>
            {
                listView1.Refresh(); // Update the UI after loading EA games
            }));
        }

        private async Task LoadUbisoftGamesAsync()
        {
            await Functions.FindUbisoftGames(listView1, imageList1);
            this.Invoke((Action)(() =>
            {
                listView1.Refresh(); // Update the UI after loading Ubisoft games
            }));
        }

        private async Task LoadBattleNetGamesAsync()
        {
            await Functions.FindBattleNetGames(listView1, imageList1);
            this.Invoke((Action)(() =>
            {
                listView1.Refresh(); // Update the UI after loading Battle.net games
            }));
        }

        // Main
        private async void Main_Load(object sender, EventArgs e)
        {
            // Call the function to find GPU and update the GPU label
            GpuInfoManager.FindGpuArchitecture(GPUlabel);

            // Show the loading GIF
            listView1.Visible = false;
            pictureBox1.Visible = true;

            // Run the game loading process on a separate thread
            await Task.Run(async () =>
            {
                await LoadGamesAsync();
            });

            // Update the UI after loading is complete
            this.Invoke((Action)(() =>
            {
                listView1.Visible = true;
                pictureBox1.Visible = false;
            }));

            // Call the function to initialize manually added games
            Functions.InitializeManuallyAddedGames(listView1, imageList1);

            // Call the function to remove sub paths
            Functions functions = new Functions();
            Functions.RemoveDuplicatePaths(listView1);
            functions.CleanSubPaths(listView1);

            // Add clickable folders
            Functions.AddClickablePaths(listView1);

            // Call function to highlight modded games
            functions.ScanAndHighlightPaths(listView1, checkUpdate);

            // Clean blacklisted paths
            HidePathsFromBlacklist();

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
                string uri = Properties.Settings.Default.GoogleSheetsURI;
                Process.Start(new ProcessStartInfo(uri) { UseShellExecute = true });
            }
        }

        // Handler for opening the Discord invite page
        private void DiscordToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (sender is ToolStripMenuItem item && item.Text == "Discord")
            {
                string uri = Properties.Settings.Default.DiscordURI;
                Process.Start(new ProcessStartInfo(uri) { UseShellExecute = true });
            }
        }

        // Handler for opening the Nexus Mods page
        private void NexusPageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (sender is ToolStripMenuItem item && item.Text == "Nexus Mods")
            {
                string uri = Properties.Settings.Default.NexusModsURI;
                Process.Start(new ProcessStartInfo(uri) { UseShellExecute = true });
            }
        }

        // Handler for opening the Buy Me a Coffee page
        private void BuyCoffee_Click(object sender, EventArgs e)
        {
            string uri = Properties.Settings.Default.BuyCoffeeURI;
            Process.Start(new ProcessStartInfo(uri) { UseShellExecute = true });
        }

        // Handler for opening Lordubuntu's GitHub page
        private void GithubToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (sender is ToolStripMenuItem item && item.Text == "Github")
            {
                string uri = Properties.Settings.Default.GithubURI;
                Process.Start(new ProcessStartInfo(uri) { UseShellExecute = true });
            }
        }


        /*
        // Edit nvngx.ini file function
        private void EditIni_Click(object sender, EventArgs e)
        {
            // Call the function to customize DLSS settings
            Functions functions = new Functions();
            functions.CustomizeDLSS(listView1);
        }
        */

        // Sort game paths
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

        // Function to restart the application so can check again for new versions
        private void CheckForUpdatesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult = MessageBox.Show("The application will be restarted. Are you sure?", "Info", MessageBoxButtons.YesNo, MessageBoxIcon.Information);

            if (DialogResult == DialogResult.Yes)
            {
                Process.Start(Application.ExecutablePath);
                Application.Exit();
            }

        }

        // Function to reset custom games added
        private void ResetManagerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult = MessageBox.Show("The application will be reset to default settings and you will lost the current configurations. Are you sure?", "Info", MessageBoxButtons.YesNo, MessageBoxIcon.Information);

            if (DialogResult == DialogResult.Yes)
            {
                Properties.Settings.Default.Reset();
                Properties.Settings.Default.Save();

                Process.Start(Application.ExecutablePath);
                Application.Exit();
            }
        }


        private void HidePathToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count > 0)
            {
                var selectedItem = listView1.SelectedItems[0];
                string pathToHide = selectedItem.SubItems[0].Text;

                // Nascondi l'elemento dalla ListView
                listView1.Items.Remove(selectedItem);

                // Aggiungi alla blacklist
                AddPathToBlacklist(pathToHide);
            }
        }

        private void AddPathToBlacklist(string path)
        {
            // Carica la lista dei path nascosti dalle impostazioni
            List<string> hiddenPaths = Properties.Settings.Default.HiddenPaths?.Split(',').ToList() ?? new List<string>();

            // Aggiungi il nuovo path alla lista solo se non è già presente
            if (!hiddenPaths.Contains(path))
            {
                hiddenPaths.Add(path);

                // Salva la lista aggiornata di path nascosti come stringa
                Properties.Settings.Default.HiddenPaths = string.Join(",", hiddenPaths);

                // Salva le impostazioni
                Properties.Settings.Default.Save();
            }
        }

        private void HidePathsFromBlacklist()
        {
            // Carica la lista dei path nascosti dalle impostazioni
            List<string> hiddenPaths = Properties.Settings.Default.HiddenPaths?.Split(',').ToList() ?? new List<string>();

            // Cicla attraverso gli elementi della ListView
            foreach (ListViewItem item in listView1.Items)
            {
                // Ottiene il path corrente dall'elemento della ListView
                string currentPath = item.SubItems[0].Text;

                // Se il path corrente è presente nella lista dei path nascosti, nascondilo
                if (hiddenPaths.Contains(currentPath))
                {
                    item.Remove(); // Rimuove l'elemento dalla ListView
                }
            }
        }

        private void button1_Click_2(object sender, EventArgs e)
        {
            Functions.FindGamesInCustomFolder(listView1, imageList1);
            Functions functions = new Functions();
            // Call the function to remove duplicate paths
            Functions.RemoveDuplicatePaths(listView1);
            // Call the function to remove sub paths
            functions.CleanSubPaths(listView1);
            // Add clickable folders
            Functions.AddClickablePaths(listView1);
            // Call function to highlight modded games
            functions.ScanAndHighlightPaths(listView1, checkUpdate);
            // Clean blacklisted paths
            HidePathsFromBlacklist();
            // Initialize the game list and search functionality
            functions.InitializeGameListAndSearch(listView1, searchBox);
        }

        /*
        private void showOnlyDLSS3GamesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string dlss3games = Properties.Settings.Default.DLSS3Games;
            string[] dlss3gamesArray = dlss3games.Split(',');

            foreach (string currentGame in dlss3gamesArray)
            {
                string gameName = currentGame.Trim();

                foreach (ListViewItem item in listView1.Items)
                {
                    string itemName = item.Text;

                    // Controlla se il nome del gioco è contenuto nel testo dell'elemento della ListView
                    if (itemName.IndexOf(gameName, StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        MessageBox.Show($"Trovato un gioco DLSS3: {gameName}", "Gioco DLSS3 Trovato", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        break; // Esci dal ciclo interno una volta trovato un gioco DLSS3
                    }
                }
            }
        }
        */




    }
}
