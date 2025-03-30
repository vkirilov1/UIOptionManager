using Service.Model.OptionList;
using Service.Others.Identifiers.Model;
using Service.Others.OptionListLoggerDelegates;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using UI.Others.Commands;
using UI.ViewModel.Option;

namespace UI.ViewModel.OptionList
{
    public class MixedOptionListViewModel<T> : BaseOptionListViewModel, INotifyPropertyChanged where T : SystemId<T>
    {
        private readonly MixedOptionList<T> _mixedOptionList;

        private string? _newOptionName;
        public string? NewOptionName
        {
            get => _newOptionName;
            set
            {
                _newOptionName = value;
                OnPropertyChanged();
                CommandManager.InvalidateRequerySuggested();
            }
        }

        public ICommand AddOptionCommand { get; }

        public MixedOptionListViewModel(string name)
            : this(name, null)
        {
        }

        public MixedOptionListViewModel(string name, string? description) : base(name)
        {
            try
            {
                _mixedOptionList = new(name, description);
                Description = description;

                _mixedOptionList.Options.ToList().ForEach(opt =>
                    _options.Add(new OptionViewModel(opt.Value)));

                AddOptionCommand = new DelegateCommand<string>(
                    _ => AddOption(),
                    _ => CanAddOption()
                );
            }
            catch (Exception e)
            {
                var logExc = new ActionOnLog(OLLDelegates.LogError);
                logExc(e.Message);
            }
        }

        private void AddOption()
        {
            try
            {
                _mixedOptionList.AddUserMixedOptionToList(NewOptionName);
                _options.Add(new OptionViewModel(NewOptionName));
                NewOptionName = string.Empty;
                OnPropertyChanged(nameof(Options));
            }
            catch (Exception e)
            {
                var logExc = new ActionOnLog(OLLDelegates.LogError);
                logExc(e.Message);
            }
        }

        private bool CanAddOption() => !string.IsNullOrWhiteSpace(NewOptionName);

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
