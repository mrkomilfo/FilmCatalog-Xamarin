using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using FilmCatalog.Models;

namespace FilmCatalog
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class NewProducerPage : ContentPage
	{
        Producer newProducer;

		public NewProducerPage ()
		{
			InitializeComponent();
            newProducer = new Producer();
        }

        async void Save_Clicked(object sender, EventArgs e)
        {
            newProducer.Name = nameEntry.Text;
          
            MessagingCenter.Send(this, "AddProducer", newProducer);
            await Navigation.PopModalAsync();
        }

        async void Cancel_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }
    }
}