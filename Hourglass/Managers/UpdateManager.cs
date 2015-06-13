// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UpdateManager.cs" company="Chris Dziemborowicz">
//   Copyright (c) Chris Dziemborowicz. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Hourglass.Managers
{
    using System;
    using System.ComponentModel;
    using System.Net;
    using System.Reflection;
    using System.Threading.Tasks;
    using System.Xml.Serialization;

    using Hourglass.Properties;
    using Hourglass.Serialization;

    /// <summary>
    /// Manages app updates.
    /// </summary>
    public class UpdateManager : Manager, INotifyPropertyChanged
    {
        /// <summary>
        /// Singleton instance of the <see cref="UpdateManager"/> class.
        /// </summary>
        public static readonly UpdateManager Instance = new UpdateManager();

        /// <summary>
        /// The URL for the XML file containing information about the latest version of the app.
        /// </summary>
        private const string UpdateCheckUrl = "http://update.dziemborowicz.com/hourglass";

        /// <summary>
        /// The latest version of the app.
        /// </summary>
        private Version latestVersion;

        /// <summary>
        /// The URI to download the update to the latest version of the app.
        /// </summary>
        private Uri updateUri;

        /// <summary>
        /// Prevents a default instance of the <see cref="UpdateManager"/> class from being created.
        /// </summary>
        private UpdateManager()
        {
        }
        
        /// <summary>
        /// Raised when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Gets the name of the app.
        /// </summary>
        public string AppName
        {
            get { return Assembly.GetExecutingAssembly().GetName().Name; }
        }

        /// <summary>
        /// Gets the current version of the app.
        /// </summary>
        public Version CurrentVersion
        {
            get { return Assembly.GetExecutingAssembly().GetName().Version; }
        }

        /// <summary>
        /// Gets a value indicating whether a newer version of the app is available.
        /// </summary>
        public bool HasUpdates
        {
            get { return this.LatestVersion != null && this.LatestVersion > this.CurrentVersion; }
        }

        /// <summary>
        /// Gets the latest version of the app.
        /// </summary>
        public Version LatestVersion
        {
            get { return this.latestVersion; }
        }

        /// <summary>
        /// Gets the unique identifier of this instance of the app.
        /// </summary>
        public Guid UniqueId
        {
            get
            {
                if (Settings.Default.UniqueId == Guid.Empty)
                {
                    Settings.Default.UniqueId = Guid.NewGuid();
                }

                return Settings.Default.UniqueId;
            }
        }

        /// <summary>
        /// Gets the URI to download the update to the latest version of the app.
        /// </summary>
        public Uri UpdateUri
        {
            get { return this.updateUri; }
        }

        /// <summary>
        /// Initializes the class.
        /// </summary>
        public override void Initialize()
        {
            this.GetUpdateInfoAsync()
                .ContinueWith(task => this.SetUpdateInfo(task.Result));
        }

        /// <summary>
        /// Raises the <see cref="PropertyChanged"/> event.
        /// </summary>
        /// <param name="propertyNames">One or more property names.</param>
        protected void OnPropertyChanged(params string[] propertyNames)
        {
            PropertyChangedEventHandler eventHandler = this.PropertyChanged;

            if (eventHandler != null)
            {
                foreach (string propertyName in propertyNames)
                {
                    eventHandler(this, new PropertyChangedEventArgs(propertyName));
                }
            }
        }

        /// <summary>
        /// Fetches the latest <see cref="UpdateInfo"/> from the <see cref="UpdateCheckUrl"/>.
        /// </summary>
        /// <returns>The task object representing the asynchronous operation.</returns>
        private async Task<UpdateInfo> GetUpdateInfoAsync()
        {
            try
            {
                HttpWebRequest request = WebRequest.CreateHttp(UpdateCheckUrl);
                request.UserAgent = string.Format(
                    "Mozilla/5.0 ({0}) {1}/{2} (UUID: {3})",
                    Environment.OSVersion.VersionString,
                    this.AppName,
                    this.CurrentVersion,
                    this.UniqueId);

                using (HttpWebResponse response = (HttpWebResponse)await request.GetResponseAsync())
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(UpdateInfo));
                    return (UpdateInfo)serializer.Deserialize(response.GetResponseStream());
                }
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Sets the properties of this manager from an <see cref="UpdateInfo"/>.
        /// </summary>
        /// <param name="updateInfo">An <see cref="UpdateInfo"/>.</param>
        /// <returns><c>true</c> if the properties were set successfully, or <c>false</c> otherwise.</returns>
        private bool SetUpdateInfo(UpdateInfo updateInfo)
        {
            try
            {
                this.latestVersion = new Version(updateInfo.LatestVersion);
                this.updateUri = new Uri(updateInfo.UpdateUrl);

                this.OnPropertyChanged("LatestVersion", "UpdateUri");
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
