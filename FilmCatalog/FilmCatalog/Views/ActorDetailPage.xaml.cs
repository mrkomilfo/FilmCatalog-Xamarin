using System;
using System.Collections.Generic;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using FilmCatalog.Models;

namespace FilmCatalog
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ActorDetailPage : ContentPage
	{
        public Actor Actor { get; set; }
        public List<Film> Films { get; set; }

		public ActorDetailPage (Actor actor)
		{
			InitializeComponent ();
            Actor = actor;
           
            MessagingCenter.Subscribe<MenuPage, List<Film>>(this, "ActorFilms", (obj, films) =>
            {
                Films = films;
            });
            MessagingCenter.Send(this, "getFilms", Actor.Name);
            this.BindingContext = this;
        }

        public void FilmTapped(object sender, ItemTappedEventArgs e)
        {
            Navigation.PushAsync( new FilmDetailPage(e.Item as Film));
        }

        void DeleteClicked(object sender, EventArgs e)
        {
            MessagingCenter.Send(this, "DeleteActor", Actor.Name);
        }

        async void EditClicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new NavigationPage(new NewActorPage(Actor)));
        }

        private void WikiButton_Clicked(object sender, EventArgs e)
        {
            if (Actor.Wiki != null)
            {
                try
                {
                    Device.OpenUri(new Uri(Actor.Wiki));
                }
                catch (Exception) { }
            }
        }
    }
}