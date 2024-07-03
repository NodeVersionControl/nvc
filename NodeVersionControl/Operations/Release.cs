using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using NodeVersionControl.Model;
using Newtonsoft.Json.Linq;



namespace NodeVersionControl.Operations
{
    public class Release
    {

        public async static Task<string> GetVersion()
        {
            string[] listVersion = await Release.GetNodeJsVersionsAsync();

            if (listVersion == null || listVersion.Length == 0)
            {
                Console.WriteLine("Argumento inválido");
                return null;
            }

            string versionInstall = "";
            int currentSelection = 0;
            ConsoleKey key;

            do
            {
                Console.Clear();
                Console.WriteLine("Por favor, escolha uma das opções:");

                int start = Math.Max(0, currentSelection - 10); // Mostra no máximo 10 opções acima da seleção
                int end = Math.Min(listVersion.Length, currentSelection + 11); // Mostra no máximo 10 opções abaixo da seleção

                for (int i = start; i < end; i++)
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
            return versionInstall;
        }

        //public static async Task<string[]> GetNodeJsVersionsAsync()
        //{
        //    string accessToken = "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"; //para evitar erro de limit exceeded
        //    string repositoryUrl = "https://api.github.com/repos/nodejs/node/tags";

        //    using (HttpClient client = new HttpClient())
        //    {
        //        client.Timeout = TimeSpan.FromMinutes(10);
        //        client.DefaultRequestHeaders.UserAgent.ParseAdd("C# HttpClient");
        //        client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);

        //        try
        //        {
        //            HttpResponseMessage response = await client.GetAsync(repositoryUrl);
        //            response.EnsureSuccessStatusCode();

        //            string responseBody = await response.Content.ReadAsStringAsync();

        //            // Desserializa a resposta JSON para uma lista de objetos Tag
        //            var tags = JsonSerializer.Deserialize<Tag[]>(responseBody);

        //            if (tags != null && tags.Length > 0)
        //            {
        //                List<string> listVersion = new List<string>();

        //                foreach (var tag in tags)
        //                {
        //                    listVersion.Add(tag.name);
        //                }

        //                return listVersion.ToArray();
        //            }
        //        }
        //        catch (HttpRequestException ex)
        //        {
        //            Console.WriteLine($"Erro na requisição HTTP: {ex.Message}");
        //        }
        //        catch (Exception ex)
        //        {
        //            Console.WriteLine($"Ocorreu um erro: {ex.Message}");
        //        }
        //    }

        //    return null;
        //}


        public async static Task<string[]> GetNodeJsVersionsAsync()
        {
            using HttpClient client = new HttpClient();
            try
            {
                HttpResponseMessage response = await client.GetAsync("https://nodejs.org/dist/index.json");
                response.EnsureSuccessStatusCode();

                string responseBody = await response.Content.ReadAsStringAsync();
                JArray json = JArray.Parse(responseBody);
                List<string> versions = new List<string>();

                foreach (var item in json)
                {
                    versions.Add(item["version"].ToString());
                }

                return versions.ToArray();
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine($"Request error: {e.Message}");
                return Array.Empty<string>();
            }
        }

    }
}
