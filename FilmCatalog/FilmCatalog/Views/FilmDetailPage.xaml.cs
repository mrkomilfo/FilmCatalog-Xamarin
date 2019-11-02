using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using FilmCatalog.Models;

namespace FilmCatalog
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class FilmDetailPage : ContentPage
	{
        Film Film { get; set; }
		public FilmDetailPage (Film film)
		{
            InitializeComponent();            
            Film = film;
            if (Film.Poster != null)
            {
                Poster.Source = ImageSource.FromFile(Film.Poster);
            }
            if (film.Favorite == true)
            {
                favouriteButton.Text = Localization.Favourite;
                favouriteButton.BackgroundColor = Color.LimeGreen;
                favouriteButton.TextColor = Color.White;
            }
            else
            {
                favouriteButton.Text = Localization.ToFavourites;
                favouriteButton.BackgroundColor = Color.LightGray;
                favouriteButton.TextColor = Color.Black;
            }
            BindingContext = Film;
            
            MessagingCenter.Subscribe<MenuPage, Producer>(this, "Producer", (obj, producer) =>
            {
                if (producer!=null)
                    Navigation.PushAsync(new ProducerDetailPage(producer));
            });
            MessagingCenter.Subscribe<MenuPage, Actor>(this, "Actor", (obj, actor) =>
            {
                if (actor != null)
                    Navigation.PushAsync(new ActorDetailPage(actor));
            });
        }

        void ProducerTapped(object sender, TappedEventArgs args)
        {
            MessagingCenter.Send(this, "SearchProducer", Film.Producer);
        }

        void ActorSelected(object sender, SelectedItemChangedEventArgs args)
        {
            if (((ListView)sender).SelectedItem == null)
                return;
            MessagingCenter.Send(this, "SearchActor", args.SelectedItem.ToString());
        }

        void AgeLimitTapped(object sender, TappedEventArgs args)
        {
            MessagingCenter.Send(this, "Age", Film.AgeLimit);
        }

        void DeleteClicked(object sender, EventArgs e)
        {
            MessagingCenter.Send(this, "DeleteFilm", Film.Name);
        }

        async void EditClicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new NavigationPage(new NewFilmPage(Film)));
        }

        protected override void OnDisappearing()
        {
            if (actorsListView.SelectedItem != null)
                actorsListView.SelectedItem = null;
        }

        private void FavouriteButton_Clicked(object sender, EventArgs e)
        {
            MessagingCenter.Send(this, "Favourite", Film.Name);
            if (favouriteButton.BackgroundColor == Color.LimeGreen)
            {
                favouriteButton.Text = Localization.ToFavourites;
                favouriteButton.BackgroundColor = Color.LightGray;
                favouriteButton.TextColor = Color.Black;              
            }
            else
            {
                favouriteButton.Text = Localization.Favourite;
                favouriteButton.BackgroundColor = Color.LimeGreen;
                favouriteButton.TextColor = Color.White;
            }
        }

        private void IMDBButton_Clicked(object sender, EventArgs e)
        {
            if (Film.IMDB != null)
            {
                try
                {
                    Device.OpenUri(new Uri(Film.IMDB));
                }
                catch (Exception) { }
            }
        }
    }
}