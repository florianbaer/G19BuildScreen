namespace G19BuildScreen
{
    using System;
    using System.Globalization;
    using System.Windows.Media;

    using GalaSoft.MvvmLight;

    public class G19BuildScreenAppletViewModel : ViewModelBase
    {
        public G19BuildScreenAppletViewModel(G19BuildScreenAppletModel model)
        {
            this.model = model;
        }

        public G19BuildScreenAppletViewModel()
        {
        }

        private G19BuildScreenAppletModel model;
        

        private readonly SolidColorBrush statusColorBrush;

        private SolidColorBrush backgroundColor;

        private string buildDefinitionName;

        private string requestedBy;

        private string status;

        private Color statusColor;

        private DateTime timeRequested;

        private string passedTests;

        private string failedTests;

        private string errorTests;

        private string inconclusiveTests;

        private string teamProjectName;

        public SolidColorBrush BackgroundColor
        {
            get
            {
                if (this.IsInDesignMode)
                {
                    return new SolidColorBrush(Colors.Gray);
                }

                return this.backgroundColor;
            }

            set
            {
                this.backgroundColor = value;
            }
        }

        public string BuildDefinitionName
        {
            get
            {
                if (this.IsInDesignMode)
                {
                    return "BuildDefintionName";
                }

                return this.model.BuildDefinitionName;
            }
        }

        public string TeamProjectName
        {
            get
            {
                if (this.IsInDesignMode)
                {
                    return "G19BuildScreen";
                }
                return this.model.TeamProjectName;
            }
        }

        public string RequestedBy
        {
            get
            {
                if (this.IsInDesignMode)
                {
                    return "binaryfr3ak";
                }

                return this.model.RequestedBy;
            }
        }

        public string Status
        {
            get
            {
                if (this.IsInDesignMode)
                {
                    return "Successful";
                }

                return this.model.Status;
            }
        }

        public Color StatusColor
        {
            get
            {
                if (this.IsInDesignMode)
                {
                    return Colors.Green;
                }

                return StringStatusColorParser.GetColorForStatus(this.Status);
            }
        }

        public SolidColorBrush StatusColorBrush
        {
            get
            {
                if (this.IsInDesignMode)
                {
                    return new SolidColorBrush(Colors.Green);
                }

                return new SolidColorBrush(this.StatusColor);
            }
        }

        public string TimeRequested
        {
            get
            {
                if (this.IsInDesignMode)
                {
                    return $"{DateTime.Now.ToLocalTime().ToString("HH:mm:ss - dd-MMM-yyyy")}";
                }

                return this.model.TimeRequested.ToString("HH:mm:ss - dd-MMM-yyyy");
            }
        }

        public string TotalTests
        {
            get
            {
                if (this.IsInDesignMode)
                {
                    return $"from 126";
                }
                return $"from {this.model.TestResults.Total}";
            }
        }

        public string PassedTests
        {
            get
            {
                if (this.IsInDesignMode)
                {
                    return $"Passed: 126";
                }
                return $"{this.model.TestResults.Passed}";
            }
        }

        public string FailedTests
        {
            get
            {
                if (this.IsInDesignMode)
                {
                    return $"Failed: 3";
                }
                return $"Failed: {this.model.TestResults.Failed}";
            }
        }

        public string ErrorTests
        {
            get
            {
                if (this.IsInDesignMode)
                {
                    return $"Error: 0";
                }
                return $"Error: {this.model.TestResults.Error}";
            }
        }

        public string InconclusiveTests
        {
            get
            {

                if (this.IsInDesignMode)
                {
                    return $"Inconclusive: 2";
                }
                return $"Inconclusive: {this.model.TestResults.Inconclusive}";
            }
        }
    }
}