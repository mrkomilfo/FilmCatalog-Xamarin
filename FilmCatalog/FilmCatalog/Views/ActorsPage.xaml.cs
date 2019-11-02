using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using FilmCatalog.Collections;
using FilmCatalog.Models;


namespace FilmCatalog
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ActorsPage : ContentPage
	{       
        public ObservableCollection<Actor> Actors { get; set; }
        public ActorsDB startActors { get; set; }
        private bool sort = true;

        public ActorsPage(ref ActorsDB actors)
        {
            InitializeComponent();
            startActors = new ActorsDB();
            startActors = actors;
            Actors = new ObservableCollection<Actor>();
            foreach (Actor actor in startActors)
            {
                Actors.Add(actor);
            }
            this.SearchBar.SearchCommand = new Command(() => { Search(); });
            this.BindingContext = this;
        }

        async void ActorTapped(object sender, ItemTappedEventArgs args)
        {
            await Navigation.PushAsync(new ActorDetailPage(args.Item as Actor));
        }

        async void AddActor_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new NavigationPage(new NewActorPage()));
        }

        public ActorsPage()
        {
            InitializeComponent();
        }

        private void Search()
        {
            string[] keyWords = SearchBar.Text.Split(' ');
            var matches = new ActorsDB();
            Actors.Clear();
            foreach (Actor actor in startActors)
            {
                bool appropriate = true;
                foreach (string word in keyWords)
                {
                    if (word == "")
                    {
                        continue;
                    }
                    else if (!actor.Name.ToLower().Contains(word.ToLower()))
                    {
                        appropriate = false;
                        break;
                    }
                }
                if (appropriate == true)
                {
                    Actors.Add(actor);
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
                Actors.Clear();
                foreach (Actor actor in startActors)
                {
                    Actors.Add(actor);
                }
            }
        }

        void Sort_Clicked(object sender, EventArgs e)
        {
            List<Actor> sortedActors;
            if (sort == true)
            {
                sortedActors = (from i in Actors
                               orderby i.Name
                               select i).ToList();
            }
            else
            {
                sortedActors = (from i in Actors
                               orderby i.Name descending
                               select i).ToList();
            }

            sort = !sort;
            Actors.Clear();

            foreach (var f in sortedActors)
            {
                Actors.Add(f);
            }
        }
    }
}