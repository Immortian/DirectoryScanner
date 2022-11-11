namespace DirectoryScanner.Model
{
    public class Item
    {
        public string Path { get; set; }
        public long Size { get; set; }
        public string Name { get; set; }
        //File or folder
        public string Type { get; set; }

        public string FormatedSize
        {
            get
            {
                int divideCount = 0;
                for (int i = 0; i < 4; i++)
                {
                    if (Size > 1024)
                    {
                        divideCount++;
                        Size /= 1024;
                    }
                }
                switch (divideCount)
                {
                    case 0: return $"{Size} Bytes"; break;
                    case 1: return $"{Size} KB"; break;
                    case 2: return $"{Size} MB"; break;
                    case 3: return $"{Size} GB"; break;
                    default: return Size.ToString();
                }
            }
        }
    }
}
