using System;
using System.Collections.Generic;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using FilmCatalog.Models;

namespace FilmCatalog
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class NewFilmPage : ContentPage
	{
        Film newFilm = new Film();
        string oldName;
        bool favourite;
        bool format;

        public NewFilmPage()
		{
			InitializeComponent();
            format = true;
		}

        public NewFilmPage(Film film)
        {
            InitializeComponent();
            nameEntry.Text = film.Name;
            ratingEntry.Text = film.Rating.ToString();
            genreEntry.Text = film.Genre;
            ageLimitEntry.Text = film.AgeLimit.ToString();
            releaseEntry.Text = film.Release.ToString();
            actorsEditor.Text = film.ActorsToString();
            producerEntry.Text = film.Producer;
            pathEntry.Text = film.Poster;
            IMDBEntry.Text = film.IMDB;
            newFilm.Favorite = false;
            oldName = film.Name;
            favourite = film.Favorite;
            format = false;
        }

        List<string> GetActors(string actors)
        {
            List<string> actorsList = new List<string>();
            string[] actorsArr = actors.Split(new char[] { '\r' });

            foreach (string newActor in actorsArr)
            {
                actorsList.Add(newActor);
            }
            return actorsList;
        }

        async void Save_Clicked(object sender, EventArgs e)
        {           
            newFilm.Name = nameEntry.Text;
            newFilm.Rating = float.Parse(ratingEntry.Text);
            newFilm.Genre = genreEntry.Text;
            newFilm.AgeLimit = Int32.Parse(ageLimitEntry.Text);
            newFilm.Release = Int32.Parse(releaseEntry.Text);
            newFilm.Actors = GetActors(actorsEditor.Text);
            newFilm.Producer = producerEntry.Text;
            newFilm.Poster = pathEntry.Text;
            newFilm.IMDB = IMDBEntry.Text;            

            if (format == true)
            {
                MessagingCenter.Send(this, "AddAutoProducer", newFilm.Producer);
                MessagingCenter.Send(this, "AddFilm", newFilm);
            }
            else
            {
                //MessagingCenter.Send(this, "AddAutoProducer", newFilm.Producer);
                newFilm.Favorite = favourite;
                MessagingCenter.Send(this, "EditFilm", (newFilm,oldName));
            }

            await Navigation.PopModalAsync();           
        }

        async void Cancel_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }
    }
}