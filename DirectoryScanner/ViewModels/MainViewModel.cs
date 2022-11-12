using DirectoryScanner.Models;
using System;
namespace DirectoryScanner.ViewModels
{
    public class MainViewModel
    {
        public MainViewModel()
        {
            var envInfo = EnviromentProvider.EnviromentProvider.ScanEnviroment(Environment.CurrentDirectory);

            var document = new GenerateDocument(envInfo);
        }
    }
}
