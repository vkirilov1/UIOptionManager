using Service.Exceptions;
using Service.Model.Option;

namespace Service.Model.OptionList
{
    public class UserOptionList : BaseOptionList<UserOption>
    {
        public UserOptionList(string name, string? description, int userId) : base(name, userId)
        {
            Description = description;
        }

        public void InternalAddOption(UserOption option)
        {
            _options.Add(option);
        }

        public void AddOption(string value)
        {
            ValidateValue(value);
            _options.Add(new UserOption { Value = value, UserId = UserId });
        }

        public void ValidateValue(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new EmptyUserOptionException(Name);

            if (_options.Any(opt => opt.Value == value))
                throw new OptionAlreadyExistsException(Name, value);
        }

        public void ValidateSelection(string selectedOption)
        {
            if (string.IsNullOrEmpty(selectedOption))
                throw new EmptySelectedOptionException(Name);

            if (!_options.Any(opt => opt.Value == selectedOption))
                throw new OptionDoesNotExistException(selectedOption, Name);
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