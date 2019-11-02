using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using FilmCatalog.Models;

namespace FilmCatalog
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class NewActorPage : ContentPage
	{
        Actor newActor = new Actor();
        string oldName;
        bool format;

        public NewActorPage()
		{
			InitializeComponent();
            format = true;
		}

        public NewActorPage(Actor actor)
        {
            InitializeComponent();
            nameEntry.Text = actor.Name;
            birthDatePicker.Date = actor.Birthday;
            biographyEditor.Text = actor.Biography;
            oldName = actor.Name;
            wikiEntry.Text = actor.Wiki;
            format = false;
        }

        public NewActorPage(string autoActor)
        {
            InitializeComponent();
            nameEntry.Text = autoActor;
            format = true;
        }

        async void Save_Clicked(object sender, EventArgs e)
        {
            newActor.Name = nameEntry.Text;
            newActor.Birthday = birthDatePicker.Date;
            newActor.Biography = biographyEditor.Text;
            newActor.Wiki = wikiEntry.Text;

            if (format == true)
                MessagingCenter.Send(this, "AddActor", newActor);
            else
                MessagingCenter.Send(this, "EditActor", (newActor, oldName));

            await Navigation.PopModalAsync();
        }

        async void Cancel_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }
    }
}