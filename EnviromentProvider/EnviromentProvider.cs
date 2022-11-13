using EnviromentProvider.Model;

namespace EnviromentProvider
{
    public class EnviromentProvider
    {
        /// <summary>
        /// Represents enviroment as Folder with Item collection
        /// </summary>
        /// <param name="currentPath"></param>
        /// <param name="files">Files represented as collection</param>
        /// <returns></returns>
        public static Folder ScanEnviroment(string currentPath, ref List<Model.File> files)
        {
            var currentDirectory = new DirectoryInfo(currentPath);

            var currentFolder = new Folder
            {
                Path = currentDirectory.FullName,
                Name = currentDirectory.Name,
                Type = "folder",
                Items = ScanDirectory(currentDirectory, ref files)
            };
            currentFolder.Size = currentFolder.Items.Sum(item=> item.Size);
            return currentFolder;
        }

        /// <summary>
        /// Recursive search for files and sub-directories in selected directory
        /// </summary>
        /// <param name="directory"></param>
        /// <param name="items"></param>
        private static List<Item> ScanDirectory(DirectoryInfo directory, ref List<Model.File> files)
        {
            var items = new List<Item>();
            //Parse files
            foreach (var file in directory.GetFiles())
            {
                if ((directory.Attributes & FileAttributes.ReparsePoint) != FileAttributes.ReparsePoint &&
                (directory.Attributes & FileAttributes.System) != FileAttributes.System)
                {
                    var curFile = new Model.File
                    {
                        Name = file.Name,
                        Path = file.FullName,
                        Size = file.Length,
                        Type = "file",
                        MimeType = MimeTypes.GetMimeType(file.Name)
                    };
                    items.Add(curFile);
                    files.Add(curFile);
                }
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
                        Items = ScanDirectory(dir, ref files) //recursive search
                    };
                    folder.Size = folder.Items.Sum(item => item.Size);

                    items.Add(folder);
                }
            }
            return items;
        }
    }
}
