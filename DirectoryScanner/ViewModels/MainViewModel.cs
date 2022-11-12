using DirectoryScanner.Models;
using EnviromentProvider;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

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
