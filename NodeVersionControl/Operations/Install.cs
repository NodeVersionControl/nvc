using Serilog;
using System.IO.Compression;

namespace NodeVersionControl
{
    internal static class Install
    {
        public static void InstallVersion(string versionToInstall)
        {
            versionToInstall = SharedMethods.SanatizeVersionString(versionToInstall);

            string nodeVersionPath = Path.Combine(Globals.NODE_VERSIONS_DIRECTORY, versionToInstall);

            if (Directory.Exists(nodeVersionPath))
            {
                Log.Logger.Information("Trying to install a version of NodeJS that is already installed.");
                return;
            }

            CleanTempFolder();
            DownloadVersionZipToTempFolder(versionToInstall);
            ExtractZipFolder(new DirectoryInfo(Globals.TEMP_FOLDER).GetFiles()[0].FullName, nodeVersionPath);

            if (Directory.Exists(nodeVersionPath))
                Log.Logger.Information($"Successfully installed version {versionToInstall}.");
            else
                Log.Logger.Information($"Failed to install version {versionToInstall}.");
        }

        private static void CleanTempFolder()
        {
            if (!Directory.Exists(Globals.TEMP_FOLDER))
            {
                Directory.CreateDirectory(Globals.TEMP_FOLDER);
            }
            else
            {
                Log.Logger.Debug($"Clearing out Temp folder {Globals.TEMP_FOLDER}");
                SharedMethods.DeleteDirectory(Globals.TEMP_FOLDER, true);
            }
        }

        private static void DownloadVersionZipToTempFolder(string versionToInstall)
        {
            string url = $"https://nodejs.org/dist/{versionToInstall}/node-{versionToInstall}-win-{Globals.WINDOWS_ARCITECTURE}.zip";

            Log.Logger.Debug($"Downloading NodeJS version by sending GET request to {url}");

            Task<HttpResponseMessage> response = new HttpClient().GetAsync(url);

            response.Wait();

            if (response.Result.IsSuccessStatusCode)
            {
                Log.Logger.Debug($"Recieved Success Status Code from nodejs.org.");

                using (var stream = response.Result.Content.ReadAsStream())
                {
                    string zipPath = Path.Combine(Globals.TEMP_FOLDER, versionToInstall + ".zip");

                    Log.Logger.Debug($"Attempting to save zip file to {zipPath}");

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
            Log.Logger.Debug($"Attempting to extract files from {zipFilePath} to {destinationPath}");

            ZipFile.ExtractToDirectory(zipFilePath, destinationPath);

            //Since extracting the folder leaves an extra folder layer, we have to move the files and delete that folder.
            DirectoryInfo dir = new DirectoryInfo(destinationPath);
            string topLevelDirectoryName = dir.GetDirectories()[0].FullName;

            SharedMethods.CopyDirectoryContents(topLevelDirectoryName, destinationPath);
            SharedMethods.DeleteDirectory(topLevelDirectoryName);
        }
    }
}
