using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using FilmCatalog.Models;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace FilmCatalog
{
    public partial class App : Application
    {
        public Settings settings = new Settings();
        public App()
        {
            InitializeComponent();
            
            /*settings.ToolbarColor = (Color)Resources["NavigationPrimary"];
            settings.ToolbarTextColor = (Color)Resources["NavigationSecondary"];

            MessagingCenter.Subscribe<SettingsPage, Color>(this, "ChangeToolbarColor", (obj, color) =>
            {
                Resources["NavigationPrimary"] = color;
                settings.ToolbarColor = color;
            });
            MessagingCenter.Subscribe<SettingsPage, Color>(this, "ChangeToolbarTextColor", (obj, color) =>
            {
                Resources["NavigationSecondary"] = color;
                settings.ToolbarTextColor = color;
            });            */
            MainPage = new MenuPage(settings);
        }
    }
}
