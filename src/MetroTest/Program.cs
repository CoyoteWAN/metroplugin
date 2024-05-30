namespace NotADoctor99.MetroPackageManager
{
    using System;
    using System.Diagnostics;
    using System.IO;

     class Program
    {
        static void Main(String[] _)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            var metroPackageManager = new MetroPackageManager();
            metroPackageManager.ReadPackagesForCurrentUser();

            Console.WriteLine($"{metroPackageManager.Applications.Count} applications read in {stopwatch.Elapsed.TotalMilliseconds:N0} ms");

            foreach (var app in metroPackageManager.Applications)
            {
                Console.WriteLine($"{app.DisplayName} | {app.DisplayName}");
                var temporaryDirectory = @"C:\temp\_metro\";
                var fileName = $"{app.DisplayName}.png".Replace(':', '_');
                Directory.CreateDirectory(temporaryDirectory);
                app.WriteLogoToPngFile($@"C:\temp\_metro\{fileName}");
            }
        }
    }
}
