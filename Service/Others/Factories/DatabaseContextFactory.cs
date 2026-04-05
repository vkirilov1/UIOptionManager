using DataLayer.Database;
using DataLayer.Model.Users;
using Service.Model.Users;
using Service.Others.Security;

namespace Service.Others.Factories
{
    public static class DatabaseContextFactory
    {
        public static DatabaseContext Create()
        {
            var context = new DatabaseContext();
            context.Database.EnsureCreated();

            //Adding testing Reviewer account if it doesn't exist
            if (!context.Users.Any(u => u.Username == "admin"))
            {
                context.Users.Add(new UserDBEntry
                {
                    Name = "System Reviewer",
                    Username = "admin",
                    Email = "reviewer@company.com",
                    Password = PasswordHasher.HashPassword("Admin123"),
                    Role = nameof(UserRoleEnum.Reviewer)
                });
                context.SaveChanges();
            }

            return context;
        }
    }
}
