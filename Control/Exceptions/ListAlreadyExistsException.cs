using Control.Others.Constants;

namespace Service.Exceptions
{
    public class ListAlreadyExistsException(OptionListIdentifier identifier) : Exception($"The option list with identifier '{identifier}' already exists.")
    {
    }
}

