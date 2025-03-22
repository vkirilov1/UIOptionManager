using Service.Model.Option;

namespace Service.Model.OptionList
{
    public class SystemOptionList<TEnum> : BaseOptionList<SystemOption<TEnum>> where TEnum : Enum
    {
        public SystemOptionList()
        {
            Enum.GetValues(typeof(TEnum))
                .Cast<TEnum>()
                .ToList()
                .ForEach(
                    e => _options.Add(
                        new SystemOption<TEnum>
                        {
                            SystemId = e,
                            Value = e.ToString()
                        }
                    ));
        }
    }
}
