using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using FilmCatalog.Models;
using FilmCatalog.Collections;
using System.Xml.Serialization;
using Windows.Storage;
using System.IO;

namespace FilmCatalog
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MenuPage : MasterDetailPage
    {
        public Settings Settings;
        public FilmsDB Films;
        public ActorsDB Actors;
        public ProducersDB Producers;

        public MenuPage(Settings settings)
        {
            InitializeComponent();

            Master.BackgroundColor = Color.LightGray;
            //sets = settings;

            Films = new FilmsDB();
            Actors = new ActorsDB();
            Producers = new ProducersDB();
            Settings = new Settings();

            LoadFilms(ref Films);
            LoadActors(ref Actors);
            LoadProducers(ref Producers);
            LoadSettings(ref Settings);

            MessagingCenter.Subscribe<FilmDetailPage, string>(this, "SearchProducer", (obj, producerName) =>
            {
                foreach (Producer producer in Producers)
                {
                    if (producerName == producer.Name)
                    {
                        MessagingCenter.Send(this, "Producer", producer);
                        break;
                    }
                }
            });

            MessagingCenter.Subscribe<FilmDetailPage, string>(this, "SearchActor", (obj, actorName) =>
            {
                bool isExist = false;
                foreach (Actor actor in Actors)
                {
                    if (actorName == actor.Name)
                    {
                        isExist = true;
                        MessagingCenter.Send(this, "Actor", actor);
                        break;
                    }
                }
                if (!isExist)
                {
                    Navigation.PushModalAsync(new NavigationPage(new NewActorPage(actorName)));
                }

            });

            MessagingCenter.Subscribe<ActorDetailPage, string>(this, "getFilms", (obj, actorName) =>
            {
                var films = from f in Films 
                                    where f.Actors.Contains(actorName)                                   
                                    select f;
                
                 MessagingCenter.Send(this, "ActorFilms", films.ToList<Film>());
            });

            MessagingCenter.Subscribe<ProducerDetailPage, string>(this, "getFilms", (obj, producerName) =>
            {
                var films = from f in Films 
                            where f.Producer==producerName 
                            select f;

                MessagingCenter.Send(this, "ProducerFilms", films.ToList<Film>());
            });

            MessagingCenter.Subscribe<NewFilmPage,Film>(this, "AddFilm", (obj,newFilm)=>
            {
                Films.Add(newFilm);
                if (Settings.Autosave == true)
                {
                    Films.Save();
                }
                Detail = new NavigationPage(new FilmsPage(ref Films));
            });

            MessagingCenter.Subscribe<FilmDetailPage, string>(this, "DeleteFilm", (obj, deletedFilm) =>
            {
                Films.DeleteFilm(deletedFilm);
                if (Settings.Autosave == true)
                {
                    Films.Save();
                }
                Detail = new NavigationPage(new FilmsPage(ref Films));
            });

            MessagingCenter.Subscribe<NewFilmPage, (Film, string)>(this, "EditFilm", (obj, tuple) =>
            {
                Films.EditFilm(tuple.Item2, tuple.Item1);
                if (Settings.Autosave == true)
                {
                    Films.Save();
                }
                Detail = new NavigationPage(new FilmsPage(ref Films));
            });

            MessagingCenter.Subscribe<NewActorPage, Actor>(this, "AddActor", (obj, newActor) =>
            {
                Actors.Add(newActor);
                if (Settings.Autosave == true)
                {
                    Actors.Save();
                }
                Detail = new NavigationPage(new ActorsPage(ref Actors));
            });

            MessagingCenter.Subscribe<ActorDetailPage, string>(this, "DeleteActor", (obj, deletedActor) =>
            {
                Actors.DeleteActor(deletedActor);
                if (Settings.Autosave == true)
                {
                    Actors.Save();
                }
                Detail = new NavigationPage(new ActorsPage(ref Actors));
            });

            MessagingCenter.Subscribe<NewActorPage, (Actor, string)>(this, "EditActor", (obj, tuple) =>
            {
                Actors.EditActor(tuple.Item2, tuple.Item1);
                if (Settings.Autosave == true)
                {
                    Actors.Save();
                }
                Detail = new NavigationPage(new ActorsPage(ref Actors));
            });

            MessagingCenter.Subscribe<NewFilmPage, string>(this, "AddAutoProducer", (obj, newProducerName) =>
            {
                bool producerExist = false;
                foreach (Producer producer in Producers)
                {
                    if (producer.Name == newProducerName)
                    {
                        producerExist = true;
                        break;
                    }
                }
                if (!producerExist)
                {
                    Producers.Add(new Producer() { Name = newProducerName });
                    if (Settings.Autosave == true)
                    {
                        Producers.Save();
                    }
                }
                Detail = new NavigationPage(new ProducersPage(ref Producers));
            });

            MessagingCenter.Subscribe<NewProducerPage, Producer>(this, "AddProducer", (obj, newProducer) =>
            {
                Producers.Add(newProducer);
                if (Settings.Autosave == true)
                {
                    Producers.Save();
                }
                Detail = new NavigationPage(new ProducersPage(ref Producers));
            });

            MessagingCenter.Subscribe<ProducerDetailPage, string>(this, "DeleteProducer", (obj, deletedProducer) =>
            {
                Producers.DeleteProducer(deletedProducer);
                Detail = new NavigationPage(new ProducersPage(ref Producers));
            });

            MessagingCenter.Subscribe<SettingsPage, bool>(this, "Autosave", (obj, enabling) =>
            {
                Settings.Autosave = enabling;
            });

            MessagingCenter.Subscribe<SettingsPage>(this, "SaveSettings", obj =>
            {
                Settings.Save();
            });

            MessagingCenter.Subscribe<FilmDetailPage, string>(this, "Favourite", (obj, filmName) =>
            {
                for (int i = 0; i < Films.Count; i++)
                {
                    if (Films[i].Name == filmName)
                    {
                        Films[i].Favorite = !Films[i].Favorite;
                        break;
                    }
                }
                if (Settings.Autosave == true)
                {
                    Films.Save();
                }
            });
            

            MessagingCenter.Subscribe<SettingsPage>(this, "Refresh", (obj) =>
            {
                FilmsLabel.Text = Localization.FilmsLabel;
                ActorsLabel.Text = Localization.ActorsLabel;
                ProducersLabel.Text = Localization.ProducersLabel;
                ChartsLabel.Text = Localization.Infographics;
                SettingsLabel.Text = Localization.Settings;
                SaveLabel.Text = Localization.Save;
                Settings.Language = Localization.Culture.ToString();
            });

            MasterBehavior = MasterBehavior.Popover;
            Detail = new NavigationPage(new FilmsPage(ref Films));
            IsPresented = false;
        }

        private void LoadFilms(ref FilmsDB Films)
        {
            try
            {
                Films = FilmsDB.Load();
            }
            catch (Exception)
            {
                List<Film> BaseFilms = new List<Film>()
                {
                    new Film {Name = "Побег из Шоушенка", Genre = "драма", AgeLimit = 12, Release = 1994, Producer = "Фрэнк Дарабонт", Poster="Posters/Побег из Шоушенка.jpg", Actors = new List<string>(){"Тим Роббинс", "Морган Фриман" }},
                    new Film {Name = "Умница Уилл Хантинг", Genre = "драма", AgeLimit = 16, Release = 1997, Producer = "Гас Ван Сент", Poster="Posters/Умница Уилл Хантинг.jpg", Actors = new List<string>(){"Мэтт Деймон", "Бен Аффлек" }},
                    new Film {Name = "Господин Никто", Genre = "фантастика, фэнтези, драма, мелодрама", AgeLimit = 18, Release = 2009, Producer = "Жако Ван Дормаль", Poster="Posters/Господин Никто.jpg", Actors = new List<string>(){"Джаред Лето", "Диана Крюгер" }},
                    new Film {Name = "Крёстный отец", Genre = "драма, криминал" , AgeLimit = 16, Release = 1972, Producer = "Френсис Форд Коппола", Poster="Posters/Крёстный отец.jpg",Actors = new List<string>(){"Марлон Брандо", "Аль Пачино", "Джеймс Каан" }},
                    new Film {Name = "Джанго освобождённый", Genre = "вестерн, комедия, драма", AgeLimit = 18, Release = 2012, Producer = "Квентин Тарантино",Poster="Posters/Джанго освобождённый.jpg", Actors = new List<string>(){"Джейми Фокс", "Кристоф Вальц" }},
                };

                foreach (Film film in BaseFilms)
                    Films.AddFilm(film);
            }
        }      

        private void LoadActors(ref ActorsDB Actors)
        {
            try
            {
                Actors = ActorsDB.Load();
            }
            catch (Exception)
            {
                List<Actor> BaseActors = new List<Actor>()
                {
                    new Actor {Name = "Тим Роббинс", Birthday = new DateTime(1958, 10, 16), Biography = "Биография"},
                    new Actor {Name = "Морган Фриман", Birthday = new DateTime(1937, 6, 1), Biography = "Биография"},
                    new Actor {Name = "Мэтт Деймон", Birthday = new DateTime(1970, 10, 8), Biography = "Биография"},
                    new Actor {Name = "Бен Аффлек", Birthday = new DateTime(1972, 8, 15), Biography = "Биография"},
                    new Actor {Name = "Джаред Лето", Birthday = new DateTime(1971, 12, 26), Biography = "Биография"},
                    new Actor {Name = "Диана Крюгер", Birthday = new DateTime(1976, 7, 15), Biography = "Биография"},
                    new Actor {Name = "Марлон Брандо", Birthday = new DateTime(1924, 4, 3), Biography = "Биография"},
                    new Actor {Name = "Аль Пачино", Birthday = new DateTime(1940, 4, 25), Biography = "Биография"},
                    new Actor {Name = "Джеймс Каан", Birthday = new DateTime(1940, 5, 26), Biography = "Биография"},
                    new Actor {Name = "Джейми Фокс", Birthday = new DateTime(1967, 12, 13), Biography = "Биография"},
                    new Actor {Name = "Кристоф Вальц", Birthday = new DateTime(1956, 10, 4), Biography = "Биография"},
                };
                foreach (Actor actor in BaseActors)
                    Actors.AddActor(actor);
            }
        }

        private void LoadProducers(ref ProducersDB Producers)
        {
            try
            {
                Producers = ProducersDB.Load();
            }
            catch (Exception)
            {
                List<Producer> BaseProducers = new List<Producer>()
                {
                    new Producer {Name = "Фрэнк Дарабонт"},
                    new Producer {Name = "Гас Ван Сент"},
                    new Producer {Name = "Жако Ван Дормаль"},
                    new Producer {Name = "Френсис Форд Коппола"},
                    new Producer {Name = "Квентин Тарантино"},
                };
                foreach (Producer producer in BaseProducers)
                    Producers.AddProducer(producer);
            }
        }

        private void LoadSettings(ref Settings settings)
        {
            try
            {
                settings = Settings.Load();
            }
            catch (Exception)
            {
                settings.Autosave = false;
                settings.Language = DependencyService.Get<ILocalize>().GetCurrentCultureInfo().ToString();
            }
        }

        private void Films_button_clicked(object sender, EventArgs e)
        {
            Detail = new NavigationPage(new FilmsPage(ref Films));
            IsPresented = false;
        }        

        private void Actors_button_clicked(object sender, EventArgs e)
        {
            Detail = new NavigationPage(new ActorsPage(ref Actors));
            IsPresented = false;
        }

        private void Producers_button_clicked(object sender, EventArgs e)
        {
            Detail = new NavigationPage(new ProducersPage(ref Producers));
            IsPresented = false;
        }

        async private void Save_button_clicked(object sender, EventArgs e)
        {
            await saveImage.RotateYTo(360, 1000);
            saveImage.RotationY = 0;
            Films.Save();
            Actors.Save();
            Producers.Save();
            Settings.Save();           
        }

        private Dictionary<string, int> GetTopGenres(FilmsDB Films, int max)
        {
            var delimiter = new Regex(@"\s*,\s*");
            string genres = "";
            
            var Genres = new Dictionary<string, int>();
            foreach (Film film in Films)
            {
                genres = delimiter.Replace(film.Genre, "|");
                string[] genresArr = genres.Split(new char[] { '|' });
                foreach (string genre in genresArr)
                {
                    if (Genres.ContainsKey(genre))
                        Genres[genre]++;
                    else
                        Genres.Add(genre, 1);
                }
            }
            var Top = new Dictionary<string, int>();
            int i = 0;
            foreach (var genre in Genres.OrderByDescending(genre => genre.Value))
            {
                if (max > i)
                {
                    Top.Add(genre.Key, genre.Value);
                    i++;
                }
                else break;
            }
            return Top;
        }

        private void Charts_button_Clicked(object sender, EventArgs e)
        {
            Detail = new NavigationPage(new ChartsPage(GetTopGenres(Films, 10)));
            IsPresented = false;
        }

        private void Settings_button_Clicked(object sender, EventArgs e)
        {
            Detail = new NavigationPage(new SettingsPage(ref Settings));
            IsPresented = false;
        }
    }
}