using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using DataLayer.Model.Option;

namespace DataLayer.Model.OptionList
{
    public class UserOptionListDBEntry
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public required string Name { get; set; }

        public string? Description { get; set; }

        public virtual List<UserOptionDBEntry> Options { get; set; } = [];
    }
}
