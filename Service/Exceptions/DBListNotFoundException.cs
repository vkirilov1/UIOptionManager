namespace Service.Exceptions
{
    public class DBListNotFoundException(string listName) : Exception($"Options list '{listName}' not found in the database.")
    {
    }
}

