using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;

namespace Tasty.Utility
{
    public class MvcDotNetProjectMigrationOptions
    {
        public MvcDotNetProjectMigrationOptions(string projectRootPath, string tempWorkingDirectoryPath, IEnumerable<string> extraDirectories, IEnumerable<string> extraFiles)
        {
            TempWorkingDirectory = new DirectoryInfo(Path.Combine(Directory.GetCurrentDirectory(), tempWorkingDirectoryPath));
            var projectRoot = new DirectoryInfo(projectRootPath);
            SourceWebConfig = projectRoot.GetFile("Web.config");
            SourceFiles = ListFiles(projectRoot, extraFiles);
            SourceDirectories = ListDirectories(projectRoot, extraDirectories);
            OverwriteWebConfig = delegate { };
        }

        private IEnumerable<FileInfo> ListFiles(DirectoryInfo projectRoot, IEnumerable<string> extraFiles)
        {
            yield return projectRoot.GetFile("Global.asax");
            foreach (var fileName in extraFiles)
                yield return projectRoot.GetFile(fileName);
        }

        private IEnumerable<DirectoryInfo> ListDirectories(DirectoryInfo projectRoot, IEnumerable<string> extraDirectories)
        {
            yield return projectRoot.GetDirectory("bin");
            foreach (var directoryName in extraDirectories)
                yield return projectRoot.GetDirectory(directoryName);
        }

        public IEnumerable<FileInfo> SourceFiles { get; private set; }
        public IEnumerable<DirectoryInfo> SourceDirectories { get; private set; }
        public FileInfo SourceWebConfig { get; private set; }
        public DirectoryInfo TempWorkingDirectory { get; set; }
        public Action<Configuration> OverwriteWebConfig { get; set; }
    }
}
