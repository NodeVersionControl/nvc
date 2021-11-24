namespace NodeVersionControl
{
    internal static class Remove
    {
        public static void RemoveVersion(string versionToRemove)
        {
            if (!versionToRemove.StartsWith('v'))
                versionToRemove = "v" + versionToRemove;

            if (versionToRemove == SharedMethods.GetCurrentNodeVersion())
                throw new Exception("Trying to remove your current NodeJS version. Switch to another verison before removing.");

            string versionToRemovePath = Path.Combine(Globals.NODE_VERSIONS_DIRECTORY, versionToRemove);

            if (Directory.Exists(versionToRemovePath))
            {
                SharedMethods.DeleteDirectory(versionToRemovePath);

                if (Directory.Exists(versionToRemovePath))
                    Console.WriteLine($"Version {versionToRemovePath} removed successfully.");
                else
                    Console.WriteLine($"Failed to remove version {versionToRemovePath}.");
            }
            else
            {
                Console.WriteLine($"Version {versionToRemovePath} did not exist.");
            }
        }
    }
}
