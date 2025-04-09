using Control.Others.Constants;
using Control.ViewModel;
using Control.ViewModel.OptionList;
using Service.Exceptions;
using Service.Others.Identifiers.Model;

namespace Control.Others.Factories
{
    public static class OptionListFactory
    {
        private static readonly List<OptionListIdentifier> _addedLists = [];

        public static BaseOptionListViewModel CreateSystemOptionList<T>(OptionListIdentifier id, string? description = null)
            where T : SystemId<T>
        {
            if (!_addedLists.Contains(id))
            {
                _addedLists.Add(id);
                return new SystemOptionListViewModel<T>(id, description);
            }

            throw new ListAlreadyExistsException(id);
        }

        public static BaseOptionListViewModel CreateMixedOptionList<T>(OptionListIdentifier id, string? description = null)
            where T : SystemId<T>
        {
            if (!_addedLists.Contains(id))
            {
                _addedLists.Add(id);
                return new MixedOptionListViewModel<T>(id, description);
            }

            throw new ListAlreadyExistsException(id);
        }

        public static BaseOptionListViewModel CreateUserOptionList(OptionListIdentifier id, string? description = null)
        {
            if (!_addedLists.Contains(id))
            {
                _addedLists.Add(id);
                return new UserOptionsListViewModel(id, description);
            }

            throw new ListAlreadyExistsException(id);
        }
    }
}
