using Control.Components.Common;
using Control.Components.Users;
using Control.Others.Commands;
using Service.Model.Users;
using Service.Others.OptionListLoggerDelegates;
using System.Windows;
using System.Windows.Input;

namespace Control.ViewModel.Users
{
    public class LoginViewModel : DependencyObject
    {
        public static readonly DependencyProperty UsernameProperty =
            DependencyProperty.Register(
                nameof(Username),
                typeof(string),
                typeof(LoginViewModel),
                new PropertyMetadata(string.Empty));

        public string Username
        {
            get => (string)GetValue(UsernameProperty);
            set => SetValue(UsernameProperty, value);
        }

        public string Password { get; set; } = "";

        public ICommand LoginCommand { get; }
        public ICommand RegisterCommand { get; }

        public LoginViewModel()
        {
            LoginCommand = new DelegateCommandNoParam(Login);
            RegisterCommand = new DelegateCommandNoParam(OpenRegisterWindow);
        }

        private void Login()
        {
            try
            {
                var user = UserFactory.LoginUser(Username.Trim(), Password.Trim());

                var mainWindow = new MainUserScreen(user);
                mainWindow.Show();

                Application.Current.Windows
                    .OfType<Window>()
                    .FirstOrDefault(w => w is LoginPage)?
                    .Close();
            }
            catch (Exception e)
            {
                var logExc = new ActionOnLog(OLLDelegates.LogError);
                logExc(e.Message);

                MessageBox.Show("Login failed: " + e.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void OpenRegisterWindow()
        {
            var registerWindow = new RegisterPage();
            registerWindow.ShowDialog();
        }
    }
}
