namespace Service.Model.Option
{
    public class SystemOption<TEnum> : BaseOption where TEnum : Enum
    {
        public required TEnum SystemId { get; set; }
    }
}
