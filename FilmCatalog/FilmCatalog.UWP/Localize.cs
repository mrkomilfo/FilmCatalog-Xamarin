using System.Globalization;
using Xamarin.Forms;
[assembly: Dependency(typeof(FilmCatalog.UWP.Localize))]
namespace FilmCatalog.UWP
{
    public class Localize : ILocalize
    {
        public System.Globalization.CultureInfo GetCurrentCultureInfo()
        {
            return CultureInfo.CurrentUICulture;
        }
    }
}
