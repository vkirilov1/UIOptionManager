using Control.ViewModel.Common;
using Service.Model.Users;
using System.Windows;

namespace Control.Components.Common
{
    public partial class ReviewScreen : Window
    {
        private readonly User _loggedInUser;

        public ReviewScreen(User user)
        {
            InitializeComponent();
            _loggedInUser = user;
            DataContext = new ReviewScreenViewModel(_loggedInUser);
        }
    }
}
