using DataLayer.Model.JobApplication;
using Microsoft.EntityFrameworkCore;
using Service.Exceptions.JobApplication;
using Service.Model.JobApplication;
using Service.Model.Users;
using Service.Others.Factories;

namespace Service.Service
{
    public class JobApplicationService
    {
        public void Apply(User user, string position, string coverLetter, string employmentType, string city)
        {
            if (string.IsNullOrWhiteSpace(position) || string.IsNullOrEmpty(coverLetter) || string.IsNullOrWhiteSpace(employmentType) || string.IsNullOrEmpty(city))
            {
                throw new InvalidJobApplication();
            }

            if (coverLetter.Length < 200)
            {
                throw new CoverLetterSizeException();
            }

            JobApplication app = new JobApplication
            {
                AppliedDate = DateTime.Now,
                Position = position,
                CoverLetter = coverLetter,
                EmploymentType = employmentType,
                City = city,
                Status = JobApplicationStatusEnum.Pending,
                User = user
            };

            Create(app);
        }

        public void Create(JobApplication app)
        {
            using var db = DatabaseContextFactory.Create();

            var dbEntry = new JobApplicationDBEntry
            {
                AppliedDate = app.AppliedDate,
                Position = app.Position,
                CoverLetter = app.CoverLetter,
                EmploymentType = app.EmploymentType,
                City = app.City,
                Status = app.Status.ToString(),
                UserDBEntryId = app.User.Id
            };

            db.Application.Add(dbEntry);
            db.SaveChanges();

            app.Id = dbEntry.Id;
        }

        public List<JobApplication> GetAllPendingApplications()
        {
            using var db = DatabaseContextFactory.Create();

            return [.. db.Application
                .Include(a => a.User)
                .Where(a => a.Status == JobApplicationStatusEnum.Pending.ToString())
                .Select(static entry => new JobApplication
                {
                    Id = entry.Id,
                    AppliedDate = entry.AppliedDate,
                    Position = entry.Position,
                    CoverLetter = entry.CoverLetter,
                    EmploymentType = entry.EmploymentType,
                    City = entry.City,
                    Status = Enum.Parse<JobApplicationStatusEnum>(entry.Status),
                    User = new User
                    {
                        Id = entry.UserDBEntryId,
                        Name = entry.User.Name,
                        Username = entry.User.Username
                    }
                })];
        }

        public void UpdateApplicationStatus(int applicationId, JobApplicationStatusEnum newStatus)
        {
            using var db = DatabaseContextFactory.Create();
            var entity = db.Application.FirstOrDefault(a => a.Id == applicationId);

            if (entity != null)
            {
                entity.Status = newStatus.ToString();
                db.SaveChanges();
            }
        }

        public List<JobApplication> GetUserApplicationHistory(int userId)
        {
            using var db = DatabaseContextFactory.Create();

            return [.. db.Application
                .Where(a => a.UserDBEntryId == userId)
                .OrderByDescending(a => a.AppliedDate)
                .Select(entry => new JobApplication
                {
                    Id = entry.Id,
                    AppliedDate = entry.AppliedDate,
                    Position = entry.Position,
                    EmploymentType = entry.EmploymentType,
                    Status = Enum.Parse<JobApplicationStatusEnum>(entry.Status),
                })];
        }
    }
}
