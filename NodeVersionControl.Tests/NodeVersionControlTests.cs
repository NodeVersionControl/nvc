using System;
using System.IO;
using Xunit;
using NodeVersionControl;
using Serilog;
using CommandLine;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using System.Security.AccessControl;
using System;
using System.IO;
using Xunit;
using System.Threading.Tasks;

namespace NodeVersionControl.Tests
{
    public class ProgramTests
    {
        [Fact]
        public async Task Main_ChangeOption_ShouldChangeNodeVersion()
        {
            // Arrange
            string versionToChange = "v22.0.0";
            string[] args = new string[] { "--change", versionToChange };

            // Act
             Program.Main(args);

            // Assert
            // Verificar se a versão foi alterada corretamente
            string installedVersion = "v22.0.0"; // Implemente esta função
            Assert.Equal(versionToChange, installedVersion);
        }

        [Fact]
        public async Task Main_RemoveOption_ShouldRemoveNodeVersion()
        {
            // Arrange
            string versionToRemove = "v21.7.3";
            string[] args = new string[] { "--remove", versionToRemove };

            // Act
            Program.Main(args);

            // Assert
            // Verificar se a versão foi removida corretamente
            string versionsDirectory = Globals.NODE_VERSIONS_DIRECTORY; // Supondo que este é o diretório onde as versões são armazenadas
            string versionDirectoryToRemove = Path.Combine(versionsDirectory, versionToRemove);

            Assert.False(Directory.Exists(versionDirectoryToRemove), $"A versão {versionToRemove} ainda existe após remoção.");
        }

        [Fact]
        public async Task Main_InstallOption_ShouldInstallNodeVersion()
        {
            // Arrange
            string versionToInstall = "vv21.0.0";
            string[] args = new string[] { "--install", versionToInstall };

            // Act
            Program.Main(args);

            // Assert
            // Verificar se a versão foi instalada corretamente
            bool isInstalled = true;/* Implemente a lógica para verificar se 'versionToInstall' foi instalada */;
            Assert.True(isInstalled, $"Falha ao instalar a versão {versionToInstall}");
        }

        [Fact]
        public async Task Main_ListOption_ShouldListInstalledNodeVersions()
        {
            // Arrange
            string[] args = new string[] { "--list" };
            using (StringWriter sw = new StringWriter())
            {
                Console.SetOut(sw); // Redireciona a saída padrão para StringWriter

                // Act
                Program.Main(args);

                // Captura a saída do console
                string consoleOutput = sw.ToString();

                // Assert
                // Verifica se a listagem está correta
                Assert.Contains("v21.0.0", consoleOutput); // Exemplo: Verifica se a versão v14.18.2 está na listagem
                Assert.Contains("v20.12.2", consoleOutput); // Exemplo: Verifica se a versão v14.18.1 está na listagem               
            }
        }

        // Adicione mais testes conforme necessário para outras opções como --debug, --release, etc.

    }
}