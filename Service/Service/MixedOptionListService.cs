using DataLayer.Model.Option;
using DataLayer.Model.OptionList;
using Microsoft.EntityFrameworkCore;
using Service.Exceptions;
using Service.Model.Option;
using Service.Model.OptionList;
using Service.Others.Factories;
using Service.Others.Identifiers.Model;
using Service.Others.OptionListLoggerDelegates;

namespace Service.Service
{
    public class MixedOptionListService<T> where T : SystemId<T>
    {
        private readonly ActionOnLog _logInf = new ActionOnLog(OLLDelegates.LogInformation);

        public MixedOptionList<T> LoadOrCreate(string name, string? description, int userId)
        {
            using var dbContext = DatabaseContextFactory.Create();

            var dbEntry = dbContext.MixedOptionLists
                .Include(l => l.Options)
                .FirstOrDefault(l => l.Name == name && l.UserDBEntryId == userId);

            if (dbEntry == null)
            {
                return Create(name, description, userId);
            }

            var list = new MixedOptionList<T>(dbEntry.Name, dbEntry.Description, userId)
            {
                Id = dbEntry.Id,
                SelectedOption = dbEntry.SelectedOption
            };

            foreach (var option in dbEntry.Options)
            {
                list.OptionsInternalAdd(new MixedOption<T>
                {
                    Value = option.Value,
                    SysId = option.SystemId.HasValue ? SystemId<T>.FromDatabaseValue(option.SystemId.Value) : null,
                    UserId = userId
                });
            }

            _logInf($"Dataloaded mixed options from {typeof(T).Name} to list '{name}'!");
            return list;
        }

        private MixedOptionList<T> Create(string name, string? description, int userId)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new EmptyListNameException(typeof(MixedOptionList<T>).Name);

            using var dbContext = DatabaseContextFactory.Create();

            var newDbEntry = new MixedOptionListDBEntry
            {
                Name = name,
                Description = description,
                SystemIdType = typeof(T).Name,
                UserDBEntryId = userId
            };

            dbContext.MixedOptionLists.Add(newDbEntry);
            dbContext.SaveChanges();

            var list = new MixedOptionList<T>(name, description, userId) { Id = newDbEntry.Id };
            list.InitializeSystemOptions();

            foreach (var option in list.Options)
            {
                dbContext.MixedOptions.Add(new MixedOptionDBEntry
                {
                    Value = option.Value,
                    SystemId = option.SysId?.Value,
                    MixedOptionListDBEntryId = list.Id,
                    UserDBEntryId = userId
                });
            }
            dbContext.SaveChanges();

            _logInf($"Created mixed option list '{name}' with ID {list.Id}");
            return list;
        }

        public void AddOption(MixedOptionList<T> list, string value)
        {
            list.AddUserMixedOption(value);

            using var dbContext = DatabaseContextFactory.Create();
            var option = list.Options.First(o => o.Value == value);

            dbContext.MixedOptions.Add(option.ToDbEntry(list.Id));
            dbContext.SaveChanges();

            _logInf($"Adding option {value} to list '{list.Name}'!");
        }

        public void UpdateSelectedOption(MixedOptionList<T> list, string selectedOption)
        {
            list.ValidateSelection(selectedOption);

            using var dbContext = DatabaseContextFactory.Create();
            var dbEntry = dbContext.MixedOptionLists.FirstOrDefault(l => l.Id == list.Id)
                ?? throw new DBListNotFoundException(list.Name);

            dbEntry.SelectedOption = selectedOption;
            dbContext.SaveChanges();

            list.SelectedOption = selectedOption;
            _logInf($"Selected option updated to '{selectedOption}' in list '{list.Name}'");
        }

        public void DeleteOption(MixedOptionList<T> list, string value)
        {
            using var dbContext = DatabaseContextFactory.Create();
            var dbOption = dbContext.MixedOptions.FirstOrDefault(o => o.MixedOptionListDBEntryId == list.Id && o.Value == value)
                ?? throw new OptionDoesNotExistException(value, list.Name);

            dbContext.MixedOptions.Remove(dbOption);
            dbContext.SaveChanges();

            list.RemoveOptionLocal(value);
        }

        public void EditOption(MixedOptionList<T> list, string oldValue, string newValue)
        {
            list.ValidateEdit(oldValue, newValue);

            using var dbContext = DatabaseContextFactory.Create();
            var dbOption = dbContext.MixedOptions.FirstOrDefault(o => o.MixedOptionListDBEntryId == list.Id && o.Value == oldValue)
                ?? throw new OptionDoesNotExistException(oldValue, list.Name);

            dbOption.Value = newValue;
            dbContext.SaveChanges();

            list.UpdateOptionLocal(oldValue, newValue);
        }
    }
}