using Service.Model.Users;

namespace Service.Model.JobApplication
{
    public class JobApplication
    {
        public int Id { get; set; }

        public required DateTime AppliedDate { get; set; }

        public required string Position { get; set; }

        public string? CoverLetter { get; set; }

        public required string EmploymentType { get; set; }

        public string? City { get; set; }

        public JobApplicationStatusEnum Status { get; set; }

        public User? User { get; set; }
    }
}