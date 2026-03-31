using DataLayer.Model.OptionList;
using Service.Exceptions;
using Service.Model.OptionList;
using Service.Others.Factories;
using Service.Others.Identifiers.Model;
using Service.Others.OptionListLoggerDelegates;

namespace Service.Service
{
    public class SystemOptionListService<T> where T : SystemId<T>
    {
        private readonly ActionOnLog _logInf = new ActionOnLog(OLLDelegates.LogInformation);

        public SystemOptionList<T> LoadOrCreate(string name, string? description, int userId)
        {
            using var dbContext = DatabaseContextFactory.Create();

            var dbEntry = dbContext.SystemOptionLists
                .FirstOrDefault(l => l.Name == name && l.UserDBEntryId == userId);

            if (dbEntry == null)
            {
                return Create(name, description, userId);
            }

            var list = new SystemOptionList<T>(dbEntry.Name, dbEntry.Description, userId)
            {
                Id = dbEntry.Id,
                SelectedOption = dbEntry.SelectedOption
            };

            _logInf($"Dataloaded system options via Reflection for type {typeof(T).Name} into list '{name}'!");
            return list;
        }

        private SystemOptionList<T> Create(string name, string? description, int userId)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new EmptyListNameException(typeof(SystemOptionList<T>).Name);

            using var dbContext = DatabaseContextFactory.Create();

            var newDbEntry = new SystemOptionListDBEntry()
            {
                Name = name,
                Description = description,
                SelectedOption = null,
                UserDBEntryId = userId
            };

            dbContext.SystemOptionLists.Add(newDbEntry);
            dbContext.SaveChanges();

            var list = new SystemOptionList<T>(name, description, userId) { Id = newDbEntry.Id };

            _logInf($"Created new system list record '{name}' with ID {list.Id}. Values loaded from code.");
            return list;
        }

        public void UpdateSelectedOption(SystemOptionList<T> list, string selectedOption)
        {
            list.ValidateSelection(selectedOption);

            using var dbContext = DatabaseContextFactory.Create();
            var dbEntry = dbContext.SystemOptionLists.FirstOrDefault(l => l.Id == list.Id)
                ?? throw new DBListNotFoundException(list.Name);

            dbEntry.SelectedOption = selectedOption;
            dbContext.SaveChanges();

            list.SelectedOption = selectedOption;
            _logInf($"Selection '{selectedOption}' saved to DB for system list '{list.Name}'");
        }
    }
}