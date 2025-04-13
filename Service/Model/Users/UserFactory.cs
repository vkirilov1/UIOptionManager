using DataLayer.Model.Users;
using Service.Exceptions.User;
using Service.Others.Factories;
using Service.Others.OptionListLoggerDelegates;
using Service.Others.PasswordHasher;

namespace Service.Model.Users
{
    public static class UserFactory
    {
        public static User RegisterUser(string name, string username, string password, string? email = null)
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

            var hashedPassword = PasswordHasher.HashPassword(password);

            var newDbEntry = new UserDBEntry()
            {
                Name = name,
                Username = username,
                Password = hashedPassword,
                Email = email
            };

            dbContext.Users.Add(newDbEntry);
            dbContext.SaveChanges();

            var logInf = new ActionOnLog(OLLDelegates.LogInformation);
            logInf($"Registered user {name} with username: {username}");
                
            return new User
            {
                Id = newDbEntry.Id,
                Name = newDbEntry.Name,
                Username = newDbEntry.Username,
                Email = newDbEntry.Email
            };
        }

        public static User LoginUser(string username, string password)
        {
            if (string.IsNullOrWhiteSpace(username))
                throw new EmptyUsernameException();

            if (string.IsNullOrWhiteSpace(password))
                throw new EmptyPasswordException(username);

            using var dbContext = DatabaseContextFactory.Create();

            var dbEntry = dbContext.Users
                .SingleOrDefault(u => u.Username == username);

            if (dbEntry == null || !PasswordHasher.VerifyPassword(password, dbEntry.Password))
                throw new UserNotFoundException(username);

            var logInf = new ActionOnLog(OLLDelegates.LogInformation);
            logInf($"User {dbEntry.Name} with username: {dbEntry.Username} has logged in!");

            return new User
            {
                Id = dbEntry.Id,
                Name = dbEntry.Name,
                Username = dbEntry.Username,
                Email = dbEntry.Email
            };
        }
    }
}
