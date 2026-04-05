using Control.ViewModel.JobApplicationVM;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace Control.Components.JobApplication
{
    public partial class JobApplicationsListPanel : UserControl
    {
        public static readonly DependencyProperty ApplicationsProperty =
            DependencyProperty.Register(
                nameof(Applications),
                typeof(ObservableCollection<JobApplicationReviewViewModel>),
                typeof(JobApplicationsListPanel),
                new PropertyMetadata(null));

        public ObservableCollection<JobApplicationReviewViewModel> Applications
        {
            get { return (ObservableCollection<JobApplicationReviewViewModel>)GetValue(ApplicationsProperty); }
            set { SetValue(ApplicationsProperty, value); }
        }

        public JobApplicationsListPanel()
        {
            InitializeComponent();
        }
    }
}