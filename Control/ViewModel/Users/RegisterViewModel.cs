using Control.Components.Common;
using Control.Components.Users;
using Control.Others.Commands;
using Service.Model.Users;
using Service.Others.OptionListLoggerDelegates;
using System.Windows;
using System.Windows.Input;

namespace Control.ViewModel.Users
{
    public class RegisterViewModel : DependencyObject
    {
        public static readonly DependencyProperty NameProperty =
            DependencyProperty.Register(
                nameof(Name),
                typeof(string),
                typeof(RegisterViewModel),
                new PropertyMetadata(""));

        public static readonly DependencyProperty UsernameProperty =
            DependencyProperty.Register(
               nameof(Username),
               typeof(string),
               typeof(RegisterViewModel),
               new PropertyMetadata(""));

        public static readonly DependencyProperty EmailProperty =
            DependencyProperty.Register(
               nameof(Email),
               typeof(string),
               typeof(RegisterViewModel),
               new PropertyMetadata(""));

        public string Name
        {
            get => (string)GetValue(NameProperty);
            set => SetValue(NameProperty, value);
        }

        public string Username
        {
            get => (string)GetValue(UsernameProperty);
            set => SetValue(UsernameProperty, value);
        }

        public string Password { get; set; } = "";

        public string Email
        {
            get => (string)GetValue(EmailProperty);
            set => SetValue(EmailProperty, value);
        }

        public ICommand RegisterCommand { get; }

        public RegisterViewModel()
        {
            RegisterCommand = new DelegateCommandNoParam(Register, CanRegisterUser);
        }

        private void Register()
        {
            try
            {
                UserFactory.RegisterUser(
                    Name.Trim(),
                    Username.Trim(),
                    Password.Trim(),
                    string.IsNullOrWhiteSpace(Email) ? null : Email.Trim()
                    );

                Application.Current.Windows
                    .OfType<Window>()
                    .FirstOrDefault(w => w is RegisterPage)?
                    .Close();
            }
            catch (Exception e)
            {
                var logExc = new ActionOnLog(OLLDelegates.LogError);
                logExc(e.Message);

                MessageBox.Show("Registration failed: " + e.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private bool CanRegisterUser()
        {
            return !string.IsNullOrWhiteSpace(Name)
                && !string.IsNullOrWhiteSpace(Username)
                && !string.IsNullOrWhiteSpace(Password);
        }
    }
}
