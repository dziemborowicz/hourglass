// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UpdateManager.cs" company="Chris Dziemborowicz">
//   Copyright (c) Chris Dziemborowicz. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Hourglass.Managers
{
    using System;
    using System.ComponentModel;
    using System.IO;
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
        private const string UpdateCheckUrl = "https://updates.dziemborowicz.com/hourglass";

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
            ServicePointManager.Expect100Continue = true;
            try
            {
                // Try to use TLS 1.3 if it's supported.
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | /* Tls13 */ (SecurityProtocolType)12288;
            }
            catch (NotSupportedException)
            {
                // Otherwise, fall back to using TLS 1.2.
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            }

            Task.Factory.StartNew(() => this.SetUpdateInfo(this.FetchUpdateInfo()));
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
        /// <returns>An <see cref="UpdateInfo"/>.</returns>
        private UpdateInfo FetchUpdateInfo()
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(UpdateCheckUrl);
                request.UserAgent = string.Format(
                    "Mozilla/5.0 ({0}) {1}/{2} (UUID: {3})",
                    Environment.OSVersion.VersionString,
                    this.AppName,
                    this.CurrentVersion,
                    this.UniqueId);

                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    Stream responseStream = response.GetResponseStream();
                    if (responseStream != null)
                    {
                        XmlSerializer serializer = new XmlSerializer(typeof(UpdateInfo));
                        return (UpdateInfo)serializer.Deserialize(responseStream);
                    }
                    else
                    {
                        return null;
                    }
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
            }
            catch
            {
                return false;
            }

            this.OnPropertyChanged("HasUpdates", "LatestVersion", "UpdateUri");
            return true;
        }
    }
}
