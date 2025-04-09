using Control.Others.Constants;
using Control.ViewModel.Option;
using Service.Exceptions;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Control.ViewModel
{
    public abstract class BaseOptionListViewModel : INotifyPropertyChanged
    {
        public OptionListIdentifier ListIdentifier { get; }

        public string Name { get; set; }

        public string? Description { get; set; }

        protected readonly List<OptionViewModel> _options = [];

        public IReadOnlyList<OptionViewModel> Options => _options.AsReadOnly();

        internal BaseOptionListViewModel(OptionListIdentifier listIdentifier)
        {
            Name = listIdentifier.ToString() ?? throw new EmptyListNameException(nameof(BaseOptionListViewModel));
            ListIdentifier = listIdentifier;
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
