using Service.Others.Identifiers.Model;

namespace Service.Model.Option
{
    public class SystemOption<T> : BaseOption where T : SystemId<T>
    {
        public required SystemId<T> SysId { get; set; }
    }
}
