namespace NodeVersionControl
{
    internal class Change
    {
        public static void ChangeVersion(string newVersion)
        {
            string oldVersion = SharedMethods.GetCurrentNodeVersion();

            if (!newVersion.StartsWith('v'))
                newVersion = "v" + newVersion;

            if (oldVersion == newVersion)
                throw new Exception($"Already using version {newVersion} of NodeJS.");

            string newVersionPath = Path.Combine(Globals.NODE_VERSIONS_DIRECTORY, newVersion);

            if (!Directory.Exists(newVersionPath))
                throw new Exception($"NodeJS version {newVersion} not found.");

            StoreGlobalNodeModules(oldVersion);

            ChangeNodeVersions(newVersionPath);

            RestoreGlobalNodeModules(newVersionPath);

            if (newVersion == SharedMethods.GetCurrentNodeVersion())
                Console.WriteLine($"Successfully switched NodeJS version from {oldVersion} to {newVersion}.");
            else
                Console.WriteLine($"Failed to switch NodeJS version to {newVersion}.");
        }

        private static void ChangeNodeVersions(string versionDirectory)
        {
            //Delete current NodeJS version
            if (Directory.Exists(Globals.NODE_DIRECTORY))
                SharedMethods.DeleteDirectory(Globals.NODE_DIRECTORY, keepRootFolder: true);
            else
                Directory.CreateDirectory(Globals.NODE_DIRECTORY);

            //Copy over requested NodeJS version
            SharedMethods.CopyDirectoryContents(versionDirectory, Globals.NODE_DIRECTORY);
        }

        private static void RestoreGlobalNodeModules(string versionDirectory)
        {
            //Copy over global NPM packages
            SharedMethods.CopyDirectoryContents(Path.Combine(versionDirectory, Globals.NPM_GLOBALS_STORAGE_FOLDER_NAME), Globals.NPM_GLOBALS_DIRECTORY);
        }

        private static void StoreGlobalNodeModules(string currentVersion)
        {
            //Delete the node modules stored in the versions directory
            if (Directory.Exists(Globals.NPM_GLOBALS_DIRECTORY))
            {
                string versionedNpmGlobalsDirectory = Path.Combine(Globals.NODE_VERSIONS_DIRECTORY, currentVersion, Globals.NPM_GLOBALS_STORAGE_FOLDER_NAME);

                if (!Directory.Exists(versionedNpmGlobalsDirectory))
                {
                    Directory.CreateDirectory(versionedNpmGlobalsDirectory);
                }
                else
                {
                    SharedMethods.DeleteDirectory(versionedNpmGlobalsDirectory, keepRootFolder:true);
                }

                SharedMethods.CopyDirectoryContents(Globals.NPM_GLOBALS_DIRECTORY, versionedNpmGlobalsDirectory);
                SharedMethods.DeleteDirectory(Globals.NPM_GLOBALS_DIRECTORY, keepRootFolder: true);
            }
        }
    }
}
