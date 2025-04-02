using Service.Exceptions;
using Service.Model.OptionList;
using Service.Others.OptionListLoggerDelegates;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using UI.Others.Commands;
using UI.ViewModel.Option;

namespace UI.ViewModel.OptionList
{
    public class UserOptionsListViewModel : BaseOptionListViewModel, INotifyPropertyChanged
    {
        private readonly UserOptionList _userOptionList;

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

        public UserOptionsListViewModel(string name)
            : this(name, null)
        {
        }

        public UserOptionsListViewModel(string name, string? description) : base(name)
        {
            try
            {
                _userOptionList = new(name, description);
                Description = description;

                _userOptionList.Options.ToList().ForEach(opt =>
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
                _userOptionList.AddUserDefinedOption(NewOptionName.Trim());
                _options.Add(new OptionViewModel(NewOptionName.Trim()));
                NewOptionName = string.Empty;
                OnPropertyChanged(nameof(Options));
            }
            catch (OptionAlreadyExistsException e)
            {
                NewOptionName = null;
                MessageBox.Show(e.Message);
                var logExc = new ActionOnLog(OLLDelegates.LogError);
                logExc(e.Message);
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