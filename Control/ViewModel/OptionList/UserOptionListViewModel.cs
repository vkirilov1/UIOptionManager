using Service.Exceptions;
using Service.Model.OptionList;
using Service.Others.OptionListLoggerDelegates;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using Control.Others.Commands;
using Control.Others.Constants;
using Control.ViewModel.Option;

namespace Control.ViewModel.OptionList
{
    internal class UserOptionsListViewModel : BaseOptionListViewModel, INotifyPropertyChanged
    {
        private readonly UserOptionList? _userOptionList;

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
                        _userOptionList.UpdateSelectedOption(_selectedOption.Value);
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

        internal UserOptionsListViewModel(OptionListIdentifier listIdentifier, string? description) : base(listIdentifier)
        {
            try
            {
                _userOptionList = new(listIdentifier.ToString(), description);
                Description = description;

                _userOptionList.Options.ToList().ForEach(option =>
                {
                    var vmOption = new OptionViewModel(option.Value, listIdentifier);
                    _options.Add(vmOption);

                    if (!string.IsNullOrEmpty(_userOptionList.SelectedOption) && vmOption.Value == _userOptionList.SelectedOption)
                    {
                        _selectedOption = vmOption;
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
                _userOptionList.AddUserDefinedOption(NewOptionName.Trim());
                _options.Add(new OptionViewModel(NewOptionName.Trim(), ListIdentifier));
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