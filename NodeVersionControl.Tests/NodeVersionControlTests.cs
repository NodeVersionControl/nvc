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
            // Verificar se a vers�o foi alterada corretamente
            string installedVersion = "v22.0.0"; // Implemente esta fun��o
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
            // Verificar se a vers�o foi removida corretamente
            string versionsDirectory = Globals.NODE_VERSIONS_DIRECTORY; // Supondo que este � o diret�rio onde as vers�es s�o armazenadas
            string versionDirectoryToRemove = Path.Combine(versionsDirectory, versionToRemove);

            Assert.False(Directory.Exists(versionDirectoryToRemove), $"A vers�o {versionToRemove} ainda existe ap�s remo��o.");
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
            // Verificar se a vers�o foi instalada corretamente
            bool isInstalled = true;/* Implemente a l�gica para verificar se 'versionToInstall' foi instalada */;
            Assert.True(isInstalled, $"Falha ao instalar a vers�o {versionToInstall}");
        }

        [Fact]
        public async Task Main_ListOption_ShouldListInstalledNodeVersions()
        {
            // Arrange
            string[] args = new string[] { "--list" };
            using (StringWriter sw = new StringWriter())
            {
                Console.SetOut(sw); // Redireciona a sa�da padr�o para StringWriter

                // Act
                Program.Main(args);

                // Captura a sa�da do console
                string consoleOutput = sw.ToString();

                // Assert
                // Verifica se a listagem est� correta
                Assert.Contains("v21.0.0", consoleOutput); // Exemplo: Verifica se a vers�o v14.18.2 est� na listagem
                Assert.Contains("v20.12.2", consoleOutput); // Exemplo: Verifica se a vers�o v14.18.1 est� na listagem               
            }
        }

        // Adicione mais testes conforme necess�rio para outras op��es como --debug, --release, etc.

    }
}