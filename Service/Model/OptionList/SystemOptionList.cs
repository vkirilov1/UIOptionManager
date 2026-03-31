using Service.Exceptions;
using Service.Model.Option;
using Service.Others.Identifiers.Model;

namespace Service.Model.OptionList
{
    public class SystemOptionList<T> : BaseOptionList<SystemOption<T>> where T : SystemId<T>
    {
        public SystemOptionList(string name, string? description, int userId) : base(name, userId)
        {
            Description = description;
            InitializeOptions();
        }

        private void InitializeOptions()
        {
            foreach (var sysId in SystemId<T>.GetAllValues())
            {
                _options.Add(new SystemOption<T>
                {
                    SysId = sysId,
                    Value = sysId.Name
                });
            }
        }

        public void ValidateSelection(string selectedOption)
        {
            if (string.IsNullOrEmpty(selectedOption))
                throw new EmptySelectedOptionException(Name);

            if (!_options.Any(opt => opt.Value == selectedOption))
                throw new OptionDoesNotExistException(selectedOption, Name);
        }
    }
}