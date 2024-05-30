namespace NotADoctor99.MetroPackageManager
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Windows.Media.Imaging;

    public class MetroApplicationNoAppInfo
    {
        public String Id { get; }

        public String DisplayName { get; }

        public String Description { get; }

        public BitmapImage Logo { get; }

        internal MetroApplicationNoAppInfo(String appUserModelId, String displayName, String description, BitmapImage logo)
        {
            this.Id = appUserModelId;
            this.DisplayName = displayName;
            this.Description = description;
            this.Logo = logo;
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

        public Boolean Launch() => MetroPackageManager.Launch(this.Id);
    }
}
