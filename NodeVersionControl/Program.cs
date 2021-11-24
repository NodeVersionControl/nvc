using CommandLine;

namespace NodeVersionControl
{
    class Program
    {
        public class Options
        {
            [Option('c', "change", Required = false, HelpText = "Change to a different NodeJS version.")]
            public string Change { get; set; }

            [Option('r', "remove", Required = false, HelpText = "Remove a NodeJS version.")]
            public string Remove { get; set; }

            [Option('i', "install", Required = false, HelpText = "Installs a new NodeJS version.")]
            public string Install { get; set; }

            [Option('d', "debug", Required = false, HelpText ="Enable debug mode.")]
            public bool Debug { get; set; }

            [Option('l', "list", Required = false, HelpText = "Lists all installed NodeJS versions.")]
            public bool List { get; set; }
        }

        static void Main(string[] args)
        {
            try
            {
                Parser.Default.ParseArguments<Options>(args)
                   .WithParsed<Options>(o =>
                   {
                       Globals.DEBUG = o.Debug;

                       if (!string.IsNullOrEmpty(o.Change))
                       {
                           Change.ChangeVersion(o.Change);
                       }

                       else if (!string.IsNullOrEmpty(o.Remove))
                       {
                           Remove.RemoveVersion(o.Remove);
                       }

                       else if (!string.IsNullOrEmpty(o.Install))
                       {
                           Install.InstallVersion(o.Install);
                       }

                       else if (o.List)
                       {
                           List.ListVersions();
                       }
                   });
            }
            catch(Exception ex)
            {
                Console.WriteLine("Error encountered...");
                Console.WriteLine(ex.Message);

                if (Globals.DEBUG)
                {
                    Console.WriteLine(ex.StackTrace);
                }
            }
        }

    }
}