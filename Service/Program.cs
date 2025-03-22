using Service.JobEnums;
using Service.Model.OptionList;
using Service.Others.OptionListLoggerDelegates;

namespace Service
{
    internal class Program
    {

        static void Main(string[] args)
        {
            MixedOptionList<WorkLocationSysIdsEnum> workLocationList = new();
            SystemOptionList<EmploymentTypeSysIdsEnum> employmentTypeList = new();
            UserOptionList userOptionList = new();

            try
            {
                for (int i = 0; i < 4; i++)
                {
                    Console.WriteLine($"Enter work location {i + 1}:");

                    string? workLocationValue = Console.ReadLine()?.Trim();

                    workLocationList.AddUserDefinedOption(workLocationValue);
                }

                for (int i = 0; i < 2; i++)
                {
                    Console.WriteLine($"Enter user option {i + 1}:");

                    string? userOption = Console.ReadLine()?.Trim();

                    userOptionList.AddUserDefinedOption(userOption);
                }
            }
            catch (Exception e)
            {
                var logExc = new ActionOnLog(OLLDelegates.LogError);
                logExc(e.Message);
            }

            foreach (var item in workLocationList.Options)
            {
                Console.WriteLine(item.Id + ";" + item.Value + ";" + item.SystemId);
            }
            foreach (var item in employmentTypeList.Options)
            {
                Console.WriteLine(item.Id + ";" + item.Value + ";" + item.SystemId);
            }
            foreach (var item in userOptionList.Options)
            {
                Console.WriteLine(item.Id + ";" + item.Value);
            }
        }
    }
}
