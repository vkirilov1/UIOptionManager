using System.Collections;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Control.Components
{
    public partial class UserOptionsDropDownMenu : UserControl
    {
        public static readonly DependencyProperty OptionsProperty =
            DependencyProperty.Register(
                "Options",
                typeof(IEnumerable),
                typeof(UserOptionsDropDownMenu),
                new PropertyMetadata(null));

        public static readonly DependencyProperty SelectedItemProperty =
            DependencyProperty.Register(
                "SelectedItem",
                typeof(object),
                typeof(UserOptionsDropDownMenu),
                new FrameworkPropertyMetadata(
                    null,
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public static readonly DependencyProperty AddOptionCommandProperty =
            DependencyProperty.Register(
                "AddOptionCommand",
                typeof(ICommand),
                typeof(UserOptionsDropDownMenu));

        public static readonly DependencyProperty NewOptionNameProperty =
            DependencyProperty.Register(
                "NewOptionName",
                typeof(string),
                typeof(UserOptionsDropDownMenu),
                new FrameworkPropertyMetadata(
                    null,
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public static readonly DependencyProperty ListNameProperty =
            DependencyProperty.Register(
                "ListName",
                typeof(string),
                typeof(UserOptionsDropDownMenu),
                new PropertyMetadata(null));

        public IEnumerable Options
        {
            get => (IEnumerable)GetValue(OptionsProperty);
            set => SetValue(OptionsProperty, value);
        }

        public object SelectedItem
        {
            get => GetValue(SelectedItemProperty);
            set => SetValue(SelectedItemProperty, value);
        }

        public ICommand AddOptionCommand
        {
            get => (ICommand)GetValue(AddOptionCommandProperty);
            set => SetValue(AddOptionCommandProperty, value);
        }

        public string NewOptionName
        {
            get => (string)GetValue(NewOptionNameProperty);
            set => SetValue(NewOptionNameProperty, value);
        }

        public string ListName
        {
            get { return (string)GetValue(ListNameProperty); }
            set { SetValue(ListNameProperty, value); }
        }

        public UserOptionsDropDownMenu()
        {
            InitializeComponent();
        }
    }
}
