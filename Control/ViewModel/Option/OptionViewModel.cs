using Control.Others.Constants;
using System.ComponentModel;

namespace Control.ViewModel.Option
{
    public class OptionViewModel : INotifyPropertyChanged
    {
        private string _value = string.Empty;

        public string Value
        {
            get => _value;
            set
            {
                _value = value;
                OnPropertyChanged();
            }
        }

        public OptionListIdentifier Identifier { get; set; }

        public OptionViewModel(string value, OptionListIdentifier identifier)
        {
            _value = value;
            Identifier = identifier;
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([System.Runtime.CompilerServices.CallerMemberName] string? name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
