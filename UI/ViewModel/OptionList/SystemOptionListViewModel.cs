using Service.Model.OptionList;
using Service.Others.Identifiers.Model;
using UI.ViewModel.Option;

namespace UI.ViewModel.OptionList
{
    public class SystemOptionListViewModel<T> : BaseOptionListViewModel where T : SystemId<T>
    {
        private readonly SystemOptionList<T> _systemOptionList;

        public SystemOptionListViewModel(string name)
            : this(name, null)
        {
        }

        public SystemOptionListViewModel(string name, string? description) : base(name)
        {
            _systemOptionList = new(name, description);

            _systemOptionList.Options.ToList().ForEach(option =>
            {
                _options.Add(new OptionViewModel(option.Value));
            });

            Description = description;
        }
    }
}
