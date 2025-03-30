using Service.Model.Option;
using Service.Exceptions;
using Service.Others.OptionListLoggerDelegates;
using Microsoft.EntityFrameworkCore;
using DataLayer.Model.OptionList;
using Service.Others.Factories;

namespace Service.Model.OptionList
{
    public class UserOptionList : BaseOptionList<UserOption>
    {
        public int Id { get; set; }

        public UserOptionList(string name, string? description) : base(name)
        {
            using var dbContext = DatabaseContextFactory.Create();

            var dbEntry = dbContext.UserOptionLists
                .Include(l => l.Options)
                .FirstOrDefault(l => l.Name == Name);

            var logInf = new ActionOnLog(OLLDelegates.LogInformation);

            if (dbEntry == null)
            {

                if (string.IsNullOrWhiteSpace(Name))
                    throw new EmptyListNameException(GetType().Name);

                Description = description;

                var newDbEntry = new UserOptionListDBEntry()
                {
                    Name = Name,
                    Description = Description
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

                dbEntry.Options
                    .ToList()
                    .ForEach(option =>
                    {
                        _options.Add(new UserOption
                        {
                            Value = option.Value
                        });
                    });

                logInf($"Dataloaded user options to User Option List named {Name}!");
            }
        }

        public void AddUserDefinedOption(string value)
        {
            using var dbContext = DatabaseContextFactory.Create();

            if (string.IsNullOrEmpty(value))
                throw new EmptyUserOptionException(Name);

            if (_options.Any(opt => opt.Value == value))
                throw new OptionAlreadyExistsException(Name, value);

            var newOption = new UserOption { Value = value };
            _options.Add(newOption);

            dbContext.UserOptions.Add(newOption.ToDbEntry(Id));
            dbContext.SaveChanges();

            var logInf = new ActionOnLog(OLLDelegates.LogInformation);
            logInf($"Adding option {value} to User Option List named '{Name}'!");
        }
    }
}
