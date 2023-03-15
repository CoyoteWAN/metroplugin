namespace NotADoctor99.MetroPackageManager
{
    using System;
    using System.Collections.Generic;

    public class MetroPackage
    {
        private readonly List<MetroApplication> _applications = new List<MetroApplication>();

        public String Name { get; }

        public String FullName { get; }

        public String DisplayName { get; }

        public String Description { get; }

        public Version Version { get; }

        public String FamilyName { get; }

        public String PublisherDisplayName { get; }

        public String InstallationPath { get; }

        public DateTimeOffset InstallationDate { get; }

        public String LogoFilePath { get; }

        public IReadOnlyCollection<MetroApplication> Applications => this._applications;

        internal MetroPackage(String name, String fullName, String displayName, String description, Version version, String familyName, String publisherDisplayName, String installationPath, DateTimeOffset installationDate, String logoFilePath)
        {
            this.Name = name;
            this.FullName = fullName;
            this.DisplayName = displayName;
            this.Description = description;
            this.Version = version;
            this.FamilyName = familyName;
            this.PublisherDisplayName = publisherDisplayName;
            this.InstallationPath = installationPath;
            this.InstallationDate = installationDate;
            this.LogoFilePath = logoFilePath;
        }

        internal void AddApplication(MetroApplication application) => this._applications.Add(application);
    }
}
