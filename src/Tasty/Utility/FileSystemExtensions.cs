using System.IO;

namespace Tasty.Utility
{
    public static class DirectoryExtensions
    {
        public static void CopyTo(this DirectoryInfo directory, string target)
        {
            CopyTo(directory, new DirectoryInfo(target));
        }

        public static void CopyTo(this DirectoryInfo source, DirectoryInfo target)
        {
            if (!target.Exists)
                target.Create();
            foreach (var file in source.GetFiles())
            {
                file.CopyTo(Path.Combine(target.FullName, file.Name), true);
            }
            foreach (var childDirectory in source.GetDirectories())
            {
                CopyTo(childDirectory, new DirectoryInfo(Path.Combine(target.FullName, childDirectory.Name)));
            }
        }

        public static DirectoryInfo GetDirectory(this DirectoryInfo directory, string name)
        {
            return new DirectoryInfo(Path.Combine(directory.FullName, name));
        }

        public static FileInfo GetFile(this DirectoryInfo directory, string name)
        {
            return new FileInfo(Path.Combine(directory.FullName, name));
        }

        public static void CopyTo(this FileInfo file, DirectoryInfo target)
        {
            file.CopyTo(Path.Combine(target.FullName, file.Name), true);
        }
    }
}
