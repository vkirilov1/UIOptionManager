using System.Windows;
using System.Windows.Controls;
using Control.ViewModel.OptionList;

namespace Control.Others.Templates
{
    public class OptionListTemplateSelector : DataTemplateSelector
    {
        public required DataTemplate SystemOptionListTemplate { get; set; }
        public required DataTemplate UserAndMixedOptionListTemplate { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item == null) return base.SelectTemplate(item, container);

            var itemType = item.GetType();

            if(itemType.IsGenericType)
            {
                var genericList = itemType.GetGenericTypeDefinition();

                if (genericList == typeof(SystemOptionListViewModel<>))
                {
                    return SystemOptionListTemplate;
                }

                if (genericList == typeof(MixedOptionListViewModel<>))
                {
                    return UserAndMixedOptionListTemplate;
                }
            }

            if(item is UserOptionsListViewModel)
            {
                return UserAndMixedOptionListTemplate;
            }

            return base.SelectTemplate(item, container);
        }
    }
}
