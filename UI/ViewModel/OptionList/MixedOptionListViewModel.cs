using Service.Exceptions;
using Service.Model.OptionList;
using Service.Others.Identifiers.Model;
using Service.Others.OptionListLoggerDelegates;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using UI.Others.Commands;
using UI.ViewModel.Option;

namespace UI.ViewModel.OptionList
{
    public class MixedOptionListViewModel<T> : BaseOptionListViewModel, INotifyPropertyChanged where T : SystemId<T>
    {
        private readonly MixedOptionList<T>? _mixedOptionList;

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

        private OptionViewModel? _selectedOption;
        public OptionViewModel SelectedOption
        {
            get => _selectedOption;
            set
            {
                _selectedOption = value;
                OnPropertyChanged();

                if (_selectedOption != null)
                {
                    try
                    {
                        _mixedOptionList.UpdateSelectedOption(_selectedOption.Value);
                    }
                    catch (Exception e)
                    {
                        var logExc = new ActionOnLog(OLLDelegates.LogError);
                        logExc(e.Message);
                    }
                }
            }
        }

        public ICommand? AddOptionCommand { get; }

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

                _mixedOptionList.Options.ToList().ForEach(option =>
                {
                    var vmOption = new OptionViewModel(option.Value);
                    _options.Add(vmOption);

                    if (!string.IsNullOrEmpty(_mixedOptionList.SelectedOption))
                    {
                        if (vmOption.Value == _mixedOptionList.SelectedOption)
                        {
                            _selectedOption = vmOption;
                        }
                    }
                });

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
                _mixedOptionList.AddUserMixedOptionToList(NewOptionName.Trim());
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
    }
}
