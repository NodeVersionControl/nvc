using System.IO.Compression;

namespace NodeVersionControl
{
    internal static class Install
    {
        public static void InstallVersion(string versionToInstall)
        {
            if (!versionToInstall.StartsWith('v'))
                versionToInstall = "v" + versionToInstall;

            string nodeVersionPath = Path.Combine(Globals.NODE_VERSIONS_DIRECTORY, versionToInstall);

            if (Directory.Exists(nodeVersionPath))
            {
                Console.WriteLine("Trying to install a version of NodeJS that is already installed.");
                return;
            }

            CleanTempFolder();
            DownloadVersionZipToTempFolder(versionToInstall);
            ExtractZipFolder(new DirectoryInfo(Globals.TEMP_FOLDER).GetFiles()[0].FullName, nodeVersionPath);

            if (Directory.Exists(nodeVersionPath))
                Console.WriteLine($"Successfully installed version {versionToInstall}.");
            else
                Console.WriteLine($"Failed to install version {versionToInstall}.");
        }

        private static void CleanTempFolder()
        {
            if (!Directory.Exists(Globals.TEMP_FOLDER))
            {
                Directory.CreateDirectory(Globals.TEMP_FOLDER);
            }
            else
            {
                if (Globals.DEBUG)
                    Console.WriteLine($"DEBUG: Clearing out Temp folder {Globals.TEMP_FOLDER}");

                SharedMethods.DeleteDirectory(Globals.TEMP_FOLDER, true);
            }
        }

        private static void DownloadVersionZipToTempFolder(string versionToInstall)
        {
            string url = $"https://nodejs.org/dist/{versionToInstall}/node-{versionToInstall}-win-{Globals.WINDOWS_ARCITECTURE}.zip";

            if (Globals.DEBUG)
            {
                Console.WriteLine($"DEBUG: Downloading NodeJS version by sending GET request to {url}");
            }

            Task<HttpResponseMessage> response = new HttpClient().GetAsync(url);

            response.Wait();

            if (response.Result.IsSuccessStatusCode)
            {
                if (Globals.DEBUG)
                {
                    Console.WriteLine($"DEBUG: Recieved Success Status Code from nodejs.org.");
                }

                using (var stream = response.Result.Content.ReadAsStream())
                {
                    string zipPath = Path.Combine(Globals.TEMP_FOLDER, versionToInstall + ".zip");

                    if (Globals.DEBUG)
                    {
                        Console.WriteLine($"DEBUG: Attempting to save zip file to {zipPath}");
                    }

                    using (Stream zip = File.OpenWrite(zipPath))
                    {
                        stream.CopyTo(zip);
                    }
                }
            }
            else
            {
                throw new Exception($"NodeJS.org returned Status Code of: {response.Result.StatusCode}.");
            }
        }

        private static void ExtractZipFolder(string zipFilePath, string destinationPath)
        {
            if (Globals.DEBUG)
            {
                Console.WriteLine($"DEBUG: Attempting to extract files from {zipFilePath} to {destinationPath}");
            }

            ZipFile.ExtractToDirectory(zipFilePath, destinationPath);

            //Since extracting the folder leaves an extra folder layer, we have to move the files and delete that folder.
            DirectoryInfo dir = new DirectoryInfo(destinationPath);
            string topLevelDirectoryName = dir.GetDirectories()[0].FullName;

            SharedMethods.CopyDirectoryContents(topLevelDirectoryName, destinationPath);
            SharedMethods.DeleteDirectory(topLevelDirectoryName);
        }
    }
}
