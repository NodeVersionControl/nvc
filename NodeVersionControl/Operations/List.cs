namespace NodeVersionControl
{
    internal static class List
    {
        public static void ListVersions()
        {
            DirectoryInfo versionsDir = new DirectoryInfo(Globals.NODE_VERSIONS_DIRECTORY);

            string currentVersion = SharedMethods.GetCurrentNodeVersion();

            if (Globals.DEBUG)
            {
                Console.WriteLine($"DEBUG: Searching directory {versionsDir} for all installed NodeJS versions.");
            }

            if(versionsDir.GetDirectories().Count() == 0)
            {
                Console.WriteLine("No versions of NodeJS currently installed.");
            }
            else
            {
                Console.WriteLine("Current installed versions:");
                foreach (DirectoryInfo versionDir in versionsDir.GetDirectories())
                {
                    if(versionDir.Name == currentVersion)
                        Console.WriteLine(versionDir.Name + " (Current Version)");
                    else
                        Console.WriteLine(versionDir.Name);
                }
            }
        }
    }
}
