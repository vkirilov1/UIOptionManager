using DataLayer.Model.Option;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataLayer.Model.OptionList
{
    public class UserOptionListDBEntry
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public required string Name { get; set; }

        public string? Description { get; set; }

        public string? SelectedOption { get; set; }

        public virtual List<UserOptionDBEntry> Options { get; set; } = [];
    }
}
