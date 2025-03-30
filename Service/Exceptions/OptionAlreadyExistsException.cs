namespace Service.Exceptions
{
    public class OptionAlreadyExistsException(string listName, string option) : Exception($"Error when adding option to the list {listName}! The option \"{option}\" is already in the list specified!")
    {
    }
}
