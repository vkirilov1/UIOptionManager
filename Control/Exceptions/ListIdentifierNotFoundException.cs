using Control.Others.Constants;

namespace Service.Exceptions
{
    public class ListIdentifierNotFoundException(OptionListIdentifier identifier) : Exception($"No ListViewModel registered for identifier {identifier}")
    {
    }
}

