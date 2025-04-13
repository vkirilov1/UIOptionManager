using DataLayer.Model.OptionList;
using DataLayer.Model.Users;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataLayer.Model.Option
{
    public class UserOptionDBEntry
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public required string Value { get; set; }

        public int UserOptionListDBEntryId { get; set; }

        [ForeignKey(nameof(UserOptionListDBEntryId))]
        public UserOptionListDBEntry? UserOptionList { get; set; }

        public required int UserDBEntryId { get; set; }

        [ForeignKey(nameof(UserDBEntryId))]
        public UserDBEntry? User { get; set; }
    }
}
