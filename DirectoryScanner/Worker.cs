using EnviromentProvider;
using EnviromentProvider.Model;
using File = EnviromentProvider.Model.File;

namespace DirectoryScanner
{
    internal class Worker
    {
        public Folder Enviroment;
        public List<File> Files;
        public void CollectData()
        {
            List<File> fileList = new List<File>();
            Enviroment = EnviromentProvider.EnviromentProvider.ScanEnviroment(Environment.CurrentDirectory, ref fileList);
            Files = fileList;
        }
        [STAThread]
        public void GenerateHtmlReport()
        {
            var webBrowser = new WebBrowser();
            webBrowser.Url = new Uri("about:blank");
            webBrowser.DocumentCompleted += WebBrowser_DocumentCompleted;
        }
        [STAThread]
        private void WebBrowser_DocumentCompleted(object? sender, WebBrowserDocumentCompletedEventArgs e)
        {
            var docCore = new GenerateDocument((sender as WebBrowser).Document);
            docCore.AddTreeView(Enviroment)
                .AddMimeStatistics(Files)
                .SaveDocument();
        }
    }
}
