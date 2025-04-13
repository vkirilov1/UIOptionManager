using Control.Others.Constants;
using Control.ViewModel.Option;
using Service.Model.OptionList;
using Service.Others.Identifiers.Constants;
using Service.Others.Identifiers.Model;
using Service.Others.OptionListLoggerDelegates;

namespace Control.ViewModel.OptionList
{
    internal class SystemOptionListViewModel<T> : BaseOptionListViewModel where T : SystemId<T>
    {
        private readonly SystemOptionList<T>? _systemOptionList;

        private string? _optionDescription;
        public string? OptionDescription
        {
            get => _optionDescription;
            set
            {
                _optionDescription = value;
                OnPropertyChanged();
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

                if (_selectedOption != null)
                {
                    try
                    {
                        _systemOptionList.UpdateSelectedOption(_selectedOption.Value);

                        if (typeof(T) == typeof(SystemIdConstants.EmploymentType))
                        {
                            var selectedItem = SystemIdConstants.EmploymentType.GetAllValues().FirstOrDefault(v => v.Name == _selectedOption.Value);
                            DisplayEmploymentTypeHours(selectedItem);
                        }
                    }
                    catch (Exception e)
                    {
                        var logExc = new ActionOnLog(OLLDelegates.LogError);
                        logExc(e.Message);
                    }
                }
            }
        }

        internal SystemOptionListViewModel(OptionListIdentifier listIdentifier, int userId, string? description=null) : base(listIdentifier)
        {
            try
            {
                _systemOptionList = new(listIdentifier.ToString(), description, userId);

                _systemOptionList.Options.ToList().ForEach(option =>
                {
                    var vmOption = new OptionViewModel(option.Value, listIdentifier);
                    _options.Add(vmOption);

                    if (!string.IsNullOrEmpty(_systemOptionList.SelectedOption) && vmOption.Value == _systemOptionList.SelectedOption)
                    {
                        SelectedOption = vmOption;
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

        private void DisplayEmploymentTypeHours(SystemIdConstants.EmploymentType? selectedItem)
        {
            if (selectedItem != null)
            {
                OptionDescription = selectedItem switch
                {
                    var _ when selectedItem.Equals(SystemIdConstants.EmploymentType.FullTime) => "Full-time employees work 40 hours per week.",
                    var _ when selectedItem.Equals(SystemIdConstants.EmploymentType.PartTime) => "Part-time employees work around 20 hours per week.",
                    var _ when selectedItem.Equals(SystemIdConstants.EmploymentType.Internship) => "Internships are typically 10–20 hours per week.",
                    _ => "Select an option to see work hours for each employment type.",
                };
            }

        }
    }
}
