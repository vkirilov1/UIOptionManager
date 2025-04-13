using DataLayer.Model.Option;
using Service.Others.Identifiers.Model;

namespace Service.Model.Option
{
    public class MixedOption<T> : BaseOption where T : SystemId<T>
    {
        public int Id { get; set; }
        public virtual SystemId<T>? SysId { get; set; }
        public int UserId { get; set; }

        public MixedOptionDBEntry ToDbEntry(int mixedOptionListId)
        {
            return new MixedOptionDBEntry
            {
                Value = Value,
                SystemId = SysId?.Value,
                MixedOptionListDBEntryId = mixedOptionListId,
                UserDBEntryId = UserId
            };
        }
    }
}
