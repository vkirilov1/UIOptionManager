using Service.Model.Option;
using Service.Others.Identifiers.Model;
using Service.Others.OptionListLoggerDelegates;

namespace Service.Model.OptionList
{
    public class SystemOptionList<T> : BaseOptionList<SystemOption<T>> where T : SystemId<T>
    {
        public SystemOptionList(string name, string? description) : base(name)
        {
            Description = description;
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
    }
}
