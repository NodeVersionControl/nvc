using System.IO.Compression;

namespace NodeVersionControl
{
    internal static class Install
    {
        public static void InstallVersion(string versionToInstall)
        {
            if (!versionToInstall.StartsWith('v'))
                versionToInstall = "v" + versionToInstall;
            
            SetupFileStructure();

            if (Directory.Exists(Path.Combine(Globals.NODE_VERSIONS_DIRECTORY, versionToInstall)))
                throw new Exception("Trying to install a version of NodeJS that is already installed.");

            string nodeVersionPath = Path.Combine(Globals.NODE_VERSIONS_DIRECTORY, versionToInstall);

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
                Directory.CreateDirectory(Globals.TEMP_FOLDER);
            else
                SharedMethods.DeleteDirectory(Globals.TEMP_FOLDER, true);
        }

        private static void DownloadVersionZipToTempFolder(string versionToInstall)
        {
            Task<HttpResponseMessage> response = new HttpClient().GetAsync($"https://nodejs.org/dist/{versionToInstall}/node-{versionToInstall}-win-{Globals.WINDOWS_ARCITECTURE}.zip");

            response.Wait();

            using (var stream = response.Result.Content.ReadAsStream())
            {
                using (Stream zip = File.OpenWrite(Path.Combine(Globals.TEMP_FOLDER, versionToInstall + ".zip")))
                {
                    stream.CopyTo(zip);
                }
            }
        }

        private static void ExtractZipFolder(string zipFilePath, string destinationPath)
        {
            ZipFile.ExtractToDirectory(zipFilePath, destinationPath);

            //Since extracting the folder leaves an extra folder layer, we have to move the files and delete that folder.
            DirectoryInfo dir = new DirectoryInfo(destinationPath);
            string topLevelDirectoryName = dir.GetDirectories()[0].FullName;

            SharedMethods.CopyDirectoryContents(topLevelDirectoryName, destinationPath);
            SharedMethods.DeleteDirectory(topLevelDirectoryName);
        }

        private static void SetupFileStructure()
        {
            if (!Directory.Exists(Globals.NODE_VERSIONS_DIRECTORY))
            {
                Directory.CreateDirectory(Globals.NODE_VERSIONS_DIRECTORY);
            }
        }
    }
}
