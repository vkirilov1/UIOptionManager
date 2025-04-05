namespace Service.Exceptions
{
    public class OptionDoesNotExistException(string selectedOption, string listName) : Exception($"Option '{selectedOption}' not found in list '{listName}'.")
    {
    }
}