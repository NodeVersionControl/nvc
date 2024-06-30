using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using NodeVersionControl.Model;



namespace NodeVersionControl.Operations
{
    public class Release
    {

        public async static Task<string> GetVersion()
        {
            //https://nodejs.org/download/release/v22.3.0/

            //string url = "https://nodejs.org/download/release/v22.3.0/node-v22.3.0-win-x64.zip";           

            string[] listVersion = await Release.GetNodeJsVersionsAsync();

            string url = "https://nodejs.org/download/release/";
            string currentDirectory = Directory.GetCurrentDirectory();
            string pathExtract = $@"{currentDirectory}\node";

            string versionInstall = "";
            int currentSelection = 0;

            if (listVersion != null && listVersion.Length > 0)
            {


                ConsoleKey key;
                do
                {
                    Console.Clear();
                    Console.WriteLine("Por favor, escolha uma das opções:");

                    for (int i = 0; i < listVersion.Length; i++)
                    {
                        if (i == currentSelection)
                        {
                            Console.BackgroundColor = ConsoleColor.Gray;
                            Console.ForegroundColor = ConsoleColor.Black;
                        }

                        Console.WriteLine($"[{i + 1}] {listVersion[i]}");

                        Console.ResetColor();
                    }

                    key = Console.ReadKey(true).Key;

                    if (key == ConsoleKey.UpArrow)
                    {
                        currentSelection = (currentSelection - 1 + listVersion.Length) % listVersion.Length;
                    }
                    else if (key == ConsoleKey.DownArrow)
                    {
                        currentSelection = (currentSelection + 1) % listVersion.Length;
                    }
                } while (key != ConsoleKey.Enter);

                Console.Clear();
                versionInstall = listVersion[currentSelection];
                //Console.WriteLine($"Você escolheu {listVersion[currentSelection]}.");
                string pathDown = $@"{currentDirectory}\node\node-{versionInstall}";

                return listVersion[currentSelection];



            }

            else
            {
                Console.WriteLine("Argumento inválido");
            }


            return listVersion[currentSelection];

        }

        public static async Task<string[]> GetNodeJsVersionsAsync()
        {
            string accessToken = "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"; //para evitar erro de limit exceeded
            string repositoryUrl = "https://api.github.com/repos/nodejs/node/tags";

            using (HttpClient client = new HttpClient())
            {
                client.Timeout = TimeSpan.FromMinutes(10);
                client.DefaultRequestHeaders.UserAgent.ParseAdd("C# HttpClient");
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);

                try
                {
                    HttpResponseMessage response = await client.GetAsync(repositoryUrl);
                    response.EnsureSuccessStatusCode();

                    string responseBody = await response.Content.ReadAsStringAsync();

                    // Desserializa a resposta JSON para uma lista de objetos Tag
                    var tags = JsonSerializer.Deserialize<Tag[]>(responseBody);

                    if (tags != null && tags.Length > 0)
                    {
                        List<string> listVersion = new List<string>();

                        foreach (var tag in tags)
                        {
                            listVersion.Add(tag.name);
                        }

                        return listVersion.ToArray();
                    }
                }
                catch (HttpRequestException ex)
                {
                    Console.WriteLine($"Erro na requisição HTTP: {ex.Message}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ocorreu um erro: {ex.Message}");
                }
            }

            return null;
        }
    }
}
