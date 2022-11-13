using EnviromentProvider;
using EnviromentProvider.Model;
using File = EnviromentProvider.Model.File;

namespace DirectoryScanner
{
    internal class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            Worker worker = new Worker();
            worker.CollectData();
            worker.GenerateHtmlReport();
            Thread.Sleep(100);
            Application.DoEvents();
            Environment.Exit(0);
        }
    }
}