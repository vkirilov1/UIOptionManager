using Service.Model.Option;
using Service.Exceptions;
using Service.Others.OptionListLoggerDelegates;
using DataLayer.Model.OptionList;
using Microsoft.EntityFrameworkCore;
using DataLayer.Database;

namespace Service.Model.OptionList
{
    public class UserOptionList : BaseOptionList<UserOption>
    {
        public virtual int Id { get; set; }

        public UserOptionList(UserOptionListDBEntry dbEntry)
        {
            try
            {
                if (dbEntry == null)
                {
                    throw new TableNotFoundException(nameof(UserOptionList));
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

                    var logInf = new ActionOnLog(OLLDelegates.LogInformation);
                    logInf($"Dataloaded user options to User Option List named {Name}!");
                }
            }
            catch (Exception e)
            {
                var logExc = new ActionOnLog(OLLDelegates.LogError);
                logExc(e.Message);
            }
        }

        public void AddUserDefinedOption(string value, DatabaseContext dbContext)
        {
            try
            {
                if(String.IsNullOrEmpty(value))
                {
                    throw new EmptyUserOptionException(Name);
                }
                else if (Options.Where(option => option.Value == value).FirstOrDefault() != null)
                {
                    throw new OptionAlreadyExistsException(Name, value);
                }
                else
                {
                    UserOption newOption = new()
                    {
                        Value = value
                    };

                    _options.Add(newOption);

                    var dbEntry = newOption.ToDbEntry(Id);

                    dbContext.UserOptions.Add(dbEntry);
                    dbContext.SaveChanges();

                    var logInf = new ActionOnLog(OLLDelegates.LogInformation);
                    logInf($"Adding option {value} to User Option List named {Name}!");
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
