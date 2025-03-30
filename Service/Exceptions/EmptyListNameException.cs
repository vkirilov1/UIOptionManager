namespace Service.Exceptions
{
    public class EmptyListNameException(string listType) : Exception($"Error when adding list of type {listType}! List name cannot be empty or null!")
    {
    }
}
