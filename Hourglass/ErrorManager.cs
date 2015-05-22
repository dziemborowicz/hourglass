// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ErrorManager.cs" company="Chris Dziemborowicz">
//   Copyright (c) Chris Dziemborowicz. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Hourglass
{
    using System;
    using System.IO;
    using System.Reflection;
    using System.Windows;

    /// <summary>
    /// Manages global errors.
    /// </summary>
    public class ErrorManager : Manager
    {
        /// <summary>
        /// Singleton instance of the <see cref="ErrorManager"/> class.
        /// </summary>
        public static readonly ErrorManager Instance = new ErrorManager();

        /// <summary>
        /// The maximum number of error dump files to write to the temporary folder.
        /// </summary>
        private const int MaxDumpFiles = 100;

        /// <summary>
        /// Prevents a default instance of the <see cref="ErrorManager"/> class from being created.
        /// </summary>
        private ErrorManager()
        {
        }

        /// <summary>
        /// Initializes the class.
        /// </summary>
        public override void Initialize()
        {
            AppDomain.CurrentDomain.UnhandledException += AppDomainUnhandledException;
        }

        /// <summary>
        /// Invoked when an exception is not caught.
        /// </summary>
        /// <param name="sender">The <see cref="AppDomain"/>.</param>
        /// <param name="e">The event data.</param>
        private static void AppDomainUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            try
            {
                string errorMessage = e.ExceptionObject.ToString();

                string dumpFilePath;
                if (TryDumpErrorMessage(errorMessage, out dumpFilePath))
                {
                    errorMessage += Environment.NewLine;
                    errorMessage += Environment.NewLine;
                    errorMessage += string.Format("The error has been written to \"{0}\".", dumpFilePath);
                }

                ErrorWindow errorWindow = new ErrorWindow();
                errorWindow.ShowDialog("An unexpected error has occurred.", errorMessage);
            }
            finally
            {
                Application.Current.Shutdown(1);
            }
        }

        /// <summary>
        /// Tries to write the error message to a file in the temporary files.
        /// </summary>
        /// <param name="errorMessage">An error message.</param>
        /// <param name="dumpFilePath">The path of the dump file that was written.</param>
        /// <returns><c>true</c> if the error message is successfully written, or <c>false</c> otherwise.</returns>
        private static bool TryDumpErrorMessage(string errorMessage, out string dumpFilePath)
        {
            try
            {
                // Find an empty dump file slot in the circular buffer
                int i = 0;
                while (File.Exists(GetDumpFilePath(i)) && i < MaxDumpFiles)
                {
                    i++;
                }

                // If no empty slot found, default to the first slot
                if (i == MaxDumpFiles)
                {
                    i = 0;
                }

                // Prepend the date and time to the error message
                errorMessage = string.Format("[{0:O}] {1}", DateTime.Now, errorMessage);

                // Write the dump file
                dumpFilePath = GetDumpFilePath(i);
                File.WriteAllText(dumpFilePath, errorMessage);

                // Delete the next dump file for next time
                File.Delete(GetDumpFilePath((i + 1) % MaxDumpFiles));

                return true;
            }
            catch
            {
                dumpFilePath = null;
                return false;
            }
        }

        /// <summary>
        /// Returns the path for the dump file with the specified suffix.
        /// </summary>
        /// <param name="i">A suffix for the dump file name.</param>
        /// <returns>The path for the dump file with the specified suffix.</returns>
        private static string GetDumpFilePath(int i)
        {
            string appName = Assembly.GetExecutingAssembly().GetName().Name;
            string directory = Path.GetTempPath();
            string filename = string.Format("{0}-Crash.{1}.txt", appName, i);
            return Path.Combine(directory, filename);
        }
    }
}
