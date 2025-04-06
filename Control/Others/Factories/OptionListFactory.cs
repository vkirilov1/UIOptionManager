using Service.Exceptions;
using Service.Others.Identifiers.Constants;
using Service.Others.Identifiers.Model;
using Service.Others.OptionListLoggerDelegates;
using Control.Others.Constants;
using Control.ViewModel;
using Control.ViewModel.OptionList;

namespace Control.Others.Factories
{
    public static class OptionListFactory
    {
        private static readonly Dictionary<OptionListIdentifier, Func<BaseOptionListViewModel>> _lists = [];
        private static readonly List<OptionListIdentifier> _addedLists = [];

        static OptionListFactory()
        {
            try
            {
                RegisterSystem<SystemIdConstants.EmploymentType>(OptionListIdentifier.EmploymentType);
                RegisterUser(OptionListIdentifier.Roles, "Shows different roles the user can input");
                RegisterUser(OptionListIdentifier.Colors);
                RegisterMixed<SystemIdConstants.WorkLocation>(OptionListIdentifier.WorkLocations);
            }
            catch(Exception e)
            {
                var logExc = new ActionOnLog(OLLDelegates.LogError);
                logExc(e.Message);
            }
        }

        private static void RegisterUser(OptionListIdentifier id, string? description = null)
        {
            EnsureUnique(id);
            _lists[id] = () => new UserOptionsListViewModel(id, description);
        }

        private static void RegisterSystem<TIdentifier>(OptionListIdentifier id, string? description = null) where TIdentifier : SystemId<TIdentifier>
        {
            EnsureUnique(id);
            _lists[id] = () => new SystemOptionListViewModel<TIdentifier>(id, description);
        }

        private static void RegisterMixed<TIdentifier>(OptionListIdentifier id, string? description = null) where TIdentifier : SystemId<TIdentifier>
        {
            EnsureUnique(id);
            _lists[id] = () => new MixedOptionListViewModel<TIdentifier>(id, description);
        }

        private static void EnsureUnique(OptionListIdentifier id)
        {
            if (_lists.ContainsKey(id))
            {
                throw new ListIdentifierAlreadyRegisteredException(id);
            }
        }

        public static BaseOptionListViewModel Create(OptionListIdentifier identifier)
        {
            if (_lists.TryGetValue(identifier, out var list))
            {
                if (!_addedLists.Contains(identifier))
                {
                    _addedLists.Add(identifier);
                    return list();
                }
                else
                {
                    throw new ListAlreadyExistsException(identifier);
                }
            }

            throw new ListIdentifierNotFoundException(identifier);
        }
    }
}
