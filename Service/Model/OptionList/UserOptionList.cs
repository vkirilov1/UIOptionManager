using Service.Model.Option;
using Service.Exceptions;

namespace Service.Model.OptionList
{
    public class UserOptionList : BaseOptionList<UserOption>
    {
        public void AddUserDefinedOption(string value)
        {
            if(String.IsNullOrEmpty(value))
            {
                throw new EmptyUserOptionException();
            }
            else
            {
                _options.Add(
                    new UserOption
                    {
                        Value = value
                    });
            }
        }
    }
}
