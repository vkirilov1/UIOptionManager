using Control.Others.Commands;
using Control.Others.Constants;
using Control.ViewModel.Option;
using Service.Exceptions;
using Service.Model.OptionList;
using Service.Service;
using Service.Others.OptionListLoggerDelegates;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;

namespace Control.ViewModel.OptionList
{
    internal class UserOptionsListViewModel : BaseOptionListViewModel, INotifyPropertyChanged
    {
        private readonly UserOptionList? _userOptionList;
        private readonly UserOptionListService _service = new();

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
        public OptionViewModel? SelectedOption
        {
            get => _selectedOption;
            set
            {
                _selectedOption = value;
                OnPropertyChanged();

                if (_selectedOption != null && _userOptionList != null)
                {
                    try
                    {
                        _service.UpdateSelectedOption(_userOptionList, _selectedOption.Value);
                    }
                    catch (Exception e)
                    {
                        new ActionOnLog(OLLDelegates.LogError)(e.Message);
                    }
                }
            }
        }

        public ICommand AddOptionCommand { get; }
        public ICommand DeleteOptionCommand { get; }
        public ICommand EditOptionCommand { get; }

        internal UserOptionsListViewModel(OptionListIdentifier listIdentifier, int userId, string? description = null)
            : base(listIdentifier)
        {
            try
            {
                _userOptionList = _service.LoadOrCreate(listIdentifier.ToString(), description, userId);
                Description = _userOptionList.Description;

                foreach (var option in _userOptionList.Options)
                {
                    var vmOption = new OptionViewModel(option.Value, listIdentifier);
                    _options.Add(vmOption);

                    if (!string.IsNullOrEmpty(_userOptionList.SelectedOption) && vmOption.Value == _userOptionList.SelectedOption)
                    {
                        _selectedOption = vmOption;
                    }
                }

                AddOptionCommand = new DelegateCommand<string>(_ => AddOption(), _ => CanAddOption());
                DeleteOptionCommand = new DelegateCommand<object>(_ => DeleteOption(), _ => CanDeleteOption());
                EditOptionCommand = new DelegateCommand<object>(_ => EditOption(), _ => CanEditOption());
            }
            catch (Exception e)
            {
                new ActionOnLog(OLLDelegates.LogError)(e.Message);
            }
        }

        private void AddOption()
        {
            try
            {
                var trimmedName = NewOptionName!.Trim();

                _service.AddOption(_userOptionList!, trimmedName);

                var newVm = new OptionViewModel(trimmedName, ListIdentifier);
                _options.Add(newVm);

                SelectedOption = newVm;
                NewOptionName = string.Empty;

                OnPropertyChanged(nameof(FilteredOptions));
            }
            catch (OptionAlreadyExistsException e)
            {
                NewOptionName = null;
                MessageBox.Show(e.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                new ActionOnLog(OLLDelegates.LogError)(e.Message);
            }
            catch (Exception e)
            {
                new ActionOnLog(OLLDelegates.LogError)(e.Message);
            }
        }

        private void DeleteOption()
        {
            if (SelectedOption == null) return;

            try
            {
                var valueToDelete = SelectedOption.Value;

                _service.DeleteOption(_userOptionList!, valueToDelete);

                _options.Remove(SelectedOption);
                SelectedOption = null;

                OnPropertyChanged(nameof(FilteredOptions));
            }
            catch (Exception e)
            {
                new ActionOnLog(OLLDelegates.LogError)(e.Message);
            }
        }

        private void EditOption()
        {
            if (SelectedOption == null || string.IsNullOrWhiteSpace(NewOptionName)) return;

            try
            {
                var oldValue = SelectedOption.Value;
                var newValue = NewOptionName.Trim();

                _service.EditOption(_userOptionList!, oldValue, newValue);

                SelectedOption.Value = newValue;
                NewOptionName = string.Empty;

                OnPropertyChanged(nameof(FilteredOptions));
            }
            catch (Exception e)
            {
                new ActionOnLog(OLLDelegates.LogError)(e.Message);
            }
        }

        private bool CanAddOption() => !string.IsNullOrWhiteSpace(NewOptionName);
        private bool CanDeleteOption() => SelectedOption != null;
        private bool CanEditOption() => SelectedOption != null && !string.IsNullOrWhiteSpace(NewOptionName);
    }
}