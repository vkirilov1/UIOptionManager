namespace Service.Exceptions.User
{
    public class EmptyPasswordException(string username) : Exception($"Password for username {username} cannot be empty!")
    {
    }
}

