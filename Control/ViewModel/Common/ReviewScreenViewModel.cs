using Control.Components.Common;
using Control.Components.Users;
using Control.Others.Commands;
using Control.ViewModel.JobApplicationVM;
using Service.Model.Users;
using Service.Service;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace Control.ViewModel.Common
{
    class ReviewScreenViewModel
    {
        public string Greeting => $"Hi, {User.Name}! Please review the Applications below!";

        public ObservableCollection<JobApplicationReviewViewModel> AllApplications {  get; set; }

        private User User { get; }

        public ICommand LogoutCommand { get; }

        private JobApplicationService _service = new();

        public ReviewScreenViewModel(User user)
        {
            User = user;
            AllApplications = [];
            LoadApplications();
            LogoutCommand = new DelegateCommandNoParam(Logout);
        }

        private void Logout()
        {
            var login = new LoginPage();
            login.Show();
            Application.Current.Windows.OfType<ReviewScreen>().FirstOrDefault()?.Close();
        }

        private void LoadApplications()
        {
            AllApplications.Clear();
            var applications = _service.GetAllPendingApplications();

            foreach (var app in applications)
            {
                var vm = new JobApplicationReviewViewModel(app);
                vm.ReviewCompleted += (finishedApp) => AllApplications.Remove(finishedApp);
                AllApplications.Add(vm);
            }
        }
    }
}
