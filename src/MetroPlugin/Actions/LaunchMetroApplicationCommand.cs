namespace NotADoctor99.MetroPlugin
{
    using System;
    using System.IO;

    using Loupedeck;
    using NotADoctor99.MetroPackageManager;

    public class LaunchMetroApplicationCommand : PluginDynamicCommand
    {
        private readonly MetroPackageManager _metroPackageManager = new MetroPackageManager();
        private TemporaryDirectory _temporaryDirectory;

        public LaunchMetroApplicationCommand()
        {
            this.DisplayName = "Launch Microsoft Store Application";
            this.Description = "Launches Microsoft Store Applications";
            this.GroupName = "Launch Microsoft Store Application";

            this._metroPackageManager.ReadPackagesForCurrentUser();

            foreach (var application in this._metroPackageManager.Applications)
            {
                this.AddParameter(application.FullName, application.DisplayName, this.GroupName).Description = $"Launches {application.DisplayName}";
            }
        }

        protected override Boolean OnLoad()
        {
            this._temporaryDirectory = new TemporaryDirectory();

            foreach (var application in this._metroPackageManager.Applications)
            {
                this.AddParameter(application.FullName, application.DisplayName, this.GroupName).Description = $"Launches {application.DisplayName}";
                application.WriteLogoToPngFile(this.GetIconFilePath(application.FullName));
            }

            return true;
        }

        protected override Boolean OnUnload()
        {
            this._temporaryDirectory.Delete();
            return true;
        }

        protected override void RunCommand(String actionParameter) => MetroPackageManager.Launch(actionParameter);

        protected override BitmapImage GetCommandImage(String actionParameter, PluginImageSize imageSize)
        {
            var applicationIconFilePath = this.GetIconFilePath(actionParameter);

            return File.Exists(applicationIconFilePath) ? BitmapImage.FromFile(applicationIconFilePath) : null;
        }

        private String GetIconFilePath(String applicationFullName) => this._temporaryDirectory.MakeFilePath($"{applicationFullName}.png");
    }
}
