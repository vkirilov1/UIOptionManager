namespace Service.Model.Option
{
    public class MixedOption<TEnum> : BaseOption where TEnum : struct, Enum
    {
        public TEnum? SystemId { get; set; }
    }
}
