using System.Collections;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Control.Components.OptionList
{
    public partial class UserOptionsDropDownMenu : UserControl
    {
        public static readonly DependencyProperty OptionsProperty =
            DependencyProperty.Register(
                nameof(Options),
                typeof(IEnumerable),
                typeof(UserOptionsDropDownMenu),
                new PropertyMetadata(null));

        public static readonly DependencyProperty SelectedItemProperty =
            DependencyProperty.Register(
                nameof(SelectedItem),
                typeof(object),
                typeof(UserOptionsDropDownMenu),
                new FrameworkPropertyMetadata(
                    null,
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public static readonly DependencyProperty DisplayMemberPathProperty =
            DependencyProperty.Register(
                nameof(DisplayMemberPath),
                typeof(string),
                typeof(UserOptionsDropDownMenu),
                new PropertyMetadata("Value"));

        public static readonly DependencyProperty AddOptionCommandProperty =
            DependencyProperty.Register(
                nameof(AddOptionCommand),
                typeof(ICommand),
                typeof(UserOptionsDropDownMenu));

        public static readonly DependencyProperty DeleteOptionCommandProperty =
            DependencyProperty.Register(
                nameof(DeleteOptionCommand),
                typeof(ICommand),
                typeof(UserOptionsDropDownMenu));

        public static readonly DependencyProperty EditOptionCommandProperty =
            DependencyProperty.Register(
                nameof(EditOptionCommand),
                typeof(ICommand),
                typeof(UserOptionsDropDownMenu));

        public static readonly DependencyProperty NewOptionNameProperty =
            DependencyProperty.Register(
                nameof(NewOptionName),
                typeof(string),
                typeof(UserOptionsDropDownMenu),
                new FrameworkPropertyMetadata(
                    null,
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public static readonly DependencyProperty ListNameProperty =
            DependencyProperty.Register(
                nameof(ListName),
                typeof(string),
                typeof(UserOptionsDropDownMenu),
                new PropertyMetadata(null));

        public static readonly DependencyProperty FilterTextProperty =
            DependencyProperty.Register(
                nameof(FilterText),
                typeof(string),
                typeof(UserOptionsDropDownMenu),
                new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

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

        public string DisplayMemberPath
        {
            get { return (string)GetValue(DisplayMemberPathProperty); }
            set { SetValue(DisplayMemberPathProperty, value); }
        }

        public ICommand AddOptionCommand
        {
            get => (ICommand)GetValue(AddOptionCommandProperty);
            set => SetValue(AddOptionCommandProperty, value);
        }

        public ICommand DeleteOptionCommand
        {
            get => (ICommand)GetValue(DeleteOptionCommandProperty);
            set => SetValue(DeleteOptionCommandProperty, value);
        }

        public ICommand EditOptionCommand
        {
            get => (ICommand)GetValue(EditOptionCommandProperty);
            set => SetValue(EditOptionCommandProperty, value);
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

        public string FilterText
        {
            get => (string)GetValue(FilterTextProperty);
            set => SetValue(FilterTextProperty, value);
        }

        public UserOptionsDropDownMenu()
        {
            InitializeComponent();
        }
    }
}
