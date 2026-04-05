using System.Windows;

namespace Control.Components.JobApplication
{
    public partial class JobApplicationHistoryWindow : Window
    {
        public JobApplicationHistoryWindow()
        {
            InitializeComponent();
        }

        private void CloseClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}