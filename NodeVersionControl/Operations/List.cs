using Serilog;

namespace NodeVersionControl
{
    internal static class List
    {
        public static void ListVersions()
        {
            DirectoryInfo versionsDir = new DirectoryInfo(Globals.NODE_VERSIONS_DIRECTORY);

            Log.Logger.Debug($"Searching directory {versionsDir} for all installed NodeJS versions.");

            if(versionsDir.GetDirectories().Count() == 0)
            {
                Log.Logger.Information("No versions of NodeJS currently installed.");
            }
            else
            {
                LogVersions(versionsDir);
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
