using DataLayer.Model.OptionList;
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
        public virtual MixedOptionListDBEntry? MixedOptionList { get; set; }
    }
}
 