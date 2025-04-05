namespace Service.Exceptions
{
    public class EmptySelectedOptionException(string listName) : Exception($"Error when selecting option from the list {listName}! Option cannot be empty or null!")
    {
    }
}