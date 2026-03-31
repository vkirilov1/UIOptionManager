using DataLayer.Model.OptionList;
using Microsoft.EntityFrameworkCore;
using Service.Exceptions;
using Service.Model.Option;
using Service.Model.OptionList;
using Service.Others.Factories;
using Service.Others.OptionListLoggerDelegates;

namespace Service.Service
{
    public class UserOptionListService
    {
        private readonly ActionOnLog _logInf = new ActionOnLog(OLLDelegates.LogInformation);

        public UserOptionList LoadOrCreate(string name, string? description, int userId)
        {
            using var dbContext = DatabaseContextFactory.Create();

            var dbEntry = dbContext.UserOptionLists
                .Include(l => l.Options)
                .FirstOrDefault(l => l.Name == name && l.UserDBEntryId == userId);

            if (dbEntry == null)
                return Create(name, description, userId);

            var list = new UserOptionList(dbEntry.Name, dbEntry.Description, userId)
            {
                Id = dbEntry.Id,
                SelectedOption = dbEntry.SelectedOption
            };

            foreach (var opt in dbEntry.Options)
            {
                list.InternalAddOption(new UserOption { Value = opt.Value, UserId = userId });
            }

            _logInf($"Dataloaded user options to list '{name}'!");
            return list;
        }

        private UserOptionList Create(string name, string? description, int userId)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new EmptyListNameException(nameof(UserOptionList));

            using var dbContext = DatabaseContextFactory.Create();

            var newDbEntry = new UserOptionListDBEntry
            {
                Name = name,
                Description = description,
                UserDBEntryId = userId
            };

            dbContext.UserOptionLists.Add(newDbEntry);
            dbContext.SaveChanges();

            _logInf($"Created user option list '{name}' with ID {newDbEntry.Id}");
            return new UserOptionList(name, description, userId) { Id = newDbEntry.Id };
        }

        public void AddOption(UserOptionList list, string value)
        {
            list.AddOption(value);

            using var dbContext = DatabaseContextFactory.Create();
            var option = list.Options.First(o => o.Value == value);

            dbContext.UserOptions.Add(option.ToDbEntry(list.Id));
            dbContext.SaveChanges();

            _logInf($"Adding option {value} to list '{list.Name}'!");
        }

        public void UpdateSelectedOption(UserOptionList list, string selectedOption)
        {
            list.ValidateSelection(selectedOption);

            using var dbContext = DatabaseContextFactory.Create();
            var dbEntry = dbContext.UserOptionLists.FirstOrDefault(l => l.Id == list.Id)
                ?? throw new DBListNotFoundException(list.Name);

            dbEntry.SelectedOption = selectedOption;
            dbContext.SaveChanges();

            list.SelectedOption = selectedOption;
            _logInf($"Selected option updated to '{selectedOption}' in user list '{list.Name}'");
        }

        public void DeleteOption(UserOptionList list, string value)
        {
            using var dbContext = DatabaseContextFactory.Create();
            var dbOption = dbContext.UserOptions
                .FirstOrDefault(o => o.UserOptionListDBEntryId == list.Id && o.Value == value)
                ?? throw new OptionDoesNotExistException(value, list.Name);

            dbContext.UserOptions.Remove(dbOption);
            dbContext.SaveChanges();

            list.RemoveOptionLocal(value);
        }

        public void EditOption(UserOptionList list, string oldValue, string newValue)
        {
            list.ValidateValue(newValue);

            using var dbContext = DatabaseContextFactory.Create();
            var dbOption = dbContext.UserOptions
                .FirstOrDefault(o => o.UserOptionListDBEntryId == list.Id && o.Value == oldValue)
                ?? throw new OptionDoesNotExistException(oldValue, list.Name);

            dbOption.Value = newValue;
            dbContext.SaveChanges();

            list.UpdateOptionLocal(oldValue, newValue);
        }
    }
}