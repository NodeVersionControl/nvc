using CommandLine;
using Serilog;
using Serilog.Core;
using Serilog.Events;

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
                   .WithParsed(o =>
                   {
                       SetupSerilog(o.Debug);
                       SetupFileStructure();

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
                Log.Logger.Error(ex.Message);
                Log.Logger.Debug(ex.StackTrace);
            }
        }

        private static void SetupFileStructure()
        {
            if (!Directory.Exists(Globals.NODE_VERSIONS_DIRECTORY))
            {
                Log.Logger.Debug($"Creating NodeJS Versions Directory {Globals.NODE_VERSIONS_DIRECTORY}");
                Directory.CreateDirectory(Globals.NODE_VERSIONS_DIRECTORY);
            }

            if (!Directory.Exists(Globals.NODE_DIRECTORY))
            {
                Log.Logger.Debug($"Creating NodeJS Directory {Globals.NODE_DIRECTORY}");
                Directory.CreateDirectory(Globals.NODE_DIRECTORY);
            }

            if (!Directory.Exists(Globals.TEMP_FOLDER))
            {
                Log.Logger.Debug($"Creating NVC Temp Directory {Globals.TEMP_FOLDER}");
                Directory.CreateDirectory(Globals.TEMP_FOLDER);
            }
        }

        private static void SetupSerilog(bool debug)
        {
            LoggingLevelSwitch lls = new LoggingLevelSwitch() 
            { 
                MinimumLevel = (debug) ? LogEventLevel.Debug : LogEventLevel.Information 
            };

            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console()
                .WriteTo.File(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "log.txt"))
                .MinimumLevel.ControlledBy(lls)
                .CreateLogger();
        }
    }
}