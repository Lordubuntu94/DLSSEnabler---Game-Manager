using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DLSSEnabler___Game_Manager
{
    internal class Functions
    {
        private readonly List<ListViewItem> originalItems = new List<ListViewItem>();

        // Function to find Steam games and populate the ListView with game names
        public static async Task FindSteamGames(ListView listView, ImageList imageList)
        {
            await Task.Run(() =>
            {
                try
                {
                    // Ottieni tutti i dischi logici presenti nel sistema
                    string[] drives = Directory.GetLogicalDrives();

                    // Lista di directory da controllare su ciascun disco
                    List<string> directoriesToCheck = new List<string>();

                    // Aggiungi le directory comuni di Steam per ciascun disco
                    foreach (string drive in drives)
                    {
                        string commonPath = Path.Combine(drive, "Program Files (x86)", "Steam", "steamapps", "common");
                        if (Directory.Exists(commonPath))
                        {
                            directoriesToCheck.Add(commonPath);
                        }

                        // Aggiungi le eventuali librerie aggiuntive di Steam
                        string[] additionalLibraryPaths =
                        {
                    Path.Combine(drive, "SteamLibrary", "steamapps", "common"),
                    // Aggiungi altri percorsi se necessario
                };
                        directoriesToCheck.AddRange(additionalLibraryPaths.Where(Directory.Exists));
                    }

                    // Cerca i file .exe nei percorsi specificati su tutti i dischi
                    foreach (string directoryPath in directoriesToCheck)
                    {
                        string[] gameExeFiles = Directory.GetFiles(directoryPath, "*.exe", SearchOption.AllDirectories);

                        foreach (string exeFile in gameExeFiles)
                        {
                            try
                            {
                                string gameName = Path.GetDirectoryName(exeFile);

                                // Escludi certi percorsi
                                if (IsExcludedPathSteam(gameName))
                                {
                                    continue;
                                }

                                Icon icon = Icon.ExtractAssociatedIcon(exeFile);
                                imageList.Images.Add(icon.ToBitmap());

                                ListViewItem item = new ListViewItem(new[] { gameName })
                                {
                                    ImageIndex = imageList.Images.Count - 1
                                };

                                // Aggiorna l'interfaccia utente nel thread principale
                                listView.Invoke((MethodInvoker)(() =>
                                {
                                    listView.Items.Add(item);
                                }));
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show($"Error adding game: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                // Aggiorna l'ImageList della ListView
                listView.Invoke((MethodInvoker)(() =>
                {
                    listView.SmallImageList = imageList;
                }));
            });
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
        "sdk", "easy", "direct", "editor", "battleye", "unis", "Engine", "dlc", "CRS","Redis","DX","NoDVD"
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
        public static async Task FindEpicGamesLauncherGames(ListView listView, ImageList imageList1)
        {
            await Task.Run(() =>
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

                                            // Add the item to the ListView using Invoke to update the UI safely
                                            listView.Invoke((MethodInvoker)delegate {
                                                string gameDirectory = Path.GetDirectoryName(exeFile); // Ottieni solo il percorso della directory del gioco
                                                ListViewItem item = new ListViewItem(new[] { gameDirectory }) { ImageIndex = imageList1.Images.Count - 1 };
                                                listView.Items.Add(item);
                                            });
                                        }
                                        catch (Exception ex)
                                        {
                                            MessageBox.Show($"Error adding game: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                        }
                                    }
                                }

                                // Update the ImageList of the ListView using Invoke to update the UI safely
                                listView.Invoke((MethodInvoker)delegate {
                                    listView.SmallImageList = imageList1;
                                });
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error finding Epic Games Launcher games: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            });
        }


        // Function to check if a path should be excluded for Epic Games Launcher games
        private static bool IsExcludedPathEpic(string path)
        {
            string[] excludedKeywords = new string[]
            {
        "activation", "backup", "binaries", "build", "content", "crashreportclient",
        "easyanticheat", "engine", "extras", "fortnitegame", "install", "prereqs",
        "redist", "sdk", "support", "temp", "thirdparty", "tools", "uninstall",
        "uproject", "validation", "vaultcache","Redis","DX","easy","battleye","NoDVD","unins000"
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
        public static async Task FindGOGGalaxyGames(ListView listView, ImageList imageList)
        {
            await Task.Run(() =>
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
                                                // Get the game name (directory name)
                                                string gameName = Path.GetFileName(gameDir);

                                                // Exclude certain paths
                                                if (IsExcludedPathGOG(gameName))
                                                {
                                                    continue;
                                                }

                                                // Extract the icon associated with the executable file
                                                Icon icon = Icon.ExtractAssociatedIcon(exeFile);
                                                // Add the icon to the ImageList
                                                imageList.Images.Add(icon);

                                                // Create a ListViewItem with the game name and the index of the icon in the ImageList
                                                ListViewItem item = new ListViewItem(new[] { gameName })
                                                {
                                                    ImageIndex = imageList.Images.Count - 1
                                                };

                                                // Add the item to the ListView using Invoke to update the UI safely
                                                listView.Invoke((MethodInvoker)delegate {
                                                    listView.Items.Add(item);
                                                });
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

                // Update the ImageList of the ListView using Invoke to update the UI safely
                listView.Invoke((MethodInvoker)delegate {
                    listView.SmallImageList = imageList;
                });
            });
        }


        // Function to check if a path should be excluded for GOG Galaxy games
        private static bool IsExcludedPathGOG(string path)
        {
            string[] excludedKeywords = new string[]
            {
        "activation", "benchmark", "config", "crash", "download", "install",
        "patch", "plugin", "server", "setup", "subprocess", "support", "tool",
        "unins", "update", "util", "validate", "dosbox","Redis","DX","easy","battleye","NoDVD","uninstall","unins000"
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

        public static async Task FindEAGames(ListView listView, ImageList imageList)
        {
            await Task.Run(() =>
            {
                try
                {
                    // Get the EA Desktop installation folder
                    string eaDesktopConfigPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Electronic Arts", "EA Desktop");

                    if (Directory.Exists(eaDesktopConfigPath))
                    {
                        // Search for .ini files in the EA Desktop configuration folder
                        foreach (string userConfigPath in Directory.EnumerateFiles(eaDesktopConfigPath, "user_*.ini"))
                        {
                            // Extract the game installation path from the INI file
                            var gameInstallPath = GetGameInstallPathFromIni(userConfigPath);

                            // Check if the game installation path is valid
                            if (!string.IsNullOrEmpty(gameInstallPath) && Directory.Exists(gameInstallPath))
                            {
                                // Exclude certain paths
                                if (IsExcludedPathEAGame(gameInstallPath))
                                {
                                    continue;
                                }

                                // Search for .exe files in the game installation directory
                                string[] gameExeFiles = Directory.GetFiles(gameInstallPath, "*.exe", SearchOption.AllDirectories);

                                // Iterate through each .exe file found
                                foreach (string exeFile in gameExeFiles)
                                {
                                    try
                                    {
                                        // Get the game name from the directory of the .exe file
                                        string gameName = Path.GetDirectoryName(exeFile);

                                        // Exclude certain paths
                                        if (IsExcludedPathEAGame(gameName))
                                        {
                                            continue;
                                        }

                                        // Extract the icon associated with the .exe file
                                        Icon icon = Icon.ExtractAssociatedIcon(exeFile);

                                        // Add the icon to the ImageList
                                        listView.Invoke((MethodInvoker)(() =>
                                        {
                                            imageList.Images.Add(icon);
                                        }));

                                        // Create a ListViewItem for the game and add it to the ListView
                                        ListViewItem item = new ListViewItem(new[] { gameName })
                                        {
                                            ImageIndex = imageList.Images.Count - 1
                                        };

                                        // Update the UI in the main thread
                                        listView.Invoke((MethodInvoker)(() =>
                                        {
                                            listView.Items.Add(item);
                                        }));
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
                catch (Exception ex)
                {
                    MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                // Update the ImageList of the ListView
                listView.Invoke((MethodInvoker)(() =>
                {
                    listView.SmallImageList = imageList;
                }));
            });
        }


        // Function to extract the game installation path from the INI file
        public static string GetGameInstallPathFromIni(string iniFilePath)
        {
            try
            {
                // Read all lines from the INI file
                string[] lines = File.ReadAllLines(iniFilePath);

                // Iterate through each line to find the 'user.downloadinplacedir' entry
                foreach (string line in lines)
                {
                    // Check if the line contains the 'user.downloadinplacedir' entry
                    if (line.StartsWith("user.downloadinplacedir="))
                    {
                        // Find the index of '=' and extract everything after it
                        int index = line.IndexOf('=') + 1;
                        if (index > 0 && index < line.Length)
                        {
                            string searchPath = line.Substring(index).Trim();

                            // Trim the trailing directory separator, if present
                            if (searchPath.EndsWith(Path.DirectorySeparatorChar.ToString()))
                            {
                                searchPath = searchPath.TrimEnd(Path.DirectorySeparatorChar);
                            }

                            return searchPath;
                        }
                    }
                }

                // If 'user.downloadinplacedir' entry is not found, return default path
                string defaultPath = "C:\\Program Files\\EA Games";
                return defaultPath;
            }
            catch (Exception ex)
            {
                // Handle the exception, e.g., log an error
                Console.WriteLine($"Error while reading INI file: {ex.Message}");
                return null;
            }
        }



        // Function to check if a path should be excluded for EA games
        private static bool IsExcludedPathEAGame(string path)
        {
            string[] excludedKeywords = new string[]
            {
        "activation", "benchmark", "config", "crash", "data", "dumper", "editor", "feedback",
        "framework", "installer", "logs", "patcher", "report", "sdk", "settings", "support",
        "temp", "tools", "uninstall", "update", "util", "core","Redis","DX","easy","battleye","NoDVD","unins000"
            };

            // Check if the path contains any excluded keywords
            foreach (string keyword in excludedKeywords)
            {
                if (path.IndexOf(keyword, StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    return true;
                }
            }

            return false;
        }

        public static async Task FindUbisoftGames(ListView listView, ImageList imageList)
        {
            await Task.Run(() =>
            {
                try
                {
                    // Get the Ubisoft Game Launcher installation folder
                    string ubisoftLauncherConfigPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Ubisoft Game Launcher");

                    if (Directory.Exists(ubisoftLauncherConfigPath))
                    {
                        // Get the path to the settings file
                        string settingsFilePath = Path.Combine(ubisoftLauncherConfigPath, "settings.yaml");

                        // Check if the settings file exists
                        if (File.Exists(settingsFilePath))
                        {
                            // Extract the game installation path from the settings file
                            string gameInstallPath = GetUbisoftGameInstallPath(settingsFilePath);
                            Console.WriteLine("path: " + gameInstallPath);
                            // Check if the game installation path is valid
                            if (!string.IsNullOrEmpty(gameInstallPath) && Directory.Exists(gameInstallPath))
                            {
                                // Search for .exe files in the game installation directory
                                string[] gameExeFiles = Directory.GetFiles(gameInstallPath, "*.exe", SearchOption.AllDirectories);

                                // Iterate through each .exe file found
                                foreach (string exeFile in gameExeFiles)
                                {
                                    try
                                    {
                                        // Get the game name from the directory of the .exe file
                                        string gameName = Path.GetDirectoryName(exeFile);

                                        // Exclude certain paths
                                        if (IsExcludedPathUbisoftGame(gameName))
                                        {
                                            continue;
                                        }

                                        // Extract the icon associated with the .exe file
                                        Icon icon = Icon.ExtractAssociatedIcon(exeFile);

                                        // Add the icon to the ImageList
                                        listView.Invoke((MethodInvoker)(() =>
                                        {
                                            imageList.Images.Add(icon);
                                        }));

                                        // Create a ListViewItem for the game and add it to the ListView
                                        ListViewItem item = new ListViewItem(new[] { gameName })
                                        {
                                            ImageIndex = imageList.Images.Count - 1
                                        };

                                        // Update the UI in the main thread
                                        listView.Invoke((MethodInvoker)(() =>
                                        {
                                            listView.Items.Add(item);
                                        }));
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
                catch (Exception ex)
                {
                    MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                // Update the ImageList of the ListView
                listView.Invoke((MethodInvoker)(() =>
                {
                    listView.SmallImageList = imageList;
                }));
            });
        }

        // Function to extract the game installation path from the Ubisoft Game Launcher settings file
        public static string GetUbisoftGameInstallPath(string settingsFilePath)
        {
            try
            {
                // Read all lines from the settings file
                string[] lines = File.ReadAllLines(settingsFilePath);

                // Iterate through each line to find the 'game_installation_path' entry
                foreach (string line in lines)
                {
                    // Check if the line contains the 'game_installation_path' entry
                    if (line.Trim().StartsWith("game_installation_path:"))
                    {
                        // Extract the path value from the line
                        string installPath = line.Trim().Substring("game_installation_path:".Length).Trim();

                        // Return the game installation path
                        return installPath;
                    }
                }

                // If 'game_installation_path' entry is not found, return default path
                return null;
            }
            catch (Exception ex)
            {
                // Handle the exception, e.g., log an error
                Console.WriteLine($"Error while reading Ubisoft Game Launcher settings file: {ex.Message}");
                return null;
            }
        }



        // Function to check if a path should be excluded for Ubisoft games
        private static bool IsExcludedPathUbisoftGame(string path)
        {
            string[] excludedKeywords = new string[]
            {
        "config", "crash", "logs", "savegames", "temp", "tools", "uninstall","Redis","DX", "battleye","easy","CRS","Support","DirectX","Engine","Plugins","easy","NoDVD","unins000"
            };

            // Check if the path contains any excluded keywords
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

        // Find battlenet games
        public static async Task FindBattleNetGames(ListView listView, ImageList imageList)
        {
            await Task.Run(() =>
            {
                try
                {
                    // Ottieni tutti i dischi logici presenti nel sistema
                    string[] drives = Directory.GetLogicalDrives();

                    // Lista di directory da controllare su ciascun disco
                    List<string> directoriesToCheck = new List<string>();

                    // Aggiungi le directory comuni di Battle.net per ciascun disco
                    foreach (string drive in drives)
                    {
                        string commonPath = Path.Combine(drive, "Program Files (x86)", "Battle.net");
                        if (Directory.Exists(commonPath))
                        {
                            directoriesToCheck.Add(commonPath);
                        }

                        // Aggiungi le directory comuni di Blizzard per ciascun disco
                        string blizzardPath = Path.Combine(drive, "Program Files (x86)", "Blizzard Entertainment");
                        if (Directory.Exists(blizzardPath))
                        {
                            directoriesToCheck.Add(blizzardPath);
                        }

                        // Aggiungi le eventuali librerie aggiuntive di Battle.net
                        string[] additionalLibraryPaths =
                        {
                    Path.Combine(drive, "Games"),
                    Path.Combine(drive, "BattleNetLibrary"),
                    Path.Combine(drive, "Blizzard"),
                    // Aggiungi altri percorsi se necessario
                };
                        directoriesToCheck.AddRange(additionalLibraryPaths.Where(Directory.Exists));
                    }

                    // Cerca i file .exe nei percorsi specificati su tutti i dischi
                    foreach (string directoryPath in directoriesToCheck)
                    {
                        string[] gameExeFiles = Directory.GetFiles(directoryPath, "*.exe", SearchOption.AllDirectories);

                        foreach (string exeFile in gameExeFiles)
                        {
                            try
                            {
                                string gameName = Path.GetDirectoryName(exeFile);

                                // Escludi certi percorsi
                                if (IsExcludedPathBattleNet(gameName))
                                {
                                    continue;
                                }

                                Icon icon = Icon.ExtractAssociatedIcon(exeFile);
                                imageList.Images.Add(icon.ToBitmap());

                                ListViewItem item = new ListViewItem(new[] { gameName })
                                {
                                    ImageIndex = imageList.Images.Count - 1
                                };

                                // Aggiorna l'interfaccia utente nel thread principale
                                listView.Invoke((MethodInvoker)(() =>
                                {
                                    listView.Items.Add(item);
                                }));
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show($"Error adding game: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                // Aggiorna l'ImageList della ListView
                listView.Invoke((MethodInvoker)(() =>
                {
                    listView.SmallImageList = imageList;
                }));
            });
        }

        // Function to check if a path should be excluded
        private static bool IsExcludedPathBattleNet(string path)
        {
            string[] excludedKeywords = new string[]
            {
        "agent", "blizzardbrowser", "crashhandler", "setup", "uninstall",
        "updater", "helper", "installer", "report", "battle.net", "agent", "bna", "launcher","error"
                // Aggiungi altri termini da escludere, se necessario
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

        public static void ModInstall(ListView listView1, Label Gpu)
        {
            if (listView1.SelectedItems.Count > 0)
            {
                string selectedPath = listView1.SelectedItems[0].SubItems[0].Text;
                string logFilePath = Path.Combine(selectedPath, "mod_install.log");

                try
                {
                    string dxgiPath = Path.Combine(Application.StartupPath, "dxgi.dll");
                    string nvngxIniPath = Path.Combine(Application.StartupPath, "nvngx.ini");
                    string nvngxPath = Path.Combine(Application.StartupPath, "nvngx.dll");
                    string libxessPath = Path.Combine(Application.StartupPath, "libxess.dll");

                    string destinationDxgiPath = Path.Combine(selectedPath, "dxgi.dll");
                    string destinationNvngxIniPath = Path.Combine(selectedPath, "nvngx.ini");
                    string destinationNvngxPath = Path.Combine(selectedPath, "_nvngx.dll");
                    string destinationLibxessPath = Path.Combine(selectedPath, "libxess.dll");

                    Log("Starting installation", logFilePath);
                    Log($"Selected path: {selectedPath}", logFilePath);

                    if (File.Exists(dxgiPath) && File.Exists(nvngxIniPath) && File.Exists(nvngxPath) && File.Exists(libxessPath))
                    {
                        Log("All necessary files found in mod folder", logFilePath);
                        bool overwriteFiles = false;
                        if (File.Exists(destinationDxgiPath) || File.Exists(destinationNvngxIniPath) || File.Exists(destinationNvngxPath) || File.Exists(destinationLibxessPath))
                        {
                            DialogResult result = MessageBox.Show("One or more files already exist in the destination folder. Do you want to overwrite them?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                            if (result == DialogResult.Yes)
                            {
                                overwriteFiles = true;
                                Log("User chose to overwrite existing files", logFilePath);
                            }
                            else
                            {
                                Log("User chose not to overwrite existing files", logFilePath);
                                return;
                            }
                        }

                        if (overwriteFiles || !File.Exists(destinationDxgiPath) || !File.Exists(destinationNvngxIniPath) || !File.Exists(destinationNvngxPath) || !File.Exists(destinationLibxessPath))
                        {
                            File.Copy(dxgiPath, destinationDxgiPath, true);
                            File.Copy(nvngxIniPath, destinationNvngxIniPath, true);
                            Log("Copied dxgi.dll and nvngx.ini to destination", logFilePath);

                            if (Gpu.Text.Contains("AMD") || Gpu.Text.Contains("Intel"))
                            {
                                Log("GPU is AMD or Intel", logFilePath);
                                // temporary patch
                                Log("Using _nvngx.dll for AMD/Intel GPUs", logFilePath);
                                File.Copy(nvngxPath, destinationNvngxPath, true);
                                Log("_nvngx.dll copied", logFilePath);
                                /*
                                bool nvngxDlssCopied = false;
                                string nvngxDlssFile = FindFileInParentDirectories(selectedPath, "nvngx_dlss.dll", logFilePath);
                                
                                if (!string.IsNullOrEmpty(nvngxDlssFile))
                                {
                                    FileVersionInfo fileVersionInfo = FileVersionInfo.GetVersionInfo(nvngxDlssFile);
                                    string fileVersionStr = fileVersionInfo.FileVersion.Replace(',', '.');

                                    Log($"Found nvngx_dlss.dll with version: {fileVersionStr} at path: {nvngxDlssFile}", logFilePath);

                                    if (Version.TryParse(fileVersionStr, out Version fileVersion))
                                    {
                                        Version minVersion = new Version("3.7.0.0");
                                        if (fileVersion >= minVersion)
                                        {
                                            File.Copy(nvngxDlssFile, destinationNvngxPath, true);
                                            nvngxDlssCopied = true;
                                            Log("nvngx_dlss.dll copied and renamed to _nvngx.dll", logFilePath);
                                        }
                                    }
                                }
                                
                                if (!nvngxDlssCopied)
                                {
                                    Log("Using _nvngx.dll for AMD/Intel GPUs", logFilePath);
                                    File.Copy(nvngxPath, destinationNvngxPath, true);
                                    Log("_nvngx.dll copied", logFilePath);
                                }
                                */
                            }
                            else
                            {
                                Log("GPU is not AMD or Intel, no action taken for _nvngx.dll", logFilePath);
                            }

                            if (File.Exists(destinationLibxessPath))
                            {
                                string libxessBackupPath = Path.Combine(selectedPath, "libxess.dll.org");
                                if (!File.Exists(libxessBackupPath))
                                {
                                    File.Move(destinationLibxessPath, libxessBackupPath);
                                    Log("Existing libxess.dll renamed to libxess.dll.org", logFilePath);
                                }
                            }

                            File.Copy(libxessPath, destinationLibxessPath, true);
                            Log("Copied libxess.dll to destination", logFilePath);

                            MessageBox.Show("Installation successful!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            listView1.SelectedItems[0].BackColor = Color.LightGreen;
                            Log("Installation completed successfully", logFilePath);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Installation files are missing from the mod folder.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        Log("Installation files are missing from the mod folder", logFilePath);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred during the installation: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Log("Error during installation: " + ex.Message, logFilePath);
                }
            }
            else
            {
                MessageBox.Show("Select a game from the list before installing the mod.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private static string FindFileInParentDirectories(string startPath, string fileName, string logFilePath)
        {
            string currentPath = startPath;
            int maxLevels = 4;
            int currentLevel = 0;

            while (Directory.Exists(currentPath) && currentLevel < maxLevels)
            {
                string[] files = Directory.GetFiles(currentPath, fileName, SearchOption.AllDirectories);
                if (files.Length > 0)
                {
                    Log($"Found {fileName} at {files[0]}", logFilePath);
                    return files[0]; // Return the first found instance
                }

                DirectoryInfo parentDir = Directory.GetParent(currentPath);
                if (parentDir == null)
                {
                    break; // Reached the root directory
                }

                currentPath = parentDir.FullName;
                currentLevel++;
                Log($"Checking parent directory: {currentPath}", logFilePath);
            }

            Log($"File {fileName} not found in any parent directories starting from {startPath}", logFilePath);
            return null; // File not found
        }

        private static void Log(string message, string logFilePath)
        {
            using (StreamWriter writer = new StreamWriter(logFilePath, true))
            {
                writer.WriteLine($"{DateTime.Now}: {message}");
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
                    string nvngxPath2 = Path.Combine(selectedPath, "nvngx.dll");
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

                        // Remove nvngx.dll if exists
                        if (File.Exists(nvngxPath2))
                        {
                            File.Delete(nvngxPath2);
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


        // Function to remove duplicate paths from the ListView if there are .exe files in the same directory
        public static void RemoveDuplicatePaths(ListView listView)
        {
            // Dictionary to store unique paths and their corresponding ListViewItem and icon
            Dictionary<string, List<(ListViewItem item, Image icon)>> duplicatePaths = new Dictionary<string, List<(ListViewItem item, Image icon)>>();

            // Iterate through each ListViewItem in the ListView
            foreach (ListViewItem item in listView.Items)
            {
                // Get the path from the ListViewItem
                string path = item.SubItems[0].Text;

                // Get the icon of the ListViewItem
                Image icon = item.ImageList.Images[item.ImageIndex];

                // Check if the path already exists in the dictionary
                if (duplicatePaths.ContainsKey(path))
                {
                    // If the path exists, add the item and its icon to the list of duplicates
                    duplicatePaths[path].Add((item, icon));
                }
                else
                {
                    // If the path doesn't exist, create a new list with the current item and its icon
                    List<(ListViewItem item, Image icon)> itemList = new List<(ListViewItem item, Image icon)>
            {
                (item, icon)
            };
                    duplicatePaths.Add(path, itemList);
                }
            }

            foreach (var duplicates in
            // Iterate through each set of duplicates
            from kvp in duplicatePaths
            let duplicates = kvp.Value
            select duplicates)
            {
                if (duplicates.Count == 1)
                {
                    continue; // Se c'è solo un'icona, non c'è bisogno di decidere
                }
                // Usa la nuova funzione per selezionare l'icona da mantenere
                (ListViewItem item, Image icon) iconToKeep = SelectIconToKeep(duplicates);
                // Rimuovi tutte le icone eccetto quella selezionata
                foreach (var duplicate in duplicates)
                {
                    if (duplicate != iconToKeep)
                    {
                        listView.Items.Remove(duplicate.item);
                    }
                }
            }
        }

        // Function to find the most dissimilar duplicate among a list of duplicates
        private static (ListViewItem item, Image icon) GetMostDissimilarDuplicate(List<(ListViewItem item, Image icon)> duplicates)
        {
            // If there are only two duplicates, choose the most colorful one
            if (duplicates.Count == 2)
            {
                int score1 = CalculateColorfulnessScore(duplicates[0].icon);
                int score2 = CalculateColorfulnessScore(duplicates[1].icon);

                if (score1 != score2)
                {
                    return score1 > score2 ? duplicates[0] : duplicates[1];
                }
            }

            // Select the duplicate with the maximum dissimilarity as the first choice
            (ListViewItem item, Image icon) mostDissimilarDuplicate = duplicates[0];
            double maxDissimilarityScore = 0;

            foreach (var duplicate in duplicates)
            {
                // Calculate dissimilarity score between the current duplicate and the first one
                double dissimilarityScore = CalculateDissimilarityScore(duplicate.icon, duplicates[0].icon);

                if (dissimilarityScore > maxDissimilarityScore)
                {
                    maxDissimilarityScore = dissimilarityScore;
                    mostDissimilarDuplicate = duplicate;
                }
            }

            return mostDissimilarDuplicate;
        }

        // Function to calculate the colorfulness score of an icon
        private static int CalculateColorfulnessScore(Image icon)
        {
            // Convert the icon to a bitmap
            Bitmap bitmap = new Bitmap(icon);

            // Count the number of different colors in the image
            HashSet<Color> colors = new HashSet<Color>();
            for (int x = 0; x < bitmap.Width; x++)
            {
                for (int y = 0; y < bitmap.Height; y++)
                {
                    colors.Add(bitmap.GetPixel(x, y));
                }
            }

            return colors.Count;
        }


        // Function to calculate the dissimilarity score between two icons
        private static double CalculateDissimilarityScore(Image icon1, Image icon2)
        {
            // Convert images to Bitmaps
            Bitmap bitmap1 = new Bitmap(icon1);
            Bitmap bitmap2 = new Bitmap(icon2);

            // Extract colors from images
            List<Color> colors1 = ExtractColors(bitmap1);
            List<Color> colors2 = ExtractColors(bitmap2);

            // Apply K-Means clustering to colors
            int numClusters = 5; // Choose the number of clusters
            List<Color> clusteredColors1 = KMeansClustering(colors1, numClusters);
            List<Color> clusteredColors2 = KMeansClustering(colors2, numClusters);

            // Calculate dissimilarity score based on cluster centroids
            double dissimilarityScore = CalculateClusterDissimilarity(clusteredColors1, clusteredColors2);

            return dissimilarityScore;
        }

        // Function to extract colors from an image
        private static List<Color> ExtractColors(Bitmap bitmap)
        {
            List<Color> colors = new List<Color>();

            // Iterate over each pixel and add its color to the list
            for (int x = 0; x < bitmap.Width; x++)
            {
                for (int y = 0; y < bitmap.Height; y++)
                {
                    colors.Add(bitmap.GetPixel(x, y));
                }
            }

            return colors;
        }

        // Function to perform K-Means clustering on colors
        private static List<Color> KMeansClustering(List<Color> colors, int numClusters)
        {
            // Randomly initialize cluster centroids
            List<Color> centroids = InitializeCentroids(colors, numClusters);

            // Assign each color to the nearest centroid
            Dictionary<Color, List<Color>> clusters = AssignColorsToClusters(colors, centroids);

            // Update cluster centroids iteratively until convergence
            bool centroidsUpdated = UpdateCentroids(clusters, ref centroids);
            while (centroidsUpdated)
            {
                clusters = AssignColorsToClusters(colors, centroids);
                centroidsUpdated = UpdateCentroids(clusters, ref centroids);
            }

            return centroids;
        }

        // Function to initialize cluster centroids randomly
        private static List<Color> InitializeCentroids(List<Color> colors, int numClusters)
        {
            List<Color> centroids = new List<Color>();
            Random rand = new Random();

            // Randomly select initial centroids from colors
            for (int i = 0; i < numClusters; i++)
            {
                int index = rand.Next(colors.Count);
                centroids.Add(colors[index]);
            }

            return centroids;
        }

        // Function to assign each color to the nearest centroid
        private static Dictionary<Color, List<Color>> AssignColorsToClusters(List<Color> colors, List<Color> centroids)
        {
            Dictionary<Color, List<Color>> clusters = new Dictionary<Color, List<Color>>();

            foreach (Color color in colors)
            {
                Color nearestCentroid = FindNearestCentroid(color, centroids);

                if (!clusters.ContainsKey(nearestCentroid))
                {
                    clusters[nearestCentroid] = new List<Color>();
                }
                clusters[nearestCentroid].Add(color);
            }

            return clusters;
        }

        // Function to find the nearest centroid for a given color
        private static Color FindNearestCentroid(Color color, List<Color> centroids)
        {
            double minDistance = double.MaxValue;
            Color nearestCentroid = Color.Empty;

            foreach (Color centroid in centroids)
            {
                double distance = CalculateColorDistance(color, centroid);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    nearestCentroid = centroid;
                }
            }

            return nearestCentroid;
        }

        // Function to calculate the Euclidean distance between two colors
        private static double CalculateColorDistance(Color color1, Color color2)
        {
            int rDiff = color1.R - color2.R;
            int gDiff = color1.G - color2.G;
            int bDiff = color1.B - color2.B;
            return Math.Sqrt(rDiff * rDiff + gDiff * gDiff + bDiff * bDiff);
        }

        // Function to update cluster centroids based on the mean color of each cluster
        private static bool UpdateCentroids(Dictionary<Color, List<Color>> clusters, ref List<Color> centroids)
        {
            bool centroidsUpdated = false;

            for (int i = 0; i < centroids.Count; i++)
            {
                Color oldCentroid = centroids[i];
                List<Color> clusterColors;

                // Check if the cluster has colors before updating centroid
                if (clusters.TryGetValue(oldCentroid, out clusterColors) && clusterColors.Count > 0)
                {
                    Color newCentroid = CalculateClusterMean(clusterColors);
                    if (newCentroid != oldCentroid)
                    {
                        centroids[i] = newCentroid;
                        centroidsUpdated = true;
                    }
                }
            }

            return centroidsUpdated;
        }

        // Function to calculate the mean color of a cluster
        private static Color CalculateClusterMean(List<Color> colors)
        {
            int totalR = 0, totalG = 0, totalB = 0;

            foreach (Color color in colors)
            {
                totalR += color.R;
                totalG += color.G;
                totalB += color.B;
            }

            int meanR = totalR / colors.Count;
            int meanG = totalG / colors.Count;
            int meanB = totalB / colors.Count;

            return Color.FromArgb(meanR, meanG, meanB);
        }

        // Function to calculate the dissimilarity score between two sets of clustered colors
        private static double CalculateClusterDissimilarity(List<Color> clusteredColors1, List<Color> clusteredColors2)
        {
            double dissimilarityScore = 0;

            // Calculate the dissimilarity score as the sum of distances between corresponding colors
            for (int i = 0; i < clusteredColors1.Count; i++)
            {
                dissimilarityScore += CalculateColorDistance(clusteredColors1[i], clusteredColors2[i]);
            }

            return dissimilarityScore;
        }

        // Use this function to decide which selection strategy to use
        private static (ListViewItem item, Image icon) SelectIconToKeep(List<(ListViewItem item, Image icon)> duplicates)
        {
            // First, attempt to identify and select a significantly unique icon
            var significantlyUniqueIcon = GetMostSignificantlyUniqueIcon(duplicates);
            if (significantlyUniqueIcon.item != null)
            {
                return significantlyUniqueIcon;
            }

            // If a significantly unique icon is not found, proceed with the dissimilarity logic
            return GetMostDissimilarDuplicate(duplicates);
        }

        private static (ListViewItem item, Image icon) GetMostSignificantlyUniqueIcon(List<(ListViewItem item, Image icon)> icons)
        {
            if (icons.Count <= 1) return icons.FirstOrDefault();

            var colorfulnessScores = icons.Select(icon => CalculateColorfulnessScore(icon.icon)).ToList();
            var averageScore = colorfulnessScores.Average();
            var maxScore = colorfulnessScores.Max();

            // Identify if there exists an icon significantly more colorful than the others
            if ((maxScore > averageScore * 1.5) && colorfulnessScores.Count(score => score > averageScore * 1.5) == 1)
            {
                int uniqueIndex = colorfulnessScores.IndexOf(maxScore);
                return icons[uniqueIndex];
            }

            // Continue with the dissimilarity logic if a uniquely colorful icon is not identified
            return GetMostDissimilarDuplicate(icons);
        }

        // Function to clean sub-paths
        public void CleanSubPaths(ListView listView)
        {
            int i = 0;
            while (i < listView.Items.Count - 1)
            {
                string path1 = listView.Items[i].SubItems[0].Text; // Path of the first item

                bool pathRemoved = false;

                for (int j = i + 1; j < listView.Items.Count; j++)
                {
                    string path2 = listView.Items[j].SubItems[0].Text; // Path of the second item

                    // Check if the path of the first item is contained in the path of the second item
                    if (path2.Contains(path1))
                    {
                        // Remove the first item from the ListView
                        listView.Items.RemoveAt(i);

                        // Segna che è stata rimossa una voce
                        pathRemoved = true;

                        // Esci dal ciclo for
                        break;
                    }
                }

                if (!pathRemoved)
                {
                    i++;
                }
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



        public static void FindGamesInCustomFolder(ListView listView, ImageList imageList)
        {
            try
            {
                // Mostra una finestra di dialogo per la selezione della cartella
                using (var folderDialog = new FolderBrowserDialog())
                {

                    // Initialize or retrieve the StringCollection from application settings
                    StringCollection manuallyAddedGames = new StringCollection();

                    if (!string.IsNullOrEmpty(Properties.Settings.Default.ManuallyAddedGames))
                    {
                        manuallyAddedGames.AddRange(Properties.Settings.Default.ManuallyAddedGames.Split(','));
                    }

                    folderDialog.Description = "Select the folder containing the games";
                    folderDialog.ShowNewFolderButton = false;

                    // Se l'utente conferma la selezione della cartella
                    if (folderDialog.ShowDialog() == DialogResult.OK)
                    {
                        string selectedFolder = folderDialog.SelectedPath;

                        // Cerca i file .exe nei percorsi specificati
                        string[] gameExeFiles = Directory.GetFiles(selectedFolder, "*.exe", SearchOption.AllDirectories);

                        foreach (string exeFile in gameExeFiles)
                        {
                            try
                            {
                                string gameName = Path.GetDirectoryName(exeFile);

                                // Escludi certi percorsi
                                if (IsExcludedPathSteam(gameName))
                                {
                                    continue;
                                }

                                Icon icon = Icon.ExtractAssociatedIcon(exeFile);
                                imageList.Images.Add(icon.ToBitmap());

                                ListViewItem item = new ListViewItem(new[] { gameName })
                                {
                                    ImageIndex = imageList.Images.Count - 1
                                };

                                // Aggiorna l'interfaccia utente nel thread principale
                                listView.Invoke((MethodInvoker)(() =>
                                {
                                    listView.Items.Add(item);

                                    // Add the game path to the StringCollection
                                    manuallyAddedGames.Add(exeFile);
                                }));
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show($"Error adding game: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    }
                    // Save the StringCollection back to application settings
                    Properties.Settings.Default.ManuallyAddedGames = string.Join(",", manuallyAddedGames.Cast<string>().ToArray());
                    Properties.Settings.Default.Save();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            // Aggiorna l'ImageList della ListView
            listView.Invoke((MethodInvoker)(() =>
            {
                listView.SmallImageList = imageList;
            }));
        }






    }
}