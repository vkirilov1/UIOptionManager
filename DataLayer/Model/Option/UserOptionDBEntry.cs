using DataLayer.Model.OptionList;
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
        public virtual UserOptionListDBEntry? UserOptionList { get; set; }
    }
}
