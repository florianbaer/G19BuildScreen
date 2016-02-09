namespace G19BuildScreen
{
    using System;
    using System.Windows.Media;

    using GalaSoft.MvvmLight;

    public class G19BuildScreenAppletViewModel : ViewModelBase
    {
        private readonly SolidColorBrush statusColorBrush;

        private SolidColorBrush backgroundColor;

        private string buildDefinitionName;

        private string requestedBy;

        private string status;

        private Color statusColor;

        private string timeRequested;

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

                return this.buildDefinitionName;
            }

            set
            {
                this.buildDefinitionName = value;
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
                return this.teamProjectName;
            }
            set
            {
                this.teamProjectName = value;
            }
        }

        public string RequestedBy
        {
            get
            {
                if (this.IsInDesignMode)
                {
                    return "Requested by: binaryfr3ak";
                }

                return $"Requested by: {this.requestedBy}";
            }

            set
            {
                this.requestedBy = value;
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

                return this.status;
            }

            set
            {
                this.status = value;
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

                return this.statusColor;
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

                return this.statusColorBrush;
            }
        }

        public string TimeRequested
        {
            get
            {
                if (this.IsInDesignMode)
                {
                    return $"Requested: {DateTime.Now.ToLocalTime()}";
                }

                return this.timeRequested;
            }

            set
            {
                this.timeRequested = value;
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
                return this.passedTests;
            }
            set
            {
                this.passedTests = value;
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
                return this.failedTests;
            }
            set
            {
                this.failedTests = value;
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
                return this.errorTests;
            }
            set
            {
                this.errorTests = value;
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
                return this.inconclusiveTests;
            }
            set
            {
                this.inconclusiveTests = value;
            }
        }
    }
}