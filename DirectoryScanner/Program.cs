using DirectoryScanner;

var currentDirectoryPath = Environment.CurrentDirectory;

var envInfo = EnviromentProvider.ScanEnviroment(currentDirectoryPath);