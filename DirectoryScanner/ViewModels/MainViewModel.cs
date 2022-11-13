using EnviromentProvider;
using EnviromentProvider.Model;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace DirectoryScanner.ViewModels
{
    public class MainViewModel
    {
        private Folder enviroment;
        public Folder Enviroment { get { return enviroment; } set { enviroment = value; } }

        private List<File> files;
        public List<File> Files { get { return files; } set { files = value; } }
        public MainViewModel()
        {
            List<File> fileList = new List<File>();
            Enviroment = EnviromentProvider.EnviromentProvider.ScanEnviroment(Environment.CurrentDirectory, ref fileList);
            Files = fileList;

            var webBrowser = new WebBrowser();
            webBrowser.Url = new Uri("about:blank");
            webBrowser.DocumentCompleted += WebBrowser_DocumentCompleted;
        }

        private void WebBrowser_DocumentCompleted(object? sender, WebBrowserDocumentCompletedEventArgs e)
        {
            var docCore = new GenerateDocument((sender as WebBrowser).Document);
            docCore.AddTreeView(Enviroment)
                .AddMimeStatistics(Files)
                .SaveDocument();
        }
    }
}
