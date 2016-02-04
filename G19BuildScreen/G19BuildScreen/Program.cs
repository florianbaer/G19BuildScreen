using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Threading;
using GammaJul.LgLcd;
using GammaJul.LgLcd.Wpf;

namespace G19BuildScreen
{
    internal class App : Application
    {
        private LcdApplet applet;
        private G19BuildScreenApplet buildScreenApplet;
        private LcdDeviceQvga lcdDevice;
        private DispatcherTimer timer;

        /// <summary>
        ///     On startup, we are creation a new Applet.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            applet = new LcdApplet("G19 Build Viewer", LcdAppletCapabilities.Qvga);

            // Register to events to know when a device arrives, then connects the applet to the LCD Manager
            applet.DeviceArrival += Applet_DeviceArrival;
            applet.Connect();
        }

        /// <summary>
        ///     Simple utility method to always executes a method on the Application's thread.
        /// </summary>
        /// <param name="action">Method to execute.</param>
        private void Invoke(Action action)
        {
            if (CheckAccess())
                action();
            else
                Dispatcher.BeginInvoke(action, DispatcherPriority.Render);
        }

        /// <summary>
        ///     This event handler will be called whenever a new device of a given type arrives in the system.
        ///     This is where you should opens the device you want to shows the applet on.
        ///     Take special care for thread-safety as the SDK calls this handler in another thread.
        /// </summary>
        private void Applet_DeviceArrival(object sender, LcdDeviceTypeEventArgs e)
        {
            // since with specified LcdAppletCapabilities.Qvga at the Applet's creation,
            // we will only receive QVGA arrival notifications.
            Debug.Assert(e.DeviceType == LcdDeviceType.Qvga);
            Invoke(OnQvgaDeviceArrived);
        }

        private void OnQvgaDeviceArrived()
        {
            // First device arrival, creates the device
            if (lcdDevice == null)
            {
                lcdDevice = (LcdDeviceQvga) applet.OpenDeviceByType(LcdDeviceType.Qvga);
                buildScreenApplet = new G19BuildScreenApplet();
                lcdDevice.CurrentPage = new LcdWpfPage(lcdDevice)
                {
                    Element = buildScreenApplet
                };
                lcdDevice.SoftButtonsChanged += LcdDeviceSoftButtonsChanged;

                // Starts a timer to update the screen
                timer = new DispatcherTimer(TimeSpan.FromMilliseconds(5.0), DispatcherPriority.Render, Timer_Tick,
                    Dispatcher.CurrentDispatcher);
            }

            // Subsequent device arrival means the device was removed and replugged, simply reopens it
            else
                lcdDevice.ReOpen();
            lcdDevice.DoUpdateAndDraw();
        }

        /// <summary>
        ///     Updates the LCD screen.
        /// </summary>
        private void Timer_Tick(object sender, EventArgs e)
        {
            if (applet.IsEnabled && lcdDevice != null && !lcdDevice.IsDisposed)
                lcdDevice.DoUpdateAndDraw();
        }

        /// <summary>
        ///     When soft buttons are pressed, switch to previous image if left arrow button was clicked,
        ///     switch to next if the right arrow button was clicked, or closes the application if
        ///     the cancel button was clicked.
        /// </summary>
        private void LcdDeviceSoftButtonsChanged(object sender, LcdSoftButtonsEventArgs e)
        {
            if ((e.SoftButtons & LcdSoftButtons.Cancel) == LcdSoftButtons.Cancel)
                Invoke(Shutdown);
            else if ((e.SoftButtons & LcdSoftButtons.Left) == LcdSoftButtons.Left)
            {
            }
            //// Invoke(buildScreenApplet.PreviousImage);
            else if ((e.SoftButtons & LcdSoftButtons.Right) == LcdSoftButtons.Right)
            {
            }
            //// Invoke(buildScreenApplet.NextImage);
        }

        [STAThread]
        internal static void Main()
        {
            var app = new App();
            app.Run();
        }

        private delegate void Action();
    }
}