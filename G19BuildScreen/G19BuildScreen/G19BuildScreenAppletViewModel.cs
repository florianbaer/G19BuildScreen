namespace G19BuildScreen
{
    using System.Windows.Media;

    using GalaSoft.MvvmLight;

    public class G19BuildScreenAppletViewModel : ViewModelBase
    {
        private readonly SolidColorBrush statusColorBrush;

        private SolidColorBrush backgroundColor;
        private Color statusColor;

        private string buildDefinitionName;

        private string status;

        private string requestedBy;

        public string RequestedBy
        {
            get
            {
                if (this.IsInDesignMode)
                {
                    return "Requested by: baerf";
                }
                return $"Requested by: {this.requestedBy}";
            }
            set
            {
                this.requestedBy = value;
            }
        }

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
    }
}