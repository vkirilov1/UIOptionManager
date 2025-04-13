using Control.ViewModel.Users;
using System.Windows;
using System.Windows.Controls;

namespace Control.Components.Users
{
    public partial class RegisterPage : Window
    {
        private RegisterViewModel _viewModel;

        public RegisterPage()
        {
            InitializeComponent();
            _viewModel = new RegisterViewModel();
            DataContext = _viewModel;
        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (sender is PasswordBox passwordBox)
            {
                _viewModel.Password = passwordBox.Password;
            }
        }
    }
}
