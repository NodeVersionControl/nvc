using Serilog;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace NodeVersionControl
{
    internal static class SharedMethods
    {
        public static string GetCurrentNodeVersion()
        {
            string currentNodeExePath = Path.Combine(Globals.NODE_DIRECTORY, "node.exe");

            Log.Logger.Debug($"Starting process to get the current NodeJS version from {currentNodeExePath}");

            Process? nodeVersion = Process.Start(new ProcessStartInfo(currentNodeExePath, "--version")
            {
                UseShellExecute = false,
                RedirectStandardOutput = true,
                CreateNoWindow = true
            });

            if (nodeVersion != null)
                return nodeVersion.StandardOutput.ReadToEnd().Trim();
            else
                throw new Exception("Failed to get Current NodeJS version.");
        }

        public static void DeleteDirectory(string path, bool keepRootFolder = false)
        {
            if (!Directory.Exists(path))
                return;

            DirectoryInfo directory = new DirectoryInfo(path);

            if (!keepRootFolder)
            {
                directory.Delete(true);
            }
            else
            {
                foreach (FileInfo file in directory.GetFiles())
                {
                    file.Delete();
                }

                foreach (DirectoryInfo dir in directory.GetDirectories())
                {
                    dir.Delete(true);
                }
            }
        }

        public static void CopyDirectoryContents(string sourcePath, string destPath)
        {
            if (!Directory.Exists(sourcePath))
                return;

            DirectoryInfo sourceDirectory = new DirectoryInfo(sourcePath);

            foreach (FileInfo file in sourceDirectory.GetFiles())
            {
                File.Copy(file.FullName, Path.Combine(destPath, Path.GetFileName(file.Name)));
            }

            foreach (DirectoryInfo dir in sourceDirectory.GetDirectories())
            {
                if (dir.Name.Equals(Globals.NPM_GLOBALS_STORAGE_FOLDER_NAME))
                    continue;

                Directory.CreateDirectory(Path.Combine(destPath, dir.Name));
                CopyDirectoryContents(dir.FullName, Path.Combine(destPath, dir.Name));
            }
        }

        public static string MatchInstalledVersionFromPartial(string partialVersionName)
        {
            DirectoryInfo versionsDir = new DirectoryInfo(Globals.NODE_VERSIONS_DIRECTORY);
            List<DirectoryInfo> matchedDirectories = versionsDir.GetDirectories().ToList().FindAll(d => d.Name.StartsWith(partialVersionName));

            if(matchedDirectories.Count == 0)
                throw new Exception($"Unable to find version with matching substring: {partialVersionName}.");
            
            if(matchedDirectories.Count > 1)
                throw new Exception($"Found multiple versions ({string.Join(',', matchedDirectories.Select(d=> d.Name))}) with substring: {partialVersionName}.");

            return matchedDirectories.First().Name;
        }

        public static string SanatizeVersionString(string version)
        {
            version = version.ToLower();

            Regex rx = new Regex(@"[^v0-9.]");

            if (rx.Match(version).Success)
                throw new Exception("Only following characters are allowed in the version: '0-9', 'v', 'V', '.'");

            if (!version.StartsWith('v'))
                version = "v" + version;

            return version;
        }
    }
}
