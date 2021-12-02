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
            {
                Console.WriteLine($"Already using version {newVersion} of NodeJS.");
                return;
            }

            string newVersionPath = Path.Combine(Globals.NODE_VERSIONS_DIRECTORY, newVersion);

            if (!Directory.Exists(newVersionPath))
            {
                Console.WriteLine($"NodeJS version {newVersion} not installed.");
                return;
            }

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
            {
                if (Globals.DEBUG)
                {
                    Console.WriteLine($"DEBUG: Removing files in working NodeJS directory {Globals.NODE_DIRECTORY}");
                }

                SharedMethods.DeleteDirectory(Globals.NODE_DIRECTORY, keepRootFolder: true);
            }
            else
            {
                Directory.CreateDirectory(Globals.NODE_DIRECTORY);
            }

            //Copy over requested NodeJS version
            if (Globals.DEBUG)
            {
                Console.WriteLine($"DEBUG: Copying new NodeJS version from {versionDirectory} into working NodeJS directory.");
            }

            SharedMethods.CopyDirectoryContents(versionDirectory, Globals.NODE_DIRECTORY);
        }

        private static void RestoreGlobalNodeModules(string versionDirectory)
        {
            string npmGlobalsStorageFolder = Path.Combine(versionDirectory, Globals.NPM_GLOBALS_STORAGE_FOLDER_NAME);

            if (Globals.DEBUG)
            {
                Console.WriteLine($"DEBUG: Restoring NPM Global packages from storage folder {npmGlobalsStorageFolder} to {Globals.NPM_GLOBALS_DIRECTORY}");
            }

            if (Directory.Exists(npmGlobalsStorageFolder))
            {
                //Copy over global NPM packages
                SharedMethods.CopyDirectoryContents(npmGlobalsStorageFolder, Globals.NPM_GLOBALS_DIRECTORY);
            }
            else if (Globals.DEBUG)
            {
                Console.WriteLine($"DEBUG: No stored NPM global modules found.");
            }
            
        }

        private static void StoreGlobalNodeModules(string currentVersion)
        {
            //Delete the node modules stored in the versions directory
            if (Directory.Exists(Globals.NPM_GLOBALS_DIRECTORY))
            {
                string versionedNpmGlobalsDirectory = Path.Combine(Globals.NODE_VERSIONS_DIRECTORY, currentVersion, Globals.NPM_GLOBALS_STORAGE_FOLDER_NAME);

                if (Globals.DEBUG)
                {
                    Console.WriteLine($"DEBUG: Saving NPM Globals Directory to storage folder. {versionedNpmGlobalsDirectory}");
                }

                if (!Directory.Exists(versionedNpmGlobalsDirectory))
                {
                    Directory.CreateDirectory(versionedNpmGlobalsDirectory);
                }
                else
                {
                    if (Globals.DEBUG)
                    {
                        Console.WriteLine($"DEBUG: Version {currentVersion} contains old NPM Global Packages, purging folder {versionedNpmGlobalsDirectory}");
                    }

                    SharedMethods.DeleteDirectory(versionedNpmGlobalsDirectory, keepRootFolder:true);
                }

                SharedMethods.CopyDirectoryContents(Globals.NPM_GLOBALS_DIRECTORY, versionedNpmGlobalsDirectory);

                if (Globals.DEBUG)
                {
                    Console.WriteLine($"DEBUG: Purging NPM Globals Directory for next version.");
                }

                SharedMethods.DeleteDirectory(Globals.NPM_GLOBALS_DIRECTORY, keepRootFolder: true);
            }
            else if(Globals.DEBUG)
            {
                Console.WriteLine($"DEBUG: NPM Globals package folder {Globals.NPM_GLOBALS_DIRECTORY} did not exist.");
            }
        }
    }
}
