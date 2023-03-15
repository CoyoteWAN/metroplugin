namespace NotADoctor99.MetroPackageManager
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Windows.Media.Imaging;

    public class MetroApplication
    {
        public String Name { get; }

        public String FullName { get; }

        public String DisplayName { get; }

        public String Description { get; }

        public BitmapImage Logo { get; }

        public String PackageFullName { get; }

        internal MetroApplication(String name, String fullName, String displayName, String description, BitmapImage logo, String packageFullName)
        {
            this.Name = name;
            this.FullName = fullName;
            this.DisplayName = displayName;
            this.Description = description;
            this.Logo = logo;
            this.PackageFullName = packageFullName;
        }

        public Boolean WriteLogoToPngFile(String filePath)
        {
            try
            {
                BitmapEncoder encoder = new PngBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(this.Logo));

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    encoder.Save(fileStream);
                }

                return true;
            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.Message);
                return false;
            }
        }

        public Boolean Launch() => MetroPackageManager.Launch(this.FullName);
    }
}
