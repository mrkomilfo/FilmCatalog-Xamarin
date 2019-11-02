using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using FilmCatalog.Collections;
using FilmCatalog.Models;
using System.Collections.ObjectModel;

namespace FilmCatalog
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ProducersPage : ContentPage
    {
        public ObservableCollection<Producer> Producers { get; set; }
        public ProducersDB startProducers { get; set; }
        private bool sort = true;

        public ProducersPage(ref ProducersDB producers)
        {
            InitializeComponent();
            startProducers = new ProducersDB();
            startProducers = producers;
            Producers = new ObservableCollection<Producer>();
            foreach (Producer producer in startProducers)
            {
                Producers.Add(producer);
            }
            this.SearchBar.SearchCommand = new Command(() => { Search(); });
            this.BindingContext = this;
        }

        async void ProduccerTapped(object sender, ItemTappedEventArgs args)
        {
            await Navigation.PushAsync(new ProducerDetailPage(args.Item as Producer));
        }

        async void AddProducer_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new NewProducerPage());
        }

        private void Search()
        {
            string[] keyWords = SearchBar.Text.Split(' ');
            var matches = new ProducersDB();
            Producers.Clear();
            foreach (Producer producer in startProducers)
            {
                bool appropriate = true;
                foreach (string word in keyWords)
                {
                    if (word == "")
                    {
                        continue;
                    }
                    else if (!producer.Name.ToLower().Contains(word.ToLower()))
                    {
                        appropriate = false;
                        break;
                    }
                }
                if (appropriate == true)
                {
                    Producers.Add(producer);
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
                Producers.Clear();
                foreach (Producer producer in startProducers)
                {
                    Producers.Add(producer);
                }
            }
        }

        void Sort_Clicked(object sender, EventArgs e)
        {
            List<Producer> sortedActors;
            if (sort == true)
            {
                sortedActors = (from i in Producers
                                orderby i.Name
                                select i).ToList();
            }
            else
            {
                sortedActors = (from i in Producers
                                orderby i.Name descending
                                select i).ToList();
            }

            sort = !sort;
            Producers.Clear();

            foreach (var f in sortedActors)
            {
                Producers.Add(f);
            }
        }
    }
}