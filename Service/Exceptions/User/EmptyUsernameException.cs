namespace Service.Exceptions.User
{
    public class EmptyUsernameException() : Exception($"The username cannot be empty!")
    {
    }
}

