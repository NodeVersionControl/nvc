using CommandLine;
using NodeVersionControl.Model;
using NodeVersionControl.Operations;
using Serilog;
using Serilog.Core;
using Serilog.Events;
using System;
using System.Security.AccessControl;
using System.Text.Json;

namespace NodeVersionControl
{
    class Program
    {
        public class Options
        {
            [Option('l', "list", SetName = "list", HelpText = "Lists all installed NodeJS versions.")]
            public bool List { get; set; }

            [Option('c', "change", SetName = "change", HelpText = "Manually switch to a different version of NodeJS. Ex.: change nvc v20.0.1")]
            public string? Change { get; set; }

            [Option('i', "install", SetName = "install", HelpText = "Manually install a different version of NodeJS. Ex.: change nvc v20.0.1")]
            public string? Install { get; set; }

            [Option('s', "release", SetName = "release", HelpText = "Install from the version list.")]
            public bool Release { get; set; }

            [Option('v', "version", SetName = "version", HelpText = "Lists all installed versions of NodeJS. Select to change version.")]
            public bool Version { get; set; }

            [Option('r', "remove", SetName = "remove", HelpText = "Remove a NodeJS version.")]
            public string? Remove { get; set; }

            [Option('d', "debug", HelpText = "Enable debug mode.")]
            public bool Debug { get; set; }

        }

        //public async static Task Main()
        //{
        //    //colocar na instalaco do nvc
        //    //SetPermissions(Globals.NODE_DIRECTORY);
        //    //SetPermissions(Globals.NODE_VERSIONS_DIRECTORY);
        //    //SetPermissions(Globals.TEMP_FOLDER);

        //    Mains(new string[] { "--version" }).Wait();
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
                Main(new string[] { "--change", version }).Wait();

            }
            else if (args.Length == 1 && args[0] == "--version")
            {
                version = Version.ListVersions();
                Console.WriteLine($"Você escolheu {version}.");
                Main(new string[] { "--change", version }).Wait(); // instalr versao selecionada

            }

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

        private static void SetPermissions(string diretory)
        {
            string nodeInstallationPath = diretory; // Substitua pelo caminho real do Node

            try
            {
                // Obter o objeto de segurança atual da pasta
                DirectoryInfo directoryInfo = new DirectoryInfo(nodeInstallationPath);
                DirectorySecurity directorySecurity = directoryInfo.GetAccessControl();

                // Adicionar permissões para um usuário específico
                string username = Environment.UserName; // Ou substitua pelo nome do usuário desejado
                //FileSystemAccessRule accessRule = new FileSystemAccessRule(username, FileSystemRights.FullControl, AccessControlType.Allow);
                FileSystemAccessRule accessRule = new FileSystemAccessRule(username, FileSystemRights.FullControl, InheritanceFlags.ContainerInherit | InheritanceFlags.ObjectInherit, PropagationFlags.None, AccessControlType.Allow);
                directorySecurity.AddAccessRule(accessRule);

                // Definir o novo objeto de segurança com as permissões modificadas
                directoryInfo.SetAccessControl(directorySecurity);

                Console.WriteLine($"Permissões atribuídas com sucesso para {nodeInstallationPath}");
            }
            catch (UnauthorizedAccessException ex)
            {
                Console.WriteLine("Erro: A operação requer permissões elevadas. Execute o aplicativo como administrador.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao atribuir permissões: {ex.Message}");
            }
        }


    }
}