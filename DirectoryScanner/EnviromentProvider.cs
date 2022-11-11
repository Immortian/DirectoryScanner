using DirectoryScanner.Model;

namespace DirectoryScanner
{
    public class EnviromentProvider
    {
        /// <summary>
        /// Represents enviroment as Folder with Item collection
        /// </summary>
        /// <param name="currentPath"></param>
        /// <returns></returns>
        public static Folder ScanEnviroment(string currentPath)
        {
            var currentDirectory = new DirectoryInfo(currentPath);

            var currentFolder = new Folder
            {
                Path = currentDirectory.FullName,
                Name = currentDirectory.Name,
                Type = "folder",
                Items = ScanDirectory(currentDirectory)
            };
            currentFolder.Size = currentFolder.Items.Sum(item=> item.Size);
            return currentFolder;
        }

        /// <summary>
        /// Recursive search for files and sub-directories in selected directory
        /// </summary>
        /// <param name="directory"></param>
        /// <param name="items"></param>
        private static List<Item> ScanDirectory(DirectoryInfo directory)
        {
            var items = new List<Item>();
            //Parse files
            foreach (var file in directory.GetFiles())
            {
                if ((directory.Attributes & FileAttributes.ReparsePoint) != FileAttributes.ReparsePoint &&
                (directory.Attributes & FileAttributes.System) != FileAttributes.System)
                    items.Add(new Model.File
                    {
                        Name = file.Name,
                        Path = file.FullName,
                        Size = file.Length,
                        Type = "file",
                        MimeType = MimeTypes.GetMimeType(file.Name)
                    });
            }
            //Parse sub-directories
            foreach (var dir in directory.GetDirectories())
            {
                if ((dir.Attributes & FileAttributes.ReparsePoint) != FileAttributes.ReparsePoint &&
                (dir.Attributes & FileAttributes.System) != FileAttributes.System)
                {
                    var folder = new Folder
                    {
                        Name = dir.Name,
                        Path = dir.FullName,
                        Type = "folder",
                        Items = ScanDirectory(dir) //recursive search
                    };
                    folder.Size = folder.Items.Sum(item => item.Size);

                    items.Add(folder);
                }
            }
            return items;
        }
    }
}
