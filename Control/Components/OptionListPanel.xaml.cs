using Control.ViewModel;
using System.Windows;
using System.Windows.Controls;

namespace Control.Components
{
    public partial class OptionListPanel : UserControl
    {
        public static readonly DependencyProperty OptionListsProperty =
            DependencyProperty.Register(
                "OptionLists",
                typeof(HashSet<BaseOptionListViewModel>),
                typeof(OptionListPanel),
                new PropertyMetadata(null));

        public HashSet<BaseOptionListViewModel> OptionLists
        {
            get { return (HashSet<BaseOptionListViewModel>)GetValue(OptionListsProperty); }
            set { SetValue(OptionListsProperty, value); }
        }

        public OptionListPanel()
        {
            InitializeComponent();
        }
    }
}
