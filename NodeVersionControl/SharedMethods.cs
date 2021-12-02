using System.Diagnostics;

namespace NodeVersionControl
{
    internal static class SharedMethods
    {

        public static string GetCurrentNodeVersion()
        {
            string currentNodeExePath = Path.Combine(Globals.NODE_DIRECTORY, "node.exe");

            if (Globals.DEBUG)
            {
                Console.WriteLine($"DEBUG: Starting process to get the current NodeJS version from {currentNodeExePath}");
            }

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
    }
}
