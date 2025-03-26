using DataLayer.Database;
using DataLayer.Model.OptionList;
using Service.Exceptions;
using Service.Model.Option;
using Service.Others.Identifiers.Model;
using Service.Others.OptionListLoggerDelegates;

namespace Service.Model.OptionList
{
    public class MixedOptionList<T> : BaseOptionList<MixedOption<T>> where T : SystemId<T>
    {
        public int Id { get; set; }
        public MixedOptionList(MixedOptionListDBEntry dbEntry)
        {
            try
            {
                if (dbEntry == null)
                {
                    throw new TableNotFoundException(nameof(MixedOptionList<T>));
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

                    var logInf = new ActionOnLog(OLLDelegates.LogInformation);
                    logInf($"Dataloaded mixed options from System Identifier {typeof(T).Name} to Mixed Option List named {Name}!");
                }
            }
            catch (Exception e)
            {
                var logExc = new ActionOnLog(OLLDelegates.LogError);
                logExc(e.Message);
            }
        }

        public void AddUserMixedOptionToList(string value, DatabaseContext dbContext)
        {
            try
            {
                if (String.IsNullOrEmpty(value))
                {
                    throw new EmptyUserOptionException(Name);
                }
                else if (Options.Where(option => option.Value == value).FirstOrDefault() != null)
                {
                    throw new OptionAlreadyExistsException(Name, value);
                }
                else
                {
                    MixedOption<T> newOption = new()
                    {
                        Value = value,
                        SysId = null
                    };

                    _options.Add(newOption);

                    var dbEntry = newOption.ToDbEntry(Id);

                    dbContext.MixedOptions.Add(dbEntry);
                    dbContext.SaveChanges();

                    var logInf = new ActionOnLog(OLLDelegates.LogInformation);
                    logInf($"Adding option {value} to Mixed Option List named {Name}!");
                }
            }
            catch (Exception e)
            {
                var logExc = new ActionOnLog(OLLDelegates.LogError);
                logExc(e.Message);
            }
        }
    }
}
