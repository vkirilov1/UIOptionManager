using DataLayer.Model.OptionList;
using Service.Exceptions;
using Service.Model.Option;
using Service.Others.Factories;
using Service.Others.Identifiers.Model;
using Service.Others.OptionListLoggerDelegates;

namespace Service.Model.OptionList
{
    public class SystemOptionList<T> : BaseOptionList<SystemOption<T>> where T : SystemId<T>
    {
        public SystemOptionList(string name, string? description, int userId) : base(name, userId)
        {
            using var dbContext = DatabaseContextFactory.Create();

            var dbEntry = dbContext.SystemOptionLists
                .FirstOrDefault(l => l.Name == Name && l.UserDBEntryId == UserId);

            var logInf = new ActionOnLog(OLLDelegates.LogInformation);

            if (dbEntry == null)
            {
                if (string.IsNullOrWhiteSpace(Name))
                    throw new EmptyListNameException(GetType().Name);

                Description = description;

                var newDbEntry = new SystemOptionListDBEntry()
                {
                    Name = Name,
                    Description = Description,
                    SelectedOption = null,
                    UserDBEntryId = UserId
                };

                dbContext.SystemOptionLists.Add(newDbEntry);
                dbContext.SaveChanges();

                //We are relying on the database to set the Id automatically and then use it
                Id = newDbEntry.Id;

                dbContext.SaveChanges();

                logInf($"Created system option list '{Name}' with ID {Id}");
            }
            else
            {
                Id = dbEntry.Id;
                Name = dbEntry.Name;
                Description = dbEntry.Description;
                SelectedOption = dbEntry.SelectedOption;
                UserId = dbEntry.UserDBEntryId;
            }

            InitializeOptions();
        }

        private void InitializeOptions()
        {
            SystemId<T>.GetAllValues()
                .ToList()
                .ForEach(sysId =>
                {
                    _options.Add(new SystemOption<T>
                    {
                        SysId = sysId,
                        Value = sysId.Name
                    });
                });

            var logInf = new ActionOnLog(OLLDelegates.LogInformation);
            logInf($"Dataloaded system options from System Identifier {typeof(T).Name} to System Option List named {Name}!");
        }

        public void UpdateSelectedOption(string selectedOption)
        {
            if (string.IsNullOrEmpty(selectedOption))
                throw new EmptySelectedOptionException(Name);

            using var dbContext = DatabaseContextFactory.Create();

            var dbEntry = dbContext.SystemOptionLists
                .FirstOrDefault(l => l.Name == Name && l.UserDBEntryId == UserId)
                ?? throw new DBListNotFoundException(Name);

            var matchingOption = Options.FirstOrDefault(opt => opt.Value == selectedOption);

            if (matchingOption == null)
                throw new OptionDoesNotExistException(selectedOption, Name);

            string matchingOptionValue = matchingOption.Value;

            dbEntry.SelectedOption = matchingOptionValue;

            dbContext.SaveChanges();

            SelectedOption = matchingOptionValue;

            var logInf = new ActionOnLog(OLLDelegates.LogInformation);
            logInf($"Selected option updated to '{selectedOption}' in system list '{Name}'");
        }
    }
}
