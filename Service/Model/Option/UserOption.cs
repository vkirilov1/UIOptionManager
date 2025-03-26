using DataLayer.Model.Option;

namespace Service.Model.Option
{
    public class UserOption : BaseOption
    {
        public virtual int Id { get; set; }

        public UserOptionDBEntry ToDbEntry(int userOptionListId)
        {
            return new UserOptionDBEntry
            {
                Value = Value,
                UserOptionListDBEntryId = userOptionListId
            };
        }
    }
}
