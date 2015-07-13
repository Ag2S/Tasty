using System.Configuration;
using System.IO;
using Tasty.StandaloneHttpServer;

namespace Tasty.Utility
{
    public static class MvcDotNetProjectExtensions
    {
        public static void Start(this IStandaloneHttpServer server, short port, MvcDotNetProjectMigrationOptions options)
        {
            if (!options.TempWorkingDirectory.Exists)
                options.TempWorkingDirectory.Create();

            foreach (var file in options.SourceFiles)
                file.CopyTo(options.TempWorkingDirectory);

            foreach (var directory in options.SourceDirectories)
                directory.CopyTo(options.TempWorkingDirectory.GetDirectory(directory.Name));

            var testConfig = ConfigurationManager.OpenMappedExeConfiguration(
                new ExeConfigurationFileMap { ExeConfigFilename = options.SourceWebConfig.FullName },
                ConfigurationUserLevel.None);
            options.OverwriteWebConfig(testConfig);
            testConfig.SaveAs(Path.Combine(options.TempWorkingDirectory.FullName, "Web.config"));

            server.Start(port, options.TempWorkingDirectory.FullName);
        }
    }
}
