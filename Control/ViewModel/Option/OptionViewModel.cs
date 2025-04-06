namespace Control.ViewModel.Option
{
    public class OptionViewModel
    {
        public string Value { get; set; } = string.Empty;

        public OptionViewModel(string value)
        {
            Value = value;
        }
    }
}
