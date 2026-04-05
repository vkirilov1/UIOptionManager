using DataLayer.Model.Users;
using Service.Exceptions.User;
using Service.Others.Factories;
using Service.Others.OptionListLoggerDelegates;
using Service.Others.Security;

namespace Service.Model.Users
{
    public static class UserFactory
    {
        public static void RegisterUser(string name, string username, string password, string? email = null)
        {
            if (string.IsNullOrWhiteSpace(username))
                throw new EmptyUsernameException();

            if (string.IsNullOrWhiteSpace(name))
                throw new EmptyNameException(username);

            if (string.IsNullOrWhiteSpace(password))
                throw new EmptyPasswordException(username);

            using var dbContext = DatabaseContextFactory.Create();

            if (dbContext.Users.Any(u => u.Username == username))
                throw new UsernameAlreadyExistsException(username);

            if (email != null)
            {
                if (dbContext.Users.Any(u => u.Email == email))
                    throw new EmailAlreadyExistsException(email);
            }

            var hashedPassword = PasswordHasher.HashPassword(password);

            var newDbEntry = new UserDBEntry()
            {
                Name = name,
                Username = username,
                Password = hashedPassword,
                Role = nameof(UserRoleEnum.Applicant),
                Email = email
            };

            dbContext.Users.Add(newDbEntry);
            dbContext.SaveChanges();

            var logInf = new ActionOnLog(OLLDelegates.LogInformation);
            logInf($"Registered user {name} with username: {username}");
        }

        public static User LoginUser(string username, string password)
        {
            if (string.IsNullOrWhiteSpace(username))
                throw new EmptyUsernameException();

            if (string.IsNullOrWhiteSpace(password))
                throw new EmptyPasswordException(username);

            using var dbContext = DatabaseContextFactory.Create();

            var dbEntry = dbContext.Users
                .SingleOrDefault(u => u.Username == username) ?? throw new UserNotFoundException(username);

            if (!PasswordHasher.VerifyPassword(password, dbEntry.Password))
                throw new IncorrectPasswordException(username);

            var logInf = new ActionOnLog(OLLDelegates.LogInformation);

            Enum.TryParse(dbEntry.Role, out UserRoleEnum role);

            logInf($"User {dbEntry.Name} with username: {dbEntry.Username} and role: {dbEntry.Role} has logged in!");

            return new User
            {
                Id = dbEntry.Id,
                Name = dbEntry.Name,
                Username = dbEntry.Username,
                Role = role,
                Email = dbEntry.Email
            };
        }
    }
}
