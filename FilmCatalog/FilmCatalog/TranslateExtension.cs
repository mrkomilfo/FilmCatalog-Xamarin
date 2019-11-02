using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Globalization;
using FilmCatalog.Models;

namespace FilmCatalog
{
    [ContentProperty("Text")]
    public class TranslateExtension : IMarkupExtension
    {
        static TranslateExtension()
        {
            try
            {
                Localization.Culture = new CultureInfo(Settings.Load().Language);
            }
            catch (Exception)
            {
                Localization.Culture = DependencyService.Get<ILocalize>().GetCurrentCultureInfo();
            }
        }

        public string Text { get; set; }

        public object ProvideValue(IServiceProvider serviceProvider)
        {
            if (Text == null) return "";

            string translation = Localization.ResourceManager.GetString(Text, Localization.Culture);

            if (translation == null) translation = Text;

            return translation;
        }
    }
}
