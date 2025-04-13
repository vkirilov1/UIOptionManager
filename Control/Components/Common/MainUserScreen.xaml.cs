using Control.ViewModel.Common;
using Service.Model.Users;
using System.Windows;

namespace Control.Components.Common
{
    public partial class MainUserScreen : Window
    {
        private readonly User _loggedInUser;

        public MainUserScreen(User user)
        {
            InitializeComponent();
            _loggedInUser = user;
            DataContext = new MainUserViewModel(_loggedInUser);
        }
    }
}
