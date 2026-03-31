using Control.Others.Constants;
using Control.ViewModel.Option;
using Service.Exceptions;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Control.ViewModel
{
    public abstract class BaseOptionListViewModel : INotifyPropertyChanged
    {
        public OptionListIdentifier ListIdentifier { get; }

        public string Name { get; set; }

        public string? Description { get; set; }

        protected readonly ObservableCollection<OptionViewModel> _options = [];

        public ObservableCollection<OptionViewModel> Options => _options;

        private string _filterText = string.Empty;
        public string FilterText
        {
            get => _filterText;
            set
            {
                _filterText = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(FilteredOptions));
            }
        }

        internal BaseOptionListViewModel(OptionListIdentifier listIdentifier)
        {
            Name = listIdentifier.ToString() ?? throw new EmptyListNameException(nameof(BaseOptionListViewModel));
            ListIdentifier = listIdentifier;
        }

        public event PropertyChangedEventHandler? PropertyChanged;  
        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        public IEnumerable<OptionViewModel> FilteredOptions
        {
            get
            {
                if (string.IsNullOrWhiteSpace(FilterText))
                    return Options;

                return Options.Where(opt => opt.Value.Contains(FilterText, StringComparison.OrdinalIgnoreCase));
            }
        }
    }
}
