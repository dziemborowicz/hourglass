using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualBasic.ApplicationServices;

namespace Hourglass
{
    public class AppEntry : WindowsFormsApplicationBase
    {
        [STAThread]
        public static void Main(string[] args)
        {
            AppEntry appEntry = new AppEntry();
            appEntry.Run(args);
        }

        private App app;

        public AppEntry()
        {
            IsSingleInstance = true;
        }

        protected override bool OnStartup(StartupEventArgs eventArgs)
        {
            app = new App();
            app.Run(new TimerWindow(eventArgs.CommandLine.ToArray()));
            return false;
        }

        protected override void OnStartupNextInstance(StartupNextInstanceEventArgs eventArgs)
        {
            TimerWindow timerWindow = new TimerWindow(eventArgs.CommandLine.ToArray());
            timerWindow.Show();
        }
    }
}
