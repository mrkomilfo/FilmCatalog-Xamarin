using System;
using System.Linq;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using FilmCatalog.Collections;
using FilmCatalog.Models;
using System.Collections.ObjectModel;
using System.Collections.Generic;

namespace FilmCatalog
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FilmsPage : ContentPage
    {
        public ObservableCollection<Film> Films { get; set; }
        public FilmsDB startFilms { get; set; }
        private bool sort = true;
        private bool favourites = false;

        public FilmsPage(ref FilmsDB films)
        {
            InitializeComponent();
            startFilms = new FilmsDB();
            startFilms = films;
            Films = new ObservableCollection<Film>();
            foreach (Film film in startFilms)
            {
                Films.Add(film);
            }
            this.SearchBar.SearchCommand = new Command(() => { Search(); });
            BindingContext = this;

            MessagingCenter.Subscribe<FilmDetailPage, int>(this, "Age", (obj, age) =>
            {
                FilmsDB filmsDB = new FilmsDB();
                var selectedFilms = from f in Films
                                    where f.AgeLimit <= age
                                    orderby f.Name
                                    select f;
                foreach (var film in selectedFilms.ToList<Film>())
                {
                    filmsDB.AddFilm(film);
                }
                Navigation.PushAsync(new FilmsPage(ref filmsDB));
            });
        }

        void FilmTapped(object sender, ItemTappedEventArgs args)
        {
            Navigation.PushAsync(new FilmDetailPage(args.Item as Film));
        }

        async void AddFilm_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new NavigationPage(new NewFilmPage()));
        }

        public FilmsPage()
        {
            InitializeComponent();
        }

        private void Search()
        {
            string[] keyWords = SearchBar.Text.Split(' ');
            var matches = new FilmsDB();
            Films.Clear();
            foreach (Film film in startFilms)
            {
                bool appropriate = true;
                foreach (string word in keyWords)
                {
                    if (word == "")
                    {
                        continue;
                    }
                    else if (!film.Name.ToLower().Contains(word.ToLower()) && !film.Genre.ToLower().Contains(word.ToLower()))
                    {
                        appropriate = false;
                        break;
                    }
                }
                if (appropriate == true)
                {
                    Films.Add(film);
                }
            }
        }

        private void SearchBar_SearchButtonPressed(object sender, EventArgs e)
        {
            Search();
        }

        private void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (e.NewTextValue == string.Empty && e.OldTextValue.Length > 1)
            {
                Films.Clear();
                foreach (Film film in startFilms)
                {
                    Films.Add(film);
                }
            }
        }

        void Sort_Clicked(object sender, EventArgs e)
        {
            List<Film> sortedFilms;
            if (sort == false)
            {
                sortedFilms = (from i in Films
                              orderby i.Rating
                               select i).ToList();
            }
            else
            {
                sortedFilms = (from i in Films
                              orderby i.Rating descending
                              select i).ToList();
            }

            sort = !sort;
            Films.Clear();

            foreach (var f in sortedFilms)
            {
                Films.Add(f);
            }
        }

        private void Favourites_Clicked(object sender, EventArgs e)
        {
            List<Film> favouriteFilms = new List<Film>();
            if (favourites == false)
            {
                favouriteFilms = (from i in Films
                              where i.Favorite == true
                              select i).ToList();
            }
            else
            {
                favouriteFilms = startFilms;
            }
            favourites = !favourites;
            Films.Clear();

            foreach (var f in favouriteFilms)
            {
                Films.Add(f);
            }
        }
    }
}