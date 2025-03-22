using Service.Exceptions;
using Service.Model.Option;

namespace Service.Model.OptionList
{
    public class MixedOptionList<TEnum> : BaseOptionList<MixedOption<TEnum>> where TEnum : struct, Enum
    {
        public MixedOptionList()
        {
            Enum.GetValues(typeof(TEnum))
                .Cast<TEnum>()
                .ToList()
                .ForEach(
                    e => _options.Add(
                        new MixedOption<TEnum>
                        {
                            SystemId = e,
                            Value = e.ToString()
                        }
                    ));
        }

        public void AddUserDefinedOption(string value)
        {
            if (String.IsNullOrEmpty(value))
            {
                throw new EmptyUserOptionException();
            }
            else
            {
                _options.Add(
                    new MixedOption<TEnum>
                    {
                        Value = value,
                        SystemId = null
                    });
            }
        }
    }
}
