using System;
namespace LocationUpdates.TemplateSelectors
{
    public class LocationUpdatesTemplateSelector : DataTemplateSelector
    {
        public DataTemplate LocationTemplate { get; set; }
        public DataTemplate StatusTemplate { get; set; }

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
            => item is Models.LocationModel ? LocationTemplate : StatusTemplate;
    }
}

