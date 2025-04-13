using Control.ViewModel.Users;
using System.Windows;
using System.Windows.Controls;

namespace Control.Components.Users
{
    public partial class LoginPage : Window
    {
        private readonly LoginViewModel _viewModel;

        public LoginPage()
        {
            InitializeComponent();
            _viewModel = new LoginViewModel();
            DataContext = _viewModel;
        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (sender is PasswordBox box)
                _viewModel.Password = box.Password;
        }
    }
}
