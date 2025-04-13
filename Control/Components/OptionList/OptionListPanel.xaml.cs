using Control.ViewModel;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace Control.Components.OptionList
{
    public partial class OptionListPanel : UserControl
    {
        public static readonly DependencyProperty OptionListsProperty =
            DependencyProperty.Register(
                nameof(OptionLists),
                typeof(ObservableCollection<BaseOptionListViewModel>),
                typeof(OptionListPanel),
                new PropertyMetadata(null));

        public ObservableCollection<BaseOptionListViewModel> OptionLists
        {
            get { return (ObservableCollection<BaseOptionListViewModel>)GetValue(OptionListsProperty); }
            set { SetValue(OptionListsProperty, value); }
        }

        public OptionListPanel()
        {
            InitializeComponent();
        }
    }
}
