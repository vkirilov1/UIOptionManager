namespace Service.Exceptions.User
{
    public class EmailAlreadyExistsException(string email) : Exception($"Email {email} already registered!")
    {
    }
}
