using Control.Others.Commands;
using Service.Model.JobApplication;
using Service.Model.Users;
using Service.Service;
using System.Windows.Input;

namespace Control.ViewModel.JobApplicationVM
{
    public class JobApplicationReviewViewModel
    {
        private JobApplicationService _service = new();
        public JobApplication Application { get; }

        public string Position => Application.Position;
        public string ApplicantName => Application.User.Name;
        public string CoverLetter => Application.CoverLetter;
        public string EmploymentType => Application.EmploymentType;
        public string WorkLocation => Application.City;

        public ICommand AcceptCommand { get; }
        public ICommand RejectCommand { get; }

        public event Action<JobApplicationReviewViewModel>? ReviewCompleted;

        public JobApplicationReviewViewModel(JobApplication application)
        {
            Application = application;

            AcceptCommand = new DelegateCommandNoParam(() => UpdateStatus(nameof(JobApplicationStatusEnum.Accepted)));
            RejectCommand = new DelegateCommandNoParam(() => UpdateStatus(nameof(JobApplicationStatusEnum.Rejected)));
        }

        private void UpdateStatus(string newStatus)
        {
            try
            {
                _service.UpdateApplicationStatus(Application.Id, Enum.Parse<JobApplicationStatusEnum>(newStatus));
                ReviewCompleted?.Invoke(this);
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"Failed to update status: {ex.Message}");
            }
        }
    }
}