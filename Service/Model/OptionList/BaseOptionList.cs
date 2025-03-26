using Service.Model.Option;

namespace Service.Model.OptionList
{
    public abstract class BaseOptionList<TOption> where TOption : BaseOption
    {
        public string? Name { get; set; }

        public string? Description { get; set; }

        protected readonly List<TOption> _options = [];

        public IReadOnlyList<TOption> Options => _options.AsReadOnly();
    }
}
