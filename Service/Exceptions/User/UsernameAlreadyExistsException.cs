namespace Service.Exceptions.User
{
    public class UsernameAlreadyExistsException(string username) : Exception($"Username {username} already exists!")
    {
    }
}