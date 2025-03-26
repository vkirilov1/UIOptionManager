namespace Service.Exceptions
{
    public class EmptyUserOptionException(String listName) : Exception($"Error when adding option to the list {listName}! Option cannot be empty or null!")
    {
    }
}