using DataLayer.Model.Option;
using DataLayer.Model.OptionList;
using Microsoft.EntityFrameworkCore;
using Service.Exceptions;
using Service.Model.Option;
using Service.Others.Factories;
using Service.Others.Identifiers.Model;
using Service.Others.OptionListLoggerDelegates;

namespace Service.Model.OptionList
{
    public class MixedOptionList<T> : BaseOptionList<MixedOption<T>> where T : SystemId<T>
    {
        public MixedOptionList(string name, string? description) : base(name)
        {
            using var dbContext = DatabaseContextFactory.Create();

            var dbEntry = dbContext.MixedOptionLists
                .Include(l => l.Options)
                .FirstOrDefault(l => l.Name == Name);

            var logInf = new ActionOnLog(OLLDelegates.LogInformation);

            if (dbEntry == null)
            {
                if (string.IsNullOrWhiteSpace(Name))
                    throw new EmptyListNameException(GetType().Name);

                Description = description;
                InitializeOptions();

                var newDbEntry = new MixedOptionListDBEntry()
                {
                    Name = Name,
                    Description = Description,
                    SystemIdType = typeof(T).Name,
                    SelectedOption = null
                };

                dbContext.MixedOptionLists.Add(newDbEntry);
                dbContext.SaveChanges();

                //We are relying on the database to set the Id automatically and then use it
                Id = newDbEntry.Id;

                _options.ToList().ForEach(option =>
                dbContext.MixedOptions.Add(new MixedOptionDBEntry()
                {
                    Value = option.Value,
                    SystemId = option.SysId?.Value,
                    MixedOptionListDBEntryId = newDbEntry.Id
                }));

                dbContext.SaveChanges();

                logInf($"Created mixed option list '{Name}' with ID {Id}");
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
                        if (option.SystemId.HasValue)
                        {
                            _options.Add(new MixedOption<T>
                            {
                                SysId = SystemId<T>.FromDatabaseValue(option.SystemId.Value),
                                Value = option.Value
                            });
                        }
                        else
                        {
                            _options.Add(new MixedOption<T>
                            {
                                SysId = null,
                                Value = option.Value
                            });
                        }
                    });

                logInf($"Dataloaded mixed options from System Identifier {typeof(T).Name} to Mixed Option List named {Name}!");
            }
        }

        public void AddUserMixedOptionToList(string value)
        {
            if (string.IsNullOrEmpty(value))
                throw new EmptyUserOptionException(Name);

            if (_options.Any(opt => opt.Value == value))
                throw new OptionAlreadyExistsException(Name, value);

            using var dbContext = DatabaseContextFactory.Create();

            var newOption = new MixedOption<T> { Value = value, SysId = null };
            _options.Add(newOption);

            dbContext.MixedOptions.Add(newOption.ToDbEntry(Id));
            dbContext.SaveChanges();

            var logInf = new ActionOnLog(OLLDelegates.LogInformation);
            logInf($"Adding option {value} to Mixed Option List named '{Name}'!");
        }

        public void UpdateSelectedOption(string selectedOption)
        {
            if (string.IsNullOrEmpty(selectedOption))
                throw new EmptySelectedOptionException(Name);

            using var dbContext = DatabaseContextFactory.Create();

            var dbEntry = dbContext.MixedOptionLists
                .Include(l => l.Options)
                .FirstOrDefault(l => l.Name == Name) ?? throw new DBListNotFoundException(Name);

            var matchingOption = dbEntry.Options.FirstOrDefault(opt => opt.Value == selectedOption)
                ?? throw new OptionDoesNotExistException(selectedOption, Name);

            string matchingOptionValue = matchingOption.Value;

            dbEntry.SelectedOption = matchingOptionValue;

            dbContext.SaveChanges();

            SelectedOption = matchingOptionValue;

            var logInf = new ActionOnLog(OLLDelegates.LogInformation);
            logInf($"Selected option updated to '{selectedOption}' in mixed list '{Name}'");
        }

        private void InitializeOptions()
        {
            SystemId<T>.GetAllValues()
                .ToList()
                .ForEach(sysId =>
                {
                    _options.Add(new MixedOption<T>
                    {
                        SysId = sysId,
                        Value = sysId.Name
                    });
                });

            var logInf = new ActionOnLog(OLLDelegates.LogInformation);
            logInf($"Dataloaded system options from System Identifier {typeof(T).Name} to Mixed Option List named {Name}!");
        }
    }
}