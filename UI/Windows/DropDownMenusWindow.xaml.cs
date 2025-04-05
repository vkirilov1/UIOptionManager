using Service.Others.Identifiers.Constants;
using System.Windows;
using UI.ViewModel.OptionList;

namespace UI
{
    /// <summary>
    /// Interaction logic for DropDownMenusWindow.xaml
    /// </summary>
    // DropDownMenusWindow.xaml.cs
    public partial class DropDownMenusWindow : Window
    {
        public SystemOptionListViewModel<SystemIdConstants.EmploymentType> EmploymentTypeOptions { get; }

        public MixedOptionListViewModel<SystemIdConstants.WorkLocation> WorkLocationOptions { get; }

        public UserOptionsListViewModel Roles { get; }

        public UserOptionsListViewModel ColorsList { get; }

        public DropDownMenusWindow()
        {
            EmploymentTypeOptions = new("EmploymentTypeList");

            Roles = new("Roles");

            ColorsList = new("Colors");

            WorkLocationOptions = new("WorkLocations");

            InitializeComponent();
            DataContext = this;
        }
    }
}