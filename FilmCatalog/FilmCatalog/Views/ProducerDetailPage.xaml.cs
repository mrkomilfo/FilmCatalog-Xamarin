using System;
using System.Collections.Generic;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using FilmCatalog.Models;

namespace FilmCatalog
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ProducerDetailPage : ContentPage
	{
        public Producer Producer { get; set; }
        public List<Film> Films { get; set; }
        public ProducerDetailPage(Producer producer)
        {
            InitializeComponent();
            Producer = producer;
            MessagingCenter.Subscribe<MenuPage, List<Film>>(this, "ProducerFilms", (obj, films) =>
            {
                Films = films;
            });
            MessagingCenter.Send(this, "getFilms", Producer.Name);
            this.BindingContext = this;
        }
        public void FilmTapped(object sender, ItemTappedEventArgs e)
        {
            Navigation.PushAsync(new FilmDetailPage(e.Item as Film));
        }
        void DeleteClicked(object sender, EventArgs e)
        {
            MessagingCenter.Send(this, "DeleteProducer", Producer.Name);
        }
    }
}