using Control.Others.Constants;

namespace Control.ViewModel.Option
{
    public class OptionViewModel
    {
        public string Value { get; set; } = string.Empty;

        public OptionListIdentifier Identifier { get; set; }

        public OptionViewModel(string value, OptionListIdentifier identifier)
        {
            Value = value;
            Identifier = identifier;
        }
    }
}
