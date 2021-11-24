namespace NodeVersionControl
{
    internal static class List
    {
        public static void ListVersions()
        {
            DirectoryInfo versionsDir = new DirectoryInfo(Globals.NODE_VERSIONS_DIRECTORY);

            if(versionsDir.GetDirectories().Count() == 0)
            {
                Console.WriteLine("No versions of NodeJS currently installed.");
            }
            else
            {
                Console.WriteLine("Current installed versions:");
                foreach (DirectoryInfo versionDir in versionsDir.GetDirectories())
                {
                    Console.WriteLine(versionDir.Name);
                }
            }
        }
    }
}
