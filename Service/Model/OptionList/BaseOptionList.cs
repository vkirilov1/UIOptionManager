using Service.Model.Option;

namespace Service.Model.OptionList
{
    public abstract class BaseOptionList<TOption> where TOption : BaseOption
    {
        protected readonly List<TOption> _options = new();

        public IReadOnlyList<TOption> Options => _options.AsReadOnly();
    }
}
