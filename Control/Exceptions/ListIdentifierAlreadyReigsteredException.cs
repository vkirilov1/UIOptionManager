using Control.Others.Constants;

namespace Service.Exceptions
{
    public class ListIdentifierAlreadyRegisteredException(OptionListIdentifier identifier) : Exception($"An option list with identifier '{identifier}' is already registered.")
    {
    }
}

