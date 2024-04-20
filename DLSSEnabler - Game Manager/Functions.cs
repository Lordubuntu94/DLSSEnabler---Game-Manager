using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace DLSSEnabler___Game_Manager
{
    internal class Functions
    {
        private readonly List<ListViewItem> originalItems = new List<ListViewItem>();

        // Function to find Steam games and populate the ListView with game names
        public static void FindSteamGames(ListView listView1, ImageList imageList1)
        {

            try
            {
                // Get the Steam install path from the registry
                string steamInstallPath = Registry.GetValue(@"HKEY_LOCAL_MACHINE\Software\Wow6432Node\Valve\Steam", "InstallPath", null) as string;

                // Check if the Steam directory exists
                if (!string.IsNullOrEmpty(steamInstallPath) && Directory.Exists(steamInstallPath))
                {
                    // Get the common directory within the Steam directory
                    string commonPath = Path.Combine(steamInstallPath, "steamapps", "common");

                    // Check if the common directory exists
                    if (Directory.Exists(commonPath))
                    {
                        // Search for game executables within the common directory
                        string[] gameExeFiles = Directory.GetFiles(commonPath, "*.exe", SearchOption.AllDirectories);

                        // Iterate over each game executable found
                        foreach (string exeFile in gameExeFiles)
                        {
                            try
                            {
                                // Get the game path 
                                string gameName = Path.GetDirectoryName(exeFile);

                                // Exclude certain paths
                                if (IsExcludedPathSteam(gameName))
                                {
                                    continue;
                                }

                                // Extract the icon associated with the executable file
                                Icon ico = Icon.ExtractAssociatedIcon(exeFile);
                                // Add the icon to the ImageList
                                imageList1.Images.Add(ico.ToBitmap());

                                // Create a ListViewItem with the game name and the index of the icon in the ImageList
                                ListViewItem item = new ListViewItem(new[] { gameName })
                                {
                                    ImageIndex = imageList1.Images.Count - 1
                                };

                                // Add the item to the ListView
                                listView1.Items.Add(item);
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show($"Error adding game: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Steam folder not found!", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            // Update the ImageList of the ListView
            listView1.SmallImageList = imageList1;
        }


        // Function to check if a path should be excluded
        private static bool IsExcludedPathSteam(string path)
        {
            string[] excludedKeywords = new string[]
            {
        "activation", "apputil", "benchmark", "cefprocess", "compile", "compress",
        "config", "console", "convert", "crash", "diagnostics", "download", "helper",
        "inject", "install", "launch", "openvr", "overlay", "patch", "plugin", "redis",
        "register", "report", "server", "setup", "steamvr", "subprocess", "support",
        "tool", "unins", "update", "util", "validate", "wallpaper", "webengine", "webview",
        "7za", "createdump", "fossilize", "Rpt", "svc", "SystemSoftware", "TagesClient", "jre",
        "sdk", "easy", "direct", "editor", "battleye", "unis", "Engine", "dlc"
            };

            foreach (string keyword in excludedKeywords)
            {
                if (path.IndexOf(keyword, StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    return true;
                }
            }

            return false;
        }

        // Function to find Epic Games Launcher games and populate the ListView with game names and icons
        public static void FindEpicGamesLauncherGames(ListView listView, ImageList imageList1)
        {
            try
            {
                // Path to the Epic Games Launcher configuration file
                string epicGamesConfigPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "EpicGamesLauncher", "Saved", "Config", "Windows", "GameUserSettings.ini");

                // Check if the configuration file exists
                if (File.Exists(epicGamesConfigPath))
                {
                    // Read all lines from the configuration file
                    string[] lines = File.ReadAllLines(epicGamesConfigPath);

                    // Use Array.Find to locate the first line that contains DefaultAppInstallLocation
                    string line = Array.Find(lines, l => l.Contains("DefaultAppInstallLocation="));

                    if (line != null)
                    {
                        // Extract the install location from the line
                        string searchPath = line.Split('=')[1].Trim();

                        // Check if the install location is valid
                        if (!string.IsNullOrEmpty(searchPath) && Directory.Exists(searchPath))
                        {
                            var gameDirs = Directory.GetDirectories(searchPath);
                            imageList1.ImageSize = new Size(32, 32); // Set the size of the icons

                            foreach (var dir in gameDirs)
                            {
                                var exeFiles = Directory.GetFiles(dir, "*.exe", SearchOption.TopDirectoryOnly);
                                foreach (var exeFile in exeFiles)
                                {
                                    try
                                    {
                                        // Exclude certain paths
                                        if (IsExcludedPathEpic(dir))
                                        {
                                            continue;
                                        }

                                        Icon icon = Icon.ExtractAssociatedIcon(exeFile);
                                        imageList1.Images.Add(icon.ToBitmap());

                                        ListViewItem item = new ListViewItem(new[] { exeFile }) { ImageIndex = imageList1.Images.Count - 1 };
                                        listView.Items.Add(item);
                                    }
                                    catch (Exception ex)
                                    {
                                        MessageBox.Show($"Error adding game: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    }
                                }
                            }

                            listView.SmallImageList = imageList1;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error finding Epic Games Launcher games: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }



        // Function to check if a path should be excluded for Epic Games Launcher games
        private static bool IsExcludedPathEpic(string path)
        {
            string[] excludedKeywords = new string[]
            {
        "activation", "backup", "binaries", "build", "content", "crashreportclient",
        "easyanticheat", "engine", "extras", "fortnitegame", "install", "prereqs",
        "redist", "sdk", "support", "temp", "thirdparty", "tools", "uninstall",
        "uproject", "validation", "vaultcache"
            };

            foreach (string keyword in excludedKeywords)
            {
                if (path.IndexOf(keyword, StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    return true;
                }
            }

            return false;
        }

        // Function to find GOG Galaxy games and populate the ListView with game names and icons
        public static void FindGOGGalaxyGames(ListView listView1, ImageList imageList1)
        {
            try
            {
                // Open the registry key containing GOG Galaxy games
                using (var gogGamesKey = Registry.LocalMachine.OpenSubKey(@"Software\Wow6432Node\GOG.com\Games"))
                {
                    // Check if the registry key exists
                    if (gogGamesKey != null)
                    {
                        // Iterate over each subkey (each game) in the registry key
                        foreach (var gogGame in gogGamesKey.GetSubKeyNames())
                        {
                            // Get the game directory path from the registry value
                            string gameDir = gogGamesKey.OpenSubKey(gogGame)?.GetValue("path") as string;

                            // Check if the game directory path is valid
                            if (!string.IsNullOrEmpty(gameDir) && Directory.Exists(gameDir))
                            {
                                // Search for all executable files within the game directory and its subdirectories
                                string[] gameExeFiles = Directory.GetFiles(gameDir, "*.exe", SearchOption.AllDirectories);

                                // Check if there are any executable files
                                if (gameExeFiles.Length > 0)
                                {
                                    foreach (string exeFile in gameExeFiles)
                                    {
                                        try
                                        {
                                            // Get the game name 
                                            string gameName = Path.GetDirectoryName(exeFile);

                                            // Exclude certain paths
                                            if (IsExcludedPathGOG(gameName))
                                            {
                                                continue;
                                            }

                                            // Extract the icon associated with the executable file
                                            Icon ico = Icon.ExtractAssociatedIcon(exeFile);
                                            // Add the icon to the ImageList
                                            imageList1.Images.Add(ico);

                                            // Create a ListViewItem with the game name and the index of the icon in the ImageList
                                            ListViewItem item = new ListViewItem(new[] { gameName })
                                            {
                                                ImageIndex = imageList1.Images.Count - 1
                                            };

                                            // Add the item to the ListView
                                            listView1.Items.Add(item);
                                        }
                                        catch (Exception ex)
                                        {
                                            MessageBox.Show($"Error adding game: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error finding GOG Galaxy games: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            // Update the ImageList of the ListView
            listView1.SmallImageList = imageList1;
        }

        // Function to check if a path should be excluded for GOG Galaxy games
        private static bool IsExcludedPathGOG(string path)
        {
            string[] excludedKeywords = new string[]
            {
        "activation", "benchmark", "config", "crash", "download", "install",
        "patch", "plugin", "server", "setup", "subprocess", "support", "tool",
        "unins", "update", "util", "validate", "dosbox"
            };

            foreach (string keyword in excludedKeywords)
            {
                if (path.IndexOf(keyword, StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    return true;
                }
            }

            return false;
        }


        // Function to find games from other sources and populate the ListView with game names
        public static void FindOtherGames(ListView listView1, ImageList imageList1)
        {
            // Open a file dialog to allow the user to select games to add
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Multiselect = true,
                Filter = "Executable Files (*.exe)|*.exe"
            };

            // Show the file dialog and check if the user has selected files
            DialogResult result = openFileDialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                // Initialize or retrieve the StringCollection from application settings
                StringCollection manuallyAddedGames = new StringCollection();

                if (!string.IsNullOrEmpty(Properties.Settings.Default.ManuallyAddedGames))
                {
                    manuallyAddedGames.AddRange(Properties.Settings.Default.ManuallyAddedGames.Split(','));
                }

                // Loop through all selected files by the user
                foreach (string exeFile in openFileDialog.FileNames)
                {
                    try
                    {
                        // Extract the icon associated with the executable file
                        Icon ico = Icon.ExtractAssociatedIcon(exeFile);
                        // Add the icon to the ImageList
                        imageList1.Images.Add(ico.ToBitmap());

                        // Get the game path without the executable name
                        string gamePath = Path.GetDirectoryName(exeFile);

                        // Check if the game path is not already in the StringCollection
                        if (!manuallyAddedGames.Contains(exeFile))
                        {
                            // Create a ListViewItem with the game path (without the executable name) and the index of the icon in the ImageList
                            ListViewItem item = new ListViewItem(new[] { gamePath })
                            {
                                ImageIndex = imageList1.Images.Count - 1,
                                // Store the full path with the executable name as a tag
                                Tag = exeFile
                            };

                            // Add the item to the ListView
                            listView1.Items.Add(item);

                            // Add the game path to the StringCollection
                            manuallyAddedGames.Add(exeFile);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error adding game: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }

                // Save the StringCollection back to application settings
                Properties.Settings.Default.ManuallyAddedGames = string.Join(",", manuallyAddedGames.Cast<string>().ToArray());
                Properties.Settings.Default.Save();
            }
        }



        public static void InitializeManuallyAddedGames(ListView listView1, ImageList imageList1)
        {
            // Get the string containing manually added games from application settings
            string manuallyAddedGamesString = Properties.Settings.Default.ManuallyAddedGames;

            // Check if the string is not empty or null
            if (!string.IsNullOrEmpty(manuallyAddedGamesString))
            {
                // Split the string to get individual game paths
                string[] gamePaths = manuallyAddedGamesString.Split(',');

                // Create a list to store the valid game paths
                List<string> validGamePaths = new List<string>();

                // Loop through each game path and check if it exists on the PC
                foreach (string gamePathWithExe in gamePaths)
                {
                    try
                    {
                        // Get the game path without the executable name
                        string gamePath = Path.GetDirectoryName(gamePathWithExe);

                        // Check if the directory exists
                        if (Directory.Exists(gamePath))
                        {
                            // Extract the icon associated with the game path including the executable name
                            Icon icon = Icon.ExtractAssociatedIcon(gamePathWithExe);
                            imageList1.Images.Add(icon.ToBitmap());

                            // Create a ListViewItem with the game path without the executable name and the index of the icon in the ImageList
                            ListViewItem item = new ListViewItem(new[] { gamePath })
                            {
                                ImageIndex = imageList1.Images.Count - 1
                            };

                            // Add the item to the ListView
                            listView1.Items.Add(item);

                            // Add the valid game path to the list
                            validGamePaths.Add(gamePathWithExe);
                        }
                        else
                        {
                            // Game directory doesn't exist, remove it from properties
                            RemoveGameFromProperties(gamePathWithExe);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error adding game: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }

                // Save the valid game paths back to application settings
                Properties.Settings.Default.ManuallyAddedGames = string.Join(",", validGamePaths);
                Properties.Settings.Default.Save();
            }
        }

        private static void RemoveGameFromProperties(string gamePathWithExe)
        {
            string manuallyAddedGamesString = Properties.Settings.Default.ManuallyAddedGames;
            if (!string.IsNullOrEmpty(manuallyAddedGamesString))
            {
                string[] gamePaths = manuallyAddedGamesString.Split(',');
                List<string> updatedGamePaths = new List<string>(gamePaths);
                updatedGamePaths.Remove(gamePathWithExe);
                Properties.Settings.Default.ManuallyAddedGames = string.Join(",", updatedGamePaths);
                Properties.Settings.Default.Save();
            }
        }


        // Function to install selected mod files to the selected game folder
        public static void ModInstall(ListView listView1, Label Gpu)
        {
            // Check if a game is selected in the ListView
            if (listView1.SelectedItems.Count > 0)
            {
                // Get the selected path in the ListView
                string selectedPath = listView1.SelectedItems[0].SubItems[0].Text;

                try
                {
                    // Paths of files to move
                    string dxgiPath = Path.Combine(Application.StartupPath, "dxgi.dll");
                    string nvngxIniPath = Path.Combine(Application.StartupPath, "nvngx.ini");
                    string nvngxPath = Path.Combine(Application.StartupPath, "_nvngx.dll");
                    string libxessPath = Path.Combine(Application.StartupPath, "libxess.dll");

                    // Destination paths
                    string destinationDxgiPath = Path.Combine(selectedPath, "dxgi.dll");
                    string destinationNvngxIniPath = Path.Combine(selectedPath, "nvngx.ini");
                    string destinationNvngxPath = Path.Combine(selectedPath, "_nvngx.dll");
                    string destinationLibxessPath = Path.Combine(selectedPath, "libxess.dll");

                    // Check if the files exist
                    if (File.Exists(dxgiPath) && File.Exists(nvngxIniPath) && File.Exists(nvngxPath) && File.Exists(libxessPath))
                    {
                        // Move the files to the selected folder
                        File.Copy(dxgiPath, destinationDxgiPath, true);
                        File.Copy(nvngxIniPath, destinationNvngxIniPath, true);
                        // Based on the find gpu architecture function, if amd or intel card is detected _nvgnx.dll will be installed too
                        if (Gpu.Text.Contains("AMD") || Gpu.Text.Contains("Intel"))
                        {
                            File.Copy(nvngxPath, destinationNvngxPath, true);
                        }

                        // Check if libxess.dll already exists in the destination folder
                        if (File.Exists(destinationLibxessPath))
                        {
                            // Rename the existing libxess.dll to libxess.dll.org
                            File.Move(destinationLibxessPath, Path.Combine(selectedPath, "libxess.dll.org"));
                        }

                        // Move libxess.dll to the destination folder
                        File.Copy(libxessPath, destinationLibxessPath, true);

                        MessageBox.Show("Installation successful!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        listView1.SelectedItems[0].BackColor = Color.LightGreen;
                    }
                    else
                    {
                        MessageBox.Show("Installation files are missing from the mod folder.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred during the installation: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Select a game from the list before installing the mod.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }


        public static void ModUninstall(ListView listView1)
        {
            // Check if a game is selected in the ListView
            if (listView1.SelectedItems.Count > 0)
            {
                // Get the selected path in the ListView
                string selectedPath = listView1.SelectedItems[0].SubItems[0].Text;

                try
                {
                    // Paths of files to remove
                    string dxgiPath = Path.Combine(selectedPath, "dxgi.dll");
                    string nvngxIniPath = Path.Combine(selectedPath, "nvngx.ini");
                    string nvngxPath = Path.Combine(selectedPath, "_nvngx.dll");
                    string libxessPath = Path.Combine(selectedPath, "libxess.dll");
                    string libxessOrgPath = Path.Combine(selectedPath, "libxess.dll.org");

                    if (listView1.SelectedItems[0].SubItems[0].BackColor == Color.LightGreen)
                    {
                        // Remove dxgi.dll if exists
                        if (File.Exists(dxgiPath))
                        {
                            File.Delete(dxgiPath);
                        }

                        // Remove nvngx.ini if exists
                        if (File.Exists(nvngxIniPath))
                        {
                            File.Delete(nvngxIniPath);
                        }

                        // Remove _nvngx.dll if exists
                        if (File.Exists(nvngxPath))
                        {
                            File.Delete(nvngxPath);
                        }

                        // Remove libxess.dll and restore the original if the game had it before the mod
                        if (File.Exists(libxessOrgPath) && File.Exists(libxessPath))
                        {
                            File.Delete(libxessPath);
                            File.Move(libxessOrgPath, libxessPath);

                        }
                        else
                        {
                            File.Delete(libxessPath);
                        }

                        MessageBox.Show("Uninstallation successful!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        listView1.SelectedItems[0].BackColor = Color.Empty;
                    }
                    else
                    {
                        MessageBox.Show("Can't disable a game that is not modded!", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }

                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred during the uninstallation: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Select a game from the list before uninstalling the mod.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }



        // Function to filter and display games based on search text
        public void ResearchGame(ListView listView1, TextBox searchBox)
        {
            string searchText = searchBox.Text.ToLower();

            listView1.Items.Clear(); // Clear the current items for the new search

            IEnumerable<ListViewItem> filteredItems;

            if (string.IsNullOrWhiteSpace(searchText))
            {
                // If the search box is empty, restore the original view
                filteredItems = originalItems;
            }
            else
            {
                // Filter and display only the items that match the search
                filteredItems = originalItems.Where(item => item.Text.ToLower().Contains(searchText));
            }

            foreach (ListViewItem item in filteredItems)
            {
                listView1.Items.Add((ListViewItem)item.Clone());
            }
        }


        // Function to initialize the game list and search box
        public void InitializeGameListAndSearch(ListView listView1, TextBox searchBox)
        {
            // Suppose you've already populated listView1 with games from the Steam path,
            // now store the items in originalItems
            originalItems.Clear(); // Make sure originalItems is empty before filling it
            foreach (ListViewItem item in listView1.Items)
            {
                originalItems.Add((ListViewItem)item.Clone());
            }

            // Use a lambda expression as a wrapper
            searchBox.TextChanged += (sender, e) => ResearchGame(listView1, searchBox);
        }

        // Function to customize DLSS settings for the selected game
        public void CustomizeDLSS(ListView listView1)
        {
            // Check if a game is selected in the ListView
            if (listView1.SelectedItems.Count > 0)
            {
                // Get the selected game's path
                string selectedGamePath = listView1.SelectedItems[0].SubItems[0].Text;

                // Check if the nvgnx.ini file exists for the selected game
                string nvgnxIniPath = Path.Combine(selectedGamePath, "nvngx.ini");
                if (File.Exists(nvgnxIniPath))
                {
                    // Get the selected ListViewItem
                    ListViewItem selectedItem = listView1.SelectedItems[0];

                    // Create an instance of the Editor form
                    Editor editorForm = new Editor
                    {
                        // Set the Editor form's title
                        Text = "Customize DLSS - " + selectedItem.Text,

                        // Set the Editor form's icon using the ListViewItem's icon
                        Icon = Icon.FromHandle(((Bitmap)selectedItem.ImageList.Images[selectedItem.ImageIndex]).GetHicon())
                    };

                    // Open the Editor form
                    editorForm.ShowDialog();
                }
                else
                {
                    MessageBox.Show("The nvgnx.ini file is not present for the selected game.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                MessageBox.Show("Select a game from the list before opening the editor.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        // Function to clean sub-paths
        public void CleanSubPaths(ListView listView)
        {
            List<int> indicesToRemove = new List<int>();

            // Doppio ciclo per confrontare gli elementi
            for (int i = 0; i < listView.Items.Count - 1; i++)
            {
                string path1 = listView.Items[i].SubItems[0].Text; // Path of the first item

                for (int j = i + 1; j < listView.Items.Count; j++)
                {
                    string path2 = listView.Items[j].SubItems[0].Text; // Path of the second item

                    // Check if the path of the first item is contained in the path of the second item
                    if (path2.Contains(path1))
                    {
                        indicesToRemove.Add(i);
                        break; // Break the inner loop once a removal is determined
                    }
                }
            }

            // Removing items from the listview by index, from largest to smallest to avoid shifting issues
            foreach (int index in indicesToRemove.OrderByDescending(x => x))
            {
                listView.Items.RemoveAt(index);
            }
        }




        // Scan modded games and highlights them in green or red based on new/older mod version
        public void ScanAndHighlightPaths(ListView listView, bool check)
        {
            // Scans each item in the ListView
            foreach (ListViewItem item in listView.Items)
            {
                string path = item.SubItems[0].Text; // Retrieves the path from the ListView item

                // Checks if the required files are present in the path
                bool hasDxgiDll = File.Exists(Path.Combine(path, "dxgi.dll"));
                bool hasNvngxIni = File.Exists(Path.Combine(path, "nvngx.ini"));
                bool hasNvngxDll = File.Exists(Path.Combine(path, "_nvngx.dll"));

                // If all three files are present, sets the item's background to green
                // Or if dxgi.dll and nvngx.ini are present, based on the 'check' flag,
                // it highlights the item accordingly
                if ((hasDxgiDll && hasNvngxIni && hasNvngxDll) || (hasDxgiDll && hasNvngxIni))
                {
                    if (check)
                    {
                        item.BackColor = Color.LightSalmon; // Highlight in LightSalmon if check is true
                    }
                    else
                    {
                        item.BackColor = Color.LightGreen; // Otherwise, highlight in LightGreen
                    }
                }
            }
        }


        // Massive update function for previously modded games
        public void UpdateMod(ListView listView)
        {
            // Scans each item in the ListView
            foreach (ListViewItem item in listView.Items)
            {
                string path = item.SubItems[0].Text; // Retrieves the path from the ListView item

                // Checks if the required files are present in the path
                bool hasDxgiDll = File.Exists(Path.Combine(path, "dxgi.dll"));
                bool hasNvngxIni = File.Exists(Path.Combine(path, "nvngx.ini"));
                bool hasNvngxDll = File.Exists(Path.Combine(path, "_nvngx.dll"));
                bool hasLibxessDll = File.Exists(Path.Combine(path, "libxess.dll"));

                // Paths of the files to move
                string dxgiPath = Path.Combine(Application.StartupPath, "dxgi.dll");
                string nvngxIniPath = Path.Combine(Application.StartupPath, "nvngx.ini");
                string nvngxPath = Path.Combine(Application.StartupPath, "_nvngx.dll");
                string libxessPath = Path.Combine(Application.StartupPath, "libxess.dll");

                // Destination paths
                string destinationDxgiPath = Path.Combine(path, "dxgi.dll");
                string destinationNvngxIniPath = Path.Combine(path, "nvngx.ini");
                string destinationNvngxPath = Path.Combine(path, "_nvngx.dll");
                string destinationLibxessPath = Path.Combine(path, "libxess.dll");

                // If all four files are present, copies the mod files to the target directory
                if (hasDxgiDll && hasNvngxIni && hasNvngxDll && hasLibxessDll)
                {
                    File.Copy(dxgiPath, destinationDxgiPath, true);
                    File.Copy(nvngxIniPath, destinationNvngxIniPath, true);
                    File.Copy(nvngxPath, destinationNvngxPath, true);
                    File.Copy(libxessPath, destinationLibxessPath, true);
                }
                // If only dxgi.dll and nvngx.ini are present, copies these two mod files
                else if (hasDxgiDll && hasNvngxIni && hasLibxessDll)
                {
                    File.Copy(dxgiPath, destinationDxgiPath, true);
                    File.Copy(nvngxIniPath, destinationNvngxIniPath, true);
                    File.Copy(libxessPath, destinationLibxessPath, true);
                }
            }
            // Notifies the user that the update was successful
            MessageBox.Show("Update successful!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }


        // Function to enable double-clicking on a game path to open the full directory
        public static void AddClickablePaths(ListView listView)
        {
            // Add an event handler for double-clicking on an item in the ListView
            listView.MouseDoubleClick += (sender, e) =>
            {
                // Get the item that was clicked
                ListViewItem item = listView.GetItemAt(e.X, e.Y);

                // Check if an item was actually clicked
                if (item != null)
                {
                    // Get the path from the first column of the item
                    string path = item.SubItems[0].Text;

                    // Open the folder corresponding to the path
                    if (!string.IsNullOrEmpty(path) && Directory.Exists(path))
                    {
                        Process.Start(path);
                    }
                    else
                    {
                        MessageBox.Show("The folder path is not valid.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            };
        }
    }
}