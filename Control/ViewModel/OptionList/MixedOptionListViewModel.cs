using Control.Others.Commands;
using Control.Others.Constants;
using Control.ViewModel.Option;
using Control.ViewModel;
using Service.Model.OptionList;
using Service.Others.Identifiers.Model;
using Service.Others.OptionListLoggerDelegates;
using Service.Service;
using System.ComponentModel;
using System.Windows.Input;
using System.Windows;

internal class MixedOptionListViewModel<T> : BaseOptionListViewModel, INotifyPropertyChanged where T : SystemId<T>
{
    private readonly MixedOptionList<T>? _mixedOptionList;
    private readonly MixedOptionListService<T> _service = new();

    private string? _newOptionName;
    public string? NewOptionName
    {
        get => _newOptionName;
        set { _newOptionName = value; OnPropertyChanged(); CommandManager.InvalidateRequerySuggested(); }
    }

    private OptionViewModel? _selectedOption;
    public OptionViewModel SelectedOption
    {
        get => _selectedOption;
        set
        {
            _selectedOption = value;
            OnPropertyChanged();

            if (_selectedOption != null && _mixedOptionList != null)
            {
                try
                {
                    _service.UpdateSelectedOption(_mixedOptionList, _selectedOption.Value);
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

    internal MixedOptionListViewModel(OptionListIdentifier listIdentifier, int userId, string? description = null) : base(listIdentifier)
    {
        try
        {
            _mixedOptionList = _service.LoadOrCreate(listIdentifier.ToString(), description, userId);
            Description = _mixedOptionList.Description;

            foreach (var option in _mixedOptionList.Options)
            {
                var vmOption = new OptionViewModel(option.Value, listIdentifier);
                _options.Add(vmOption);

                if (vmOption.Value == _mixedOptionList.SelectedOption)
                    _selectedOption = vmOption;
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
            var name = NewOptionName!.Trim();
            _service.AddOption(_mixedOptionList!, name);

            var newVm = new OptionViewModel(name, ListIdentifier);
            _options.Add(newVm);
            SelectedOption = newVm;
            NewOptionName = string.Empty;

            OnPropertyChanged(nameof(FilteredOptions));
        }
        catch (Exception e)
        {
            MessageBox.Show(e.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            new ActionOnLog(OLLDelegates.LogError)(e.Message);
        }
    }

    private void DeleteOption()
    {
        try
        {
            var val = SelectedOption.Value;
            _service.DeleteOption(_mixedOptionList!, val);
            _options.Remove(SelectedOption);
            SelectedOption = null;

            OnPropertyChanged(nameof(FilteredOptions));
        }
        catch (Exception e) { new ActionOnLog(OLLDelegates.LogError)(e.Message); }
    }

    private void EditOption()
    {
        try
        {
            var oldValue = SelectedOption.Value;
            var newValue = NewOptionName!.Trim();

            _service.EditOption(_mixedOptionList!, oldValue, newValue);

            SelectedOption.Value = newValue;
            NewOptionName = string.Empty;

            OnPropertyChanged(nameof(FilteredOptions));
        }
        catch (Exception e) { new ActionOnLog(OLLDelegates.LogError)(e.Message); }
    }

    private bool CanAddOption() => !string.IsNullOrWhiteSpace(NewOptionName);
    private bool CanDeleteOption() => SelectedOption != null;
    private bool CanEditOption() => SelectedOption != null && !string.IsNullOrWhiteSpace(NewOptionName);
}