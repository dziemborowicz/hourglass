using System;
using System.Linq;
using Microsoft.VisualBasic.ApplicationServices;

namespace Hourglass
{
    public class AppEntry : WindowsFormsApplicationBase
    {
        [STAThread]
        public static void Main(string[] args)
        {
            var appEntry = new AppEntry();
            appEntry.Run(args);
        }

        private App _app;

        public AppEntry()
        {
            IsSingleInstance = true;
        }

        protected override bool OnStartup(StartupEventArgs eventArgs)
        {
            _app = new App();
            _app.Run(new TimerWindow(eventArgs.CommandLine.ToArray()));
            return false;
        }

        protected override void OnStartupNextInstance(StartupNextInstanceEventArgs eventArgs)
        {
            var timerWindow = new TimerWindow(eventArgs.CommandLine.ToArray());
            timerWindow.Show();
        }
    }
}
