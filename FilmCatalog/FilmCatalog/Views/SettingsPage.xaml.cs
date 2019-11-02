using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using FilmCatalog.Models;

namespace FilmCatalog
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class SettingsPage : ContentPage
	{
        public Settings Settings { get; set; }

		public SettingsPage (ref Settings settings)
		{
            InitializeComponent();
            Settings = new Settings();
            Settings = settings;
            this.BindingContext = this;   
            
            /*Settings = new Settings();
            Settings = settings;                        
            ToolbarColorPicker.Color = Settings.ToolbarColor;
            ToolbarTextColorPicker.Color = Settings.ToolbarTextColor;
            Settings = settings;
            BindingContext = this;*/
		}

        private void English(object sender, EventArgs e)
        {
            Localization.Culture = new System.Globalization.CultureInfo("en-US");
            MessagingCenter.Send(this,"Refresh");
            Title = Localization.Settings;
        }

        private void Russian(object sender, EventArgs e)
        {
            Localization.Culture = new System.Globalization.CultureInfo("ru-BY");
            MessagingCenter.Send(this, "Refresh");
            Title = Localization.Settings;
        }

        private void Refresh()
        {
            Title = Localization.Settings;
        }

        private void LanguagePicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (languagePicker.Items[languagePicker.SelectedIndex])
            {
                case "Русский": Localization.Culture = new System.Globalization.CultureInfo("ru-BY"); break;
                case "English": Localization.Culture = new System.Globalization.CultureInfo("en-US"); break;
            }
            MessagingCenter.Send(this, "Refresh");
            MessagingCenter.Send(this, "SaveSettings");
            header.Text = Localization.Language;
            AutosaveLabel.Text = Localization.Autosave;
            Title = Localization.Settings;
        }

        private void SaveSwitch_Toggled(object sender, ToggledEventArgs e)
        {
            MessagingCenter.Send(this, "Autosave", saveSwitch.IsToggled);
            MessagingCenter.Send(this, "SaveSettings");
        }

        /*private void ToolbarColorPicker_PropertyChanging(object sender, PropertyChangingEventArgs e)
        {
            MessagingCenter.Send(this, "ChangeToolbarColor", ToolbarColorPicker.Color);
        }

        private void ToolbarTextColorPicker_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            MessagingCenter.Send(this, "ChangeToolbarTextColor", ToolbarTextColorPicker.Color);
        }*/
    }
}