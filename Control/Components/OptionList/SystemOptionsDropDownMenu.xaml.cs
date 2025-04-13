using System.Collections;
using System.Windows;
using System.Windows.Controls;

namespace Control.Components.OptionList
{
    public partial class SystemOptionsDropDownMenu : UserControl
    {
        public static readonly DependencyProperty OptionsProperty =
            DependencyProperty.Register(
                nameof(Options),
                typeof(IEnumerable),
                typeof(SystemOptionsDropDownMenu),
                new PropertyMetadata(null));

        public static readonly DependencyProperty SelectedItemProperty =
            DependencyProperty.Register(
                nameof(SelectedItem),
                typeof(object),
                typeof(SystemOptionsDropDownMenu),
                new FrameworkPropertyMetadata(
                    null,
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public static readonly DependencyProperty DisplayMemberPathProperty =
            DependencyProperty.Register(
                nameof(DisplayMemberPath),
                typeof(string),
                typeof(SystemOptionsDropDownMenu),
                new PropertyMetadata("Value"));

        public static readonly DependencyProperty ListNameProperty =
            DependencyProperty.Register(
                nameof(ListName),
                typeof(string),
                typeof(SystemOptionsDropDownMenu),
                new PropertyMetadata(null));

        public static readonly DependencyProperty OptionDescriptionProperty =
            DependencyProperty.Register(
                nameof(OptionDescription),
                typeof(string),
                typeof(SystemOptionsDropDownMenu),
                new PropertyMetadata(null));

        public IEnumerable Options
        {
            get { return (IEnumerable)GetValue(OptionsProperty); }
            set { SetValue(OptionsProperty, value); }
        }

        public object SelectedItem
        {
            get { return GetValue(SelectedItemProperty); }
            set { SetValue(SelectedItemProperty, value); }
        }

        public string DisplayMemberPath
        {
            get { return (string)GetValue(DisplayMemberPathProperty); }
            set { SetValue(DisplayMemberPathProperty, value); }
        }

        public string ListName
        {
            get { return (string)GetValue(ListNameProperty); }
            set { SetValue(ListNameProperty, value); }
        }

        public string OptionDescription
        {
            get { return (string)GetValue(OptionDescriptionProperty); }
            set { SetValue(OptionDescriptionProperty, value); }
        }

        public SystemOptionsDropDownMenu()
        {
            InitializeComponent();
        }
    }
}
