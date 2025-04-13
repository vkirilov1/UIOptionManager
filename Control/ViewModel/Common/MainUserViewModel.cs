using Control.Components.Common;
using Control.Components.Users;
using Control.Others.Commands;
using Control.Others.Constants;
using Control.ViewModel.OptionList;
using Service.Others.Identifiers.Constants;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using Service.Model.Users;

namespace Control.ViewModel.Common
{
    public class MainUserViewModel : INotifyPropertyChanged
    {
        public string Greeting => $"Hi, {User.Name}";

        public ObservableCollection<BaseOptionListViewModel> OptionLists { get; set; }

        public ICommand LogoutCommand { get; }

        private User User { get; }

        public MainUserViewModel(User user)
        {
            User = user;
            OptionLists = [];

            LoadOptionLists(user.Id);
            LogoutCommand = new DelegateCommandNoParam(Logout);
        }

        private void LoadOptionLists(int userId)
        {
            OptionLists.Clear();

            OptionLists.Add(new SystemOptionListViewModel<SystemIdConstants.EmploymentType>(OptionListIdentifier.EmploymentType, userId));
            OptionLists.Add(new UserOptionsListViewModel(OptionListIdentifier.Roles, userId, "User can enter and choose different roles"));
            OptionLists.Add(new UserOptionsListViewModel(OptionListIdentifier.Hobbies, userId));
            OptionLists.Add(new MixedOptionListViewModel<SystemIdConstants.WorkLocation>(OptionListIdentifier.WorkLocations, userId));
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
