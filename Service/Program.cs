using DataLayer.Database;
using Microsoft.EntityFrameworkCore;
using Service.Model.OptionList;
using Service.Others.Identifiers.Constants;
using Service.Others.OptionListLoggerDelegates;
using System.Transactions;

namespace Service
{
    internal class Program
    {

        static void Main(string[] args)
        {
            using var dbContext = new DatabaseContext();

            dbContext.Database.EnsureCreated();

            var employmentTypeList = new SystemOptionList<SystemIdConstants.EmploymentType>("EmploymentTypeList", "Represents the types of employment.");

            var workLocationListDBEntry = dbContext.MixedOptionLists
                .Include(l => l.Options)
                .FirstOrDefault(l => l.Name == "WorkLocations");

            var workLocationList = new MixedOptionList<SystemIdConstants.WorkLocation>(workLocationListDBEntry);

            var rolesListDBEntry = dbContext.UserOptionLists
                .Include(l => l.Options)
                .FirstOrDefault(l => l.Name == "Roles");

            var rolesList = new UserOptionList(rolesListDBEntry);

            try
            {
                for (int i = 0; i < 4; i++)
                {
                    Console.WriteLine($"Enter work location {i + 1}:");

                    string? workLocationValue = Console.ReadLine()?.Trim();

                    workLocationList.AddUserMixedOptionToList(workLocationValue, dbContext);
                }

                for (int i = 0; i < 4; i++)
                {
                    Console.WriteLine($"Enter user option {i + 1}:");

                    string? userOption = Console.ReadLine()?.Trim();

                    rolesList.AddUserDefinedOption(userOption, dbContext);
                }
            }
            catch (Exception e)
            {
                var logExc = new ActionOnLog(OLLDelegates.LogError);
                logExc(e.Message);
            }

            foreach (var o in workLocationList.Options)
            {
                String modifierName = o.SysId == null ? "null" : o.SysId.Name;
                String modifierId = o.SysId == null ? "null" : o.SysId.Value.ToString();
                Console.WriteLine(o.Id + ";" + o.Value + ";" + modifierName + ";" + modifierId);
            }

            foreach (var o in rolesList.Options)
            {
                Console.WriteLine(o.Id + ";" + o.Value + ";");
            }

            foreach (var o in employmentTypeList.Options)
            {
                String modifierName = o.SysId == null ? "null" : o.SysId.Name;
                String modifierId = o.SysId == null ? "null" : o.SysId.Value.ToString();
                Console.WriteLine(o.Value + ";" + modifierName + ";" + modifierId);
            }
        }
    }
}
