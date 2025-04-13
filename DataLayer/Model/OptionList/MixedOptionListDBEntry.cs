﻿using DataLayer.Model.Option;
using DataLayer.Model.Users;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataLayer.Model.OptionList
{
    public class MixedOptionListDBEntry
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public required string Name { get; set; }

        public string? Description { get; set; }

        public string? SelectedOption { get; set; }

        public required string SystemIdType { get; set; }

        public required int UserDBEntryId { get; set; }

        [ForeignKey(nameof(UserDBEntryId))]
        public UserDBEntry? User {  get; set; }

        public ICollection<MixedOptionDBEntry> Options { get; set; } = [];
    }
}
