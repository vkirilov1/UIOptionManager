using Service.Exceptions;
using Service.Model.Option;
using Service.Others.Identifiers.Model;

namespace Service.Model.OptionList
{
    public class MixedOptionList<T> : BaseOptionList<MixedOption<T>> where T : SystemId<T>
    {
        public MixedOptionList(string name, string? description, int userId) : base(name, userId)
        {
            Description = description;
        }

        public void OptionsInternalAdd(MixedOption<T> option)
        {
            _options.Add(option);
        }

        public void InitializeSystemOptions()
        {
            foreach (var sysId in SystemId<T>.GetAllValues())
            {
                _options.Add(new MixedOption<T>
                {
                    SysId = sysId,
                    Value = sysId.Name,
                    UserId = UserId
                });
            }
        }

        public void AddUserMixedOption(string value)
        {
            if (string.IsNullOrEmpty(value))
                throw new EmptyUserOptionException(Name);

            if (_options.Any(opt => opt.Value == value))
                throw new OptionAlreadyExistsException(Name, value);

            _options.Add(new MixedOption<T> { Value = value, SysId = null, UserId = UserId });
        }

        public void ValidateSelection(string selectedOption)
        {
            if (string.IsNullOrEmpty(selectedOption))
                throw new EmptySelectedOptionException(Name);

            if (!_options.Any(o => o.Value == selectedOption))
                throw new OptionDoesNotExistException(selectedOption, Name);
        }

        public void ValidateEdit(string oldValue, string newValue)
        {
            if (string.IsNullOrWhiteSpace(newValue))
                throw new EmptyUserOptionException(Name);

            if (_options.Any(o => o.Value == newValue))
                throw new OptionAlreadyExistsException(Name, newValue);
        }

        public void UpdateOptionLocal(string oldValue, string newValue)
        {
            var localOption = _options.FirstOrDefault(o => o.Value == oldValue);
            if (localOption != null) localOption.Value = newValue;

            if (SelectedOption == oldValue)
                SelectedOption = newValue;
        }

        public void RemoveOptionLocal(string value)
        {
            var localOption = _options.FirstOrDefault(o => o.Value == value);
            if (localOption != null)
                _options.Remove(localOption);

            if (SelectedOption == value)
                SelectedOption = null;
        }
    }
}