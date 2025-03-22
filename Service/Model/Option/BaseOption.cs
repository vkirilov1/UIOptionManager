namespace Service.Model.Option
{
    public abstract class BaseOption
    {
        public int Id { get; set; }
        public string Value { get; set; } = string.Empty;
    }
}
