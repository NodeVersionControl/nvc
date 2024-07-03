using Serilog;

namespace NodeVersionControl
{
    internal static class Version
    {
        public static string ListVersions()
        {
            DirectoryInfo versionsDir = new DirectoryInfo(Globals.NODE_VERSIONS_DIRECTORY);

            Log.Logger.Debug($"Searching directory {versionsDir} for all installed NodeJS versions.");

            DirectoryInfo[] versionDirs = versionsDir.GetDirectories();

            if (versionDirs.Length == 0)
            {
                Log.Logger.Information("No versions of NodeJS currently installed.");
                return null;
            }
            else
            {
                string[] versions = versionDirs.Select(dir => dir.Name).ToArray();
                string currentVersion = SharedMethods.GetCurrentNodeVersion();
                int currentSelection = 0;
                ConsoleKey key;

                do
                {
                    Console.Clear();
                    Console.WriteLine("Por favor, escolha uma versão para mudar:");

                    for (int i = 0; i < versions.Length; i++)
                    {
                        if (i == currentSelection)
                        {
                            Console.BackgroundColor = ConsoleColor.Gray;
                            Console.ForegroundColor = ConsoleColor.Black;
                        }

                        if (versions[i] == currentVersion)
                        {
                            Console.WriteLine($"[{i + 1}] {versions[i]} (Current Version)");
                        }
                        else
                        {
                            Console.WriteLine($"[{i + 1}] {versions[i]}");
                        }

                        Console.ResetColor();
                    }

                    key = Console.ReadKey(true).Key;

                    if (key == ConsoleKey.UpArrow)
                    {
                        currentSelection = (currentSelection - 1 + versions.Length) % versions.Length;
                    }
                    else if (key == ConsoleKey.DownArrow)
                    {
                        currentSelection = (currentSelection + 1) % versions.Length;
                    }
                } while (key != ConsoleKey.Enter);

                string selectedVersion = versions[currentSelection];
                Console.Clear();
                Console.WriteLine($"Você escolheu {selectedVersion}.");
                return selectedVersion;
            }
        }

        private static void LogVersions(DirectoryInfo versionsDir)
        {
            string currentVersion = SharedMethods.GetCurrentNodeVersion();

            Log.Logger.Information("Current installed versions:");
            foreach (DirectoryInfo versionDir in versionsDir.GetDirectories())
            {
                if (versionDir.Name == currentVersion)
                    Log.Logger.Information(versionDir.Name + " (Current Version)");
                else
                    Log.Logger.Information(versionDir.Name);
            }
        }
    }
}
