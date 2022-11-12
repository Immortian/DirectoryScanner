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
                double tempSize = Size;
                int divideCount = 0;
                for (int i = 0; i < 4; i++)
                {
                    if (tempSize > 1024)
                    {
                        divideCount++;
                        tempSize = Math.Round(tempSize /= 1024, 2);
                    }
                }
                switch (divideCount)
                {
                    case 0: return $"{tempSize} Bytes"; break;
                    case 1: return $"{tempSize} KB"; break;
                    case 2: return $"{tempSize} MB"; break;
                    case 3: return $"{tempSize} GB"; break;
                    default: return tempSize.ToString();
                }
            }
        }
    }
}
