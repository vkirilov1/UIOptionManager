namespace Service.Exceptions.User
{
    public class EmptyNameException(string username) : Exception($"The name of the user {username} cannot be empty!")
    {
    }
}

