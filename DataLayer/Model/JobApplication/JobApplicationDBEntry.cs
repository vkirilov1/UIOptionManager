using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DataLayer.Model.Users;

namespace DataLayer.Model.JobApplication
{
    public class JobApplicationDBEntry
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public DateTime AppliedDate { get; set; }

        public required string Position { get; set; }

        public required string CoverLetter { get; set; }

        public required int UserDBEntryId { get; set; }

        [ForeignKey(nameof(UserDBEntryId))]
        public UserDBEntry? User { get; set; }

        public required string EmploymentType { get; set; }

        public required string City { get; set; }

        public string Status { get; set; } = "Pending";
    }
}