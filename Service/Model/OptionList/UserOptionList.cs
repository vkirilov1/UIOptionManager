using DataLayer.Model.OptionList;
using Microsoft.EntityFrameworkCore;
using Service.Exceptions;
using Service.Model.Option;
using Service.Others.Factories;
using Service.Others.OptionListLoggerDelegates;

namespace Service.Model.OptionList
{
    public class UserOptionList : BaseOptionList<UserOption>
    {
        public UserOptionList(string name, string? description, int userId) : base(name, userId)
        {
            using var dbContext = DatabaseContextFactory.Create();

            var dbEntry = dbContext.UserOptionLists
                .Include(l => l.Options)
                .FirstOrDefault(l => l.Name == Name && l.UserDBEntryId == UserId);

            var logInf = new ActionOnLog(OLLDelegates.LogInformation);

            if (dbEntry == null)
            {

                if (string.IsNullOrWhiteSpace(Name))
                    throw new EmptyListNameException(GetType().Name);

                Description = description;

                var newDbEntry = new UserOptionListDBEntry()
                {
                    Name = Name,
                    Description = Description,
                    SelectedOption = null,
                    UserDBEntryId = UserId
                };

                dbContext.UserOptionLists.Add(newDbEntry);
                dbContext.SaveChanges();
                Id = newDbEntry.Id;
                logInf($"Created user option list '{Name}' with ID {Id}");
            }
            else
            {
                Id = dbEntry.Id;
                Name = dbEntry.Name;
                Description = dbEntry.Description;
                SelectedOption = dbEntry.SelectedOption;

                dbEntry.Options
                    .ToList()
                    .ForEach(option =>
                    {
                        _options.Add(new UserOption
                        {
                            Value = option.Value,
                            UserId = UserId
                        });
                    });

                logInf($"Dataloaded user options to User Option List named {Name}!");
            }
        }

        public void AddUserDefinedOption(string value)
        {
            if (string.IsNullOrEmpty(value))
                throw new EmptyUserOptionException(Name);

            if (_options.Any(opt => opt.Value == value))
                throw new OptionAlreadyExistsException(Name, value);

            using var dbContext = DatabaseContextFactory.Create();

            var newOption = new UserOption { Value = value, UserId = UserId };
            _options.Add(newOption);

            dbContext.UserOptions.Add(newOption.ToDbEntry(Id));
            dbContext.SaveChanges();

            var logInf = new ActionOnLog(OLLDelegates.LogInformation);
            logInf($"Adding option {value} to User Option List named '{Name}'!");
        }

        public void UpdateSelectedOption(string selectedOption)
        {
            if (string.IsNullOrEmpty(selectedOption))
                throw new EmptySelectedOptionException(Name);

            using var dbContext = DatabaseContextFactory.Create();

            var dbEntry = dbContext.UserOptionLists
                .Include(l => l.Options)
                .FirstOrDefault(l => l.Name == Name && l.UserDBEntryId == UserId)
                ?? throw new DBListNotFoundException(Name);

            var matchingOption = dbEntry.Options.FirstOrDefault(opt => opt.Value == selectedOption)
                ?? throw new OptionDoesNotExistException(selectedOption, Name);

            string matchingOptionValue = matchingOption.Value;

            dbEntry.SelectedOption = matchingOptionValue;

            dbContext.SaveChanges();

            SelectedOption = matchingOptionValue;

            var logInf = new ActionOnLog(OLLDelegates.LogInformation);
            logInf($"Selected option updated to '{selectedOption}' in user list '{Name}'");
        }
    }
}
