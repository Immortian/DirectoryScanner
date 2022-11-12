using EnviromentProvider.Model;
using File = EnviromentProvider.Model.File;

namespace DirectoryScanner.Models
{
    public class GenerateDocument
    {
        Folder _folder;
        public GenerateDocument(Folder folder)
        {
            _folder = folder;

            var webBrowser = new WebBrowser();
            webBrowser.Url = new Uri("about:blank");
            webBrowser.DocumentCompleted += WebBrowser_DocumentCompleted;
        }

        private void WebBrowser_DocumentCompleted(object? sender, WebBrowserDocumentCompletedEventArgs e)
        {
            var document = (sender as WebBrowser).Document;
            if (document != null)
            {
                HtmlElement header = document.CreateElement("H1");
                header.InnerText = "Scan result";
                document.Body.AppendChild(header);

                HtmlElement treeView = document.CreateElement("UL");
                document.Body.AppendChild(treeView);
                AddTreeElement(ref document, treeView, _folder);
            }

            //Save HtmlDocument as file in current directory
            FileStream fs = new FileStream(@"ScanResult.html", FileMode.OpenOrCreate, FileAccess.Write);
            using (StreamWriter sw = new StreamWriter(fs))
            {
                sw.WriteLine(document.Body.InnerHtml);
            }
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
            currEl.InnerText = folder.Name + "\t|\t" + folder.FormatedSize;
            parent.AppendChild(currEl);

            if (folder.Items.Count != 0)
            {
                HtmlElement subLvl = document.CreateElement("UL");
                currEl.AppendChild(subLvl);

                foreach (var item in folder.Items.Where(x => x.Type == "file"))
                {
                    File file = (File)item;
                    HtmlElement subEl = document.CreateElement("LI");
                    subEl.InnerText = file.Name + "\t|\t" + file.MimeType + "\t|\t" + file.FormatedSize;
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
    }
}
