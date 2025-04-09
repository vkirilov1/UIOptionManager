using Control.Others.Constants;
using Control.Others.Factories;
using Control.ViewModel;
using Service.Others.Identifiers.Constants;
using Service.Others.OptionListLoggerDelegates;
using System.Windows;

namespace Control
{
    /// <summary>
    /// Interaction logic for DropDownMenusWindow.xaml
    /// </summary>
    // DropDownMenusWindow.xaml.cs
    public partial class DropDownMenusWindow : Window
    {
        public HashSet<BaseOptionListViewModel> OptionLists { get; } = [];

        public DropDownMenusWindow()
        {
            InitializeComponent();

            try
            {
                OptionLists.Add(OptionListFactory.CreateSystemOptionList<SystemIdConstants.EmploymentType>(OptionListIdentifier.EmploymentType));
                OptionLists.Add(OptionListFactory.CreateUserOptionList(OptionListIdentifier.Roles));
                OptionLists.Add(OptionListFactory.CreateUserOptionList(OptionListIdentifier.Hobbies));
                OptionLists.Add(OptionListFactory.CreateMixedOptionList<SystemIdConstants.WorkLocation>(OptionListIdentifier.WorkLocations));
            }
            catch (Exception e)
            {
                var logExc = new ActionOnLog(OLLDelegates.LogError);
                logExc(e.Message);
            }

            DataContext = this;
        }
    }
}