using DataLayer.Model.OptionList;
using DataLayer.Model.Users;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataLayer.Model.Option
{
    public class MixedOptionDBEntry
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public required string Value { get; set; }  

        public int? SystemId { get; set; }

        public int MixedOptionListDBEntryId { get; set; }

        [ForeignKey(nameof(MixedOptionListDBEntryId))]
        public MixedOptionListDBEntry? MixedOptionList { get; set; }

        public required int UserDBEntryId { get; set; }

        [ForeignKey(nameof(UserDBEntryId))]
        public UserDBEntry? User { get; set; }
    }
}
