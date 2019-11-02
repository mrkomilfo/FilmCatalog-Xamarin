using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Xml.Serialization;
using Windows.Storage;
using Xamarin.Forms;

namespace FilmCatalog.Models
{
    [Serializable]
    public class Settings
    {
        public string Language { get; set; }
        public bool Autosave { get; set; }

        public void Save()
        {
            XmlSerializer formatter = new XmlSerializer(typeof(Settings));
            StorageFolder localFolder = ApplicationData.Current.LocalFolder;
            using (FileStream fs = new FileStream(localFolder.Path + "/Settings.xml", FileMode.Create))
            {
                formatter.Serialize(fs, this/*new Settings { Language = Localization.Culture.ToString() }*/);
            };
        }

        public static Settings Load()
        {
            XmlSerializer formatter = new XmlSerializer(typeof(Settings));
            StorageFolder localFolder = ApplicationData.Current.LocalFolder;
            using (FileStream fs = new FileStream(localFolder.Path + "/Settings.xml", FileMode.Open))
            {
                return (Settings)formatter.Deserialize(fs);
            }            
        }
    }
}
