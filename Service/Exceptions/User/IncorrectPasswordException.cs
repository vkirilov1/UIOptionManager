namespace Service.Exceptions.User
{
    public class IncorrectPasswordException(string username) : Exception($"Incorrect password for username {username}!")
    {
    }
}
