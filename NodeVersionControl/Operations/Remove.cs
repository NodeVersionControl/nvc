using Serilog;

namespace NodeVersionControl
{
    internal static class Remove
    {
        public static void RemoveVersion(string versionToRemove)
        {
            if (!versionToRemove.StartsWith('v'))
                versionToRemove = "v" + versionToRemove;

            if (versionToRemove.Length < 8)
            {
                Log.Logger.Debug($"Attempting to remove a version using a partial version name: {versionToRemove}.");
                versionToRemove = SharedMethods.MatchInstalledVersion(versionToRemove);
            }

            if (versionToRemove == SharedMethods.GetCurrentNodeVersion())
                throw new Exception($"Trying to remove your current NodeJS version ({versionToRemove}). Switch to another verison before removing.");

            string versionToRemovePath = Path.Combine(Globals.NODE_VERSIONS_DIRECTORY, versionToRemove);

            Log.Logger.Debug($"Searching for directory {versionToRemovePath} for NodeJS version {versionToRemove} to remove.");

            if (Directory.Exists(versionToRemovePath))
            {
                SharedMethods.DeleteDirectory(versionToRemovePath);

                if (Directory.Exists(versionToRemovePath))
                    Log.Logger.Information($"Version {versionToRemovePath} removed successfully.");
                else
                    Log.Logger.Information($"Failed to remove version {versionToRemovePath}.");
            }
            else
            {
                Log.Logger.Information($"Version {versionToRemove} did not exist.");
            }
        }
    }
}
