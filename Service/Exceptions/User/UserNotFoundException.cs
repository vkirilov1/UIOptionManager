namespace Service.Exceptions.User
{
    public class UserNotFoundException(string username) : Exception($"{username} not found!")
    {
    }
}

