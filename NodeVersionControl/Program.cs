using CommandLine;
using NodeVersionControl.Model;
using NodeVersionControl.Operations;
using Serilog;
using Serilog.Core;
using Serilog.Events;
using System;
using System.Text.Json;

namespace NodeVersionControl
{
    class Program
    {
        public class Options
        {
            [Option('c', "change", SetName = "change", HelpText = "Change to a different NodeJS version.")]
            public string? Change { get; set; }

            [Option('r', "remove", SetName = "remove", HelpText = "Remove a NodeJS version.")]
            public string? Remove { get; set; }

            [Option('i', "install", SetName = "install", HelpText = "Installs a new NodeJS version.")]
            public string? Install { get; set; }

            [Option('l', "list", SetName = "list", HelpText = "Lists all installed NodeJS versions.")]
            public bool List { get; set; }

            [Option('d', "debug", HelpText = "Enable debug mode.")]
            public bool Debug { get; set; }
            [Option('s', "release", SetName = "release", HelpText = "List all release.")]
            public bool Release { get; set; }
        }
        //public async static Task Main()
        //{

        //    Mains(new string[] { "--release" }).Wait();
        //}

        public async static Task Main(string[] args)
        {
            string version = "";
            if (args.Length == 0)
            {
                Log.Logger.Information("No arguments provided.");
                return;
            }
            else if (args.Length == 1 && args[0] == "--release")
            {
                version = await Release.GetVersion();
                Console.WriteLine($"Você escolheu {version}.");
                Main(new string[] { "--install", version }).Wait(); // instalr versao selecionada

            }
            //Install.InstallVersion("v22.0.0");
            else
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
                               //Main(new string[] { "--change ", version }).Wait(); //trocar versao apos o download
                           }
                           else if (o.List)
                           {
                               List.ListVersions();
                           }
                           else
                           {
                               Log.Logger.Information("Command not recognized.");
                           }
                       });

                }
                catch (Exception ex)
                {
                    Log.Logger.Error(ex.Message);
                    Log.Logger.Debug(ex.StackTrace);
                }

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
                .WriteTo.File(
                    Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "log.txt"),
                    retainedFileCountLimit: 2,
                    rollOnFileSizeLimit: true
                    )
                .MinimumLevel.ControlledBy(lls)
                .CreateLogger();
        }
    }
}