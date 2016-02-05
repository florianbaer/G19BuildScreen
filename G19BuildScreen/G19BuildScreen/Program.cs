// // --------------------------------------------------------------------------------------------------------------------
// // <copyright file="Program.cs" company="BaerDev">
// // Copyright (c) BaerDev. All rights reserved.
// // </copyright>
// // <summary>
// // The file 'Program.cs'.
// // </summary>
// // --------------------------------------------------------------------------------------------------------------------
namespace G19BuildScreen
{
    using System;
    using System.Diagnostics;
    using System.Windows;
    using System.Windows.Threading;

    using GammaJul.LgLcd;
    using GammaJul.LgLcd.Wpf;

    internal class App : Application
    {
        private LcdApplet applet;

        private G19BuildScreenApplet buildScreenApplet;

        private LcdDeviceQvga lcdDevice;

        private DispatcherTimer timer;

        private delegate void Action();

        [STAThread]
        internal static void Main()
        {
            var app = new App();
            app.Run();
        }

        /// <summary>
        ///     On startup, we are creation a new Applet.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            this.applet = new LcdApplet("G19 Build Viewer", LcdAppletCapabilities.Qvga);

            // Register to events to know when a device arrives, then connects the applet to the LCD Manager
            this.applet.DeviceArrival += this.Applet_DeviceArrival;
            this.applet.Connect();
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
            this.Invoke(this.OnQvgaDeviceArrived);
        }

        /// <summary>
        ///     Simple utility method to always executes a method on the Application's thread.
        /// </summary>
        /// <param name="action">Method to execute.</param>
        private void Invoke(Action action)
        {
            if (this.CheckAccess())
            {
                action();
            }
            else
            {
                this.Dispatcher.BeginInvoke(action, DispatcherPriority.Render);
            }
        }

        /// <summary>
        ///     When soft buttons are pressed, switch to previous image if left arrow button was clicked,
        ///     switch to next if the right arrow button was clicked, or closes the application if
        ///     the cancel button was clicked.
        /// </summary>
        private void LcdDeviceSoftButtonsChanged(object sender, LcdSoftButtonsEventArgs e)
        {
            if ((e.SoftButtons & LcdSoftButtons.Cancel) == LcdSoftButtons.Cancel)
            {
                this.Invoke(this.Shutdown);
            }
            else if ((e.SoftButtons & LcdSoftButtons.Left) == LcdSoftButtons.Left)
            {
            }
            
            //// Invoke(buildScreenApplet.PreviousImage);
            else if ((e.SoftButtons & LcdSoftButtons.Right) == LcdSoftButtons.Right)
            {
            }

            //// Invoke(buildScreenApplet.NextImage);
        }

        private void OnQvgaDeviceArrived()
        {
            // First device arrival, creates the device
            if (this.lcdDevice == null)
            {
                this.lcdDevice = (LcdDeviceQvga)this.applet.OpenDeviceByType(LcdDeviceType.Qvga);
                this.buildScreenApplet = new G19BuildScreenApplet();
                this.lcdDevice.CurrentPage = new LcdWpfPage(this.lcdDevice) { Element = this.buildScreenApplet };
                this.lcdDevice.SoftButtonsChanged += this.LcdDeviceSoftButtonsChanged;

                // Starts a timer to update the screen
                this.timer = new DispatcherTimer(
                    TimeSpan.FromSeconds(5.0), 
                    DispatcherPriority.Render, 
                    this.UpdateApplet, 
                    Dispatcher.CurrentDispatcher);
            }

            // Subsequent device arrival means the device was removed and replugged, simply reopens it
            else
            {
                this.lcdDevice.ReOpen();
            }

            this.lcdDevice.DoUpdateAndDraw();
        }

        /// <summary>
        ///     Updates the LCD screen.
        /// </summary>
        private void UpdateApplet(object sender, EventArgs e)
        {
            if (this.applet.IsEnabled && this.lcdDevice != null && !this.lcdDevice.IsDisposed)
            {
                this.lcdDevice.DoUpdateAndDraw();
                this.buildScreenApplet.UpdateBuildInformation();
            }
        }
    }
}