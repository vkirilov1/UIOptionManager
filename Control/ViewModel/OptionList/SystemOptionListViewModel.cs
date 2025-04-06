using Service.Model.OptionList;
using Service.Others.Identifiers.Model;
using Service.Others.OptionListLoggerDelegates;
using Control.Others.Constants;
using Control.ViewModel.Option;

namespace Control.ViewModel.OptionList
{
    internal class SystemOptionListViewModel<T> : BaseOptionListViewModel where T : SystemId<T>
    {
        private readonly SystemOptionList<T>? _systemOptionList;

        private OptionViewModel? _selectedOption;
        public OptionViewModel? SelectedOption
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
                        _systemOptionList.UpdateSelectedOption(_selectedOption.Value);
                    }
                    catch (Exception e)
                    {
                        var logExc = new ActionOnLog(OLLDelegates.LogError);
                        logExc(e.Message);
                    }
                }
            }
        }

        internal SystemOptionListViewModel(OptionListIdentifier listIdentifier, string? description) : base(listIdentifier)
        {
            try
            {
                _systemOptionList = new(listIdentifier.ToString(), description);

                _systemOptionList.Options.ToList().ForEach(option =>
                {
                    var vmOption = new OptionViewModel(option.Value);
                    _options.Add(vmOption);

                    if (!string.IsNullOrEmpty(_systemOptionList.SelectedOption) && vmOption.Value == _systemOptionList.SelectedOption)
                    {
                        _selectedOption = vmOption;
                    }
                });
            }
            catch (Exception e)
            {
                var logExc = new ActionOnLog(OLLDelegates.LogError);
                logExc(e.Message);
            }

            Description = description;
        }
    }
}
