using EnviromentProvider.Model;
using File = EnviromentProvider.Model.File;

namespace EnviromentProvider
{
    public class GenerateDocument
    {
        public HtmlDocument Document;
        public GenerateDocument(HtmlDocument document)
        {
            Document = document;

            HtmlElement formater = Document.CreateElement("CENTER");
            Document.Body.AppendChild(formater);
            HtmlElement h1 = Document.CreateElement("H1");
            h1.InnerText = "Scan result";
            formater.AppendChild(h1);
            
        }
        /// <summary>
        /// Recursively construct tree view of enviroment structure
        /// </summary>
        /// <param name="enviroment"></param>
        /// <returns></returns>
        public GenerateDocument AddTreeView(Folder enviroment)
        {
            HtmlElement subHeader = Document.CreateElement("H2");
            subHeader.InnerText = "Tree view enviroment";
            Document.Body.AppendChild(subHeader);

            HtmlElement treeView = Document.CreateElement("UL");
            Document.Body.AppendChild(treeView);
            AddTreeElement(ref Document, treeView, enviroment);
            return this;
        }

        /// <summary>
        /// Recursive fill HTML document with tree view items
        /// </summary>
        /// <param name="document"></param>
        /// <param name="parent">Tree view parent HtmlElement</param>
        /// <param name="folder"></param>
        private static void AddTreeElement(ref HtmlDocument document, HtmlElement parent, Folder folder)
        {
            HtmlElement currEl = document.CreateElement("LI");
            currEl.InnerText = folder.Name + "\t|\t" + Folder.GetFormatedSize(folder.Size);
            parent.AppendChild(currEl);

            if (folder.Items.Count != 0)
            {
                HtmlElement subLvl = document.CreateElement("UL");
                currEl.AppendChild(subLvl);

                foreach (var item in folder.Items.Where(x => x.Type == "file"))
                {
                    File file = (File)item;
                    HtmlElement subEl = document.CreateElement("LI");
                    subEl.InnerText = file.Name + "\t|\t" + file.MimeType + "\t|\t" + File.GetFormatedSize(file.Size);
                    subLvl.AppendChild(subEl);
                }

                foreach (var item in folder.Items.Where(x => x.Type == "folder"))
                {
                    Folder subFolder = (Folder)item;
                    AddTreeElement(ref document, subLvl, subFolder); //recursive fill
                }
            }
            return;
        }

        public GenerateDocument AddMimeStatistics(List<File> files)
        {
            var subHeader = Document.CreateElement("H2");
            subHeader.InnerText = "MIME statistics";
            Document.Body.AppendChild(subHeader);

            if (files.Count == 0)
            {
                HtmlElement noStats = Document.CreateElement("H4");
                noStats.InnerText = "Not enought files for statistic";
                Document.Body.AppendChild(noStats);
                return this;
            }
            var table = Document.CreateElement("TABLE");
            Document.Body.AppendChild(table);
            var tableHeader = Document.CreateElement("THEAD");
            table.AppendChild(tableHeader);
            var tableRow = Document.CreateElement("TR");
            tableHeader.AppendChild(tableRow);

            //html table headers
            var talbeHeaderCell = Document.CreateElement("TH");
            talbeHeaderCell.InnerText = "Mime type";
            tableRow.AppendChild(talbeHeaderCell);
            talbeHeaderCell = Document.CreateElement("TH");
            talbeHeaderCell.InnerText = "Count";
            tableRow.AppendChild(talbeHeaderCell);
            talbeHeaderCell = Document.CreateElement("TH");
            talbeHeaderCell.InnerText = "% Ratio";
            tableRow.AppendChild(talbeHeaderCell);
            talbeHeaderCell = Document.CreateElement("TH");
            talbeHeaderCell.InnerText = "All size";
            tableRow.AppendChild(talbeHeaderCell);
            talbeHeaderCell = Document.CreateElement("TH");
            talbeHeaderCell.InnerText = "Avg size";
            tableRow.AppendChild(talbeHeaderCell);
            
            
            List<string> mimeTypes = files.Select(x => x.MimeType).Distinct().ToList();
            var tableBody = Document.CreateElement("TBODY");
            table.AppendChild(tableBody);

            
            foreach (var mimeType in mimeTypes)
            {
                tableRow = Document.CreateElement("TR");
                tableBody.AppendChild(tableRow);
                //table cells
                var tableCell = Document.CreateElement("TD");
                //mime type
                tableCell.InnerText = mimeType;
                tableRow.AppendChild(tableCell);
                tableCell = Document.CreateElement("TD");
                //file count per mime type
                tableCell.InnerText = files
                    .Where(x => x.MimeType == mimeType)
                    .Count().ToString();
                tableRow.AppendChild(tableCell);
                tableCell = Document.CreateElement("TD");
                //% ratio of files per mime type
                tableCell.InnerText = Math.Round(((double)files
                    .Where(x => x.MimeType == mimeType)
                    .Count() / (double)files.Count()) * 100, 2).ToString() + "%";
                tableRow.AppendChild(tableCell);
                tableCell = Document.CreateElement("TD");
                //sum size of all files per mime type
                tableCell.InnerText = Item.GetFormatedSize(files
                    .Where(x => x.MimeType == mimeType)
                    .Sum(x => x.Size));
                tableRow.AppendChild(tableCell);
                tableCell = Document.CreateElement("TD");
                //average size of files per mime type
                tableCell.InnerText = Item.GetFormatedSize(files
                    .Where(x => x.MimeType == mimeType)
                    .Sum(x => x.Size) / files.Where(x => x.MimeType == mimeType).Count());
                tableRow.AppendChild(tableCell);
            }
            return this;
        }

        /// <summary>
        /// Save HTML document at current directory
        /// </summary>
        public void SaveDocument()
        {
            FileStream fs = new FileStream(@"ScanResult.html", FileMode.OpenOrCreate, FileAccess.Write);
            using (StreamWriter sw = new StreamWriter(fs))
            {
                sw.WriteLine(Document.Body.InnerHtml);
            }
        }
    }
}
