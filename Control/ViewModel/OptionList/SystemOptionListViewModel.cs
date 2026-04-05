using Control.Others.Constants;
using Control.ViewModel.Option;
using Service.Model.OptionList;
using Service.Others.Identifiers.Constants;
using Service.Others.Identifiers.Model;
using Service.Others.OptionListLoggerDelegates;
using Service.Service;

namespace Control.ViewModel.OptionList
{
    internal class SystemOptionListViewModel<T> : BaseOptionListViewModel where T : SystemId<T>
    {
        private readonly SystemOptionList<T>? _systemOptionList;
        private readonly SystemOptionListService<T> _service = new();

        private string? _optionDescription;
        public string? OptionDescription
        {
            get => _optionDescription;
            set { _optionDescription = value; OnPropertyChanged(); }
        }

        protected override void UpdateServiceSelection(OptionViewModel? selected)
        {
            if (selected != null && _systemOptionList != null)
            {
                try
                {
                    _service.UpdateSelectedOption(_systemOptionList, selected.Value);

                    if (typeof(T) == typeof(SystemIdConstants.EmploymentType))
                    {
                        var selectedItem = SystemIdConstants.EmploymentType.GetAllValues()
                            .FirstOrDefault(v => v.Name == selected.Value);
                        DisplayEmploymentTypeHours(selectedItem);
                    }
                }
                catch (Exception e) { new ActionOnLog(OLLDelegates.LogError)(e.Message); }
            }
        }

        internal SystemOptionListViewModel(OptionListIdentifier listIdentifier, int userId, string? description = null) : base(listIdentifier)
        {
            try
            {
                _systemOptionList = _service.LoadOrCreate(listIdentifier.ToString(), description, userId);

                foreach (var option in _systemOptionList.Options)
                {
                    var vmOption = new OptionViewModel(option.Value, listIdentifier);
                    _options.Add(vmOption);

                    if (vmOption.Value == _systemOptionList.SelectedOption)
                        SelectedOption = vmOption;
                }
            }
            catch (Exception e) { new ActionOnLog(OLLDelegates.LogError)(e.Message); }
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
