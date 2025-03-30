using System.Collections;
using System.Windows;
using System.Windows.Controls;

namespace UI.Components
{
    public partial class SystemOptionsDropDownMenu : UserControl
    {
        public static readonly DependencyProperty OptionsProperty =
            DependencyProperty.Register(
                "Options",
                typeof(IEnumerable),
                typeof(SystemOptionsDropDownMenu),
                new PropertyMetadata(null));

        public static readonly DependencyProperty SelectedItemProperty =
            DependencyProperty.Register(
                "SelectedItem",
                typeof(object),
                typeof(SystemOptionsDropDownMenu),
                new FrameworkPropertyMetadata(
                    null,
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public static readonly DependencyProperty DisplayMemberPathProperty =
            DependencyProperty.Register(
                "DisplayMemberPath",
                typeof(string),
                typeof(SystemOptionsDropDownMenu),
                new PropertyMetadata("Value"));

        public static readonly DependencyProperty ListNameProperty =
            DependencyProperty.Register(
                "ListName",
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

        public SystemOptionsDropDownMenu()
        {
            InitializeComponent();
        }
    }
}
