namespace NotADoctor99.MetroPackageManager
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Security.Principal;
    using System.Windows.Media.Imaging;

    using Windows.ApplicationModel;
    using Windows.ApplicationModel.Core;
    using Windows.Management.Deployment;

    public class MetroPackageManager
    {
        private readonly Dictionary<String, MetroPackage> _packages = new Dictionary<String, MetroPackage>();
        private readonly Dictionary<String, MetroApplication> _applications = new Dictionary<String, MetroApplication>();

        public IReadOnlyCollection<MetroPackage> Packages => this._packages.Values;

        public IReadOnlyCollection<MetroApplication> Applications => this._applications.Values;

        public Boolean ReadPackagesForCurrentUser()
        {
            var userSecurityId = WindowsIdentity.GetCurrent().User.Value;
            return this.ReadPackages(userSecurityId);
        }

        public Boolean ReadPackages(String userSecurityId)
        {
            try
            {
                this._packages.Clear();
                this._applications.Clear();

                var packageManager = new PackageManager();

                var packages = packageManager.FindPackagesForUser(userSecurityId);

                foreach (var package in packages)
                {
                    if (package.IsFramework || !package.Status.VerifyIsOK())
                    {
                        continue;
                    }

                    this.ReadMetroPackage(package);
                }

                return true;
            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.Message);
                return false;
            }
        }

        private void ReadMetroPackage(Package package)
        {
            try
            {
                var entries = package.GetAppListEntries();

                if (0 == entries.Count)
                {
                    return;
                }

                var metroPackage = new MetroPackage(package.Id.Name, package.Id.FullName, package.DisplayName, package.Description,
                    new Version(package.Id.Version.Major, package.Id.Version.Minor, package.Id.Version.Build, package.Id.Version.Revision),
                    package.Id.FamilyName, package.PublisherDisplayName, package.InstalledPath, package.InstalledDate, package.Logo.LocalPath);

                foreach (var entry in entries)
                {
                    var metroApplication = this.ReadMetroApplication(entry);
                    this._applications[metroApplication.FullName] = metroApplication;
                    metroPackage.AddApplication(metroApplication);
                }

                this._packages[metroPackage.FullName] = metroPackage;
            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.Message);
            }
        }

        private MetroApplication ReadMetroApplication(AppListEntry entry)
        {
            var streamReference = entry.DisplayInfo.GetLogo(new Windows.Foundation.Size(80, 80));

            var logoBitmapImage = new BitmapImage();

            using (var randomAccessStream = streamReference.OpenReadAsync().AsTask().Result)
            {
                using (var stream = randomAccessStream.AsStream())
                {
                    logoBitmapImage.BeginInit();
                    logoBitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                    logoBitmapImage.StreamSource = stream;
                    logoBitmapImage.EndInit();
                
                }
            }

            var metroApplication = new MetroApplication(entry.AppInfo.Id, entry.AppUserModelId, entry.DisplayInfo.DisplayName, entry.DisplayInfo.Description, logoBitmapImage, entry.AppInfo.Package.Id.FullName);

            return metroApplication;
        }

        public static Boolean Launch(String applicationFullName)
        {
            try
            {
                var commandLine = $@"shell:AppsFolder\{applicationFullName}";
                Process.Start(commandLine);
                return true;
            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.Message);
                return false;
            }
        }
    }
}