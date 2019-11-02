using System.Collections.Generic;

using SkiaSharp;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Entry = Microcharts.Entry;
using Microcharts;

namespace FilmCatalog
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ChartsPage : ContentPage
    {
        List<Entry> entries = new List<Entry>();
        public ChartsPage(Dictionary<string,int> Genres)
        {
            InitializeComponent();
            foreach (var genre in Genres)
            {
                var entry = new Entry(genre.Value)
                {
                    Color = SKColor.Parse("#2196F3"),
                    Label = genre.Key,
                    ValueLabel = genre.Value.ToString()
                };
                entries.Add(entry);
            }
            genresChart.Chart = new BarChart { Entries = entries, ValueLabelOrientation = Orientation.Horizontal, LabelOrientation = Orientation.Horizontal, LabelTextSize = 20 };
            genresChart.BackgroundColor = Color.FromHex("#e2feff");
            //BackgroundColor = Color.FromHex("#e2feff");
        }
    }
}