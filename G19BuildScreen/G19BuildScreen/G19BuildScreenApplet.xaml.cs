// // --------------------------------------------------------------------------------------------------------------------
// // <copyright file="G19BuildScreenApplet.xaml.cs" company="BaerDev">
// // Copyright (c) BaerDev. All rights reserved.
// // </copyright>
// // <summary>
// // The file 'G19BuildScreenApplet.xaml.cs'.
// // </summary>
// // --------------------------------------------------------------------------------------------------------------------

namespace G19BuildScreen
{
    using System;
    using System.Configuration;
    using System.Linq;
    using System.Net;
    using System.Windows.Controls;
    using System.Windows.Media;
    using System.Windows.Threading;

    using G19BuildScreen.Authorizers;
    using G19BuildScreen.BuildInformationGetter;

    using Microsoft.TeamFoundation.Build.Client;
    using Microsoft.TeamFoundation.Client;
    using Microsoft.TeamFoundation.TestManagement.Client;

    /// <summary>
    ///     Interaction logic for G19BuildScreenApplet.xaml
    /// </summary>
    public partial class G19BuildScreenApplet : UserControl
    {
        public G19BuildScreenApplet()
        {
            this.InitializeComponent();
        }
    }
}