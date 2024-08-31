/*
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace DLSSEnabler___Game_Manager
{
    internal class DLSS3Games
    {
        public static async Task<List<string>> GetDLSS3Games()
        {
            List<string> games = new List<string>();

            try
            {
                Properties.Settings.Default.DLSS3Games = "";
                Properties.Settings.Default.Save();
                var web = new HtmlWeb();
                int page = 1;
                bool foundGames = true;

                while (foundGames)
                {
                    string url = $"https://gg.deals/games/games-with-dlss-3-support/?page={page}";
                    var doc = await web.LoadFromWebAsync(url);

                    var nodes = doc.DocumentNode.SelectNodes("//a[@class='game-info-title title']");

                    if (nodes != null)
                    {
                        foreach (var node in nodes)
                        {
                            string gameTitle = WebUtility.HtmlDecode(node.InnerText.Trim());
                            games.Add(gameTitle);
                            // Aggiungi il gioco alla lista delle proprietà
                            // Ottieni la lista di giochi DLSS3 dal file di impostazioni
                            List<string> dlss3GamesList = new List<string>(Properties.Settings.Default.DLSS3Games.Split(','));

                            // Aggiungi il nuovo gioco alla lista
                            dlss3GamesList.Add(gameTitle);

                            // Aggiorna le proprietà con la lista aggiornata
                            Properties.Settings.Default.DLSS3Games = string.Join(",", dlss3GamesList);

                            // Salva le modifiche alle proprietà
                            Properties.Settings.Default.Save();
                        }
                        page++; // Passa alla pagina successiva
                    }
                    else
                    {
                        Console.WriteLine($"Nessun gioco trovato nella pagina {page}.");
                        foundGames = false; // Interrompe il ciclo se non sono stati trovati giochi
                    }
                }
                // Salva le modifiche alle proprietà
                Properties.Settings.Default.Save();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Errore durante l'estrazione dei giochi: {ex.Message}");
            }

            return games;
        }
    }
}
*/