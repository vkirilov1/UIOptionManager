using Control.Components.Common;
using Control.Components.JobApplication;
using Control.Components.Users;
using Control.Others.Commands;
using Control.ViewModel.JobApplicationVM;
using Service.Model.Users;
using Service.Service;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;

namespace Control.ViewModel.Common
{
    public class MainUserViewModel : INotifyPropertyChanged
    {
        public string Greeting => $"Hi, {User.Name}";

        public JobApplicationViewModel JobApplication { get; set; }

        private User User { get; }

        public ICommand ViewHistoryCommand { get; }

        public ICommand LogoutCommand { get; }

        private JobApplicationService _service = new();

        public MainUserViewModel(User user)
        {
            User = user;

            JobApplication = new JobApplicationViewModel(user);
            ViewHistoryCommand = new DelegateCommandNoParam(OpenHistory);
            LogoutCommand = new DelegateCommandNoParam(Logout);
        }

        private void OpenHistory()
        {
            var history = _service.GetUserApplicationHistory(User.Id);
            var historyWindow = new JobApplicationHistoryWindow
            {
                DataContext = new { History = history },
                Owner = Application.Current.MainWindow
            };
            historyWindow.ShowDialog();
        }

        private void Logout()
        {
            var login = new LoginPage();
            login.Show();
            Application.Current.Windows.OfType<MainUserScreen>().FirstOrDefault()?.Close();
        }

        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
