using System;
using System.Collections.Generic;
using System.IO;
using Windows.Storage;

using FilmCatalog.Models;
using System.Runtime.Serialization.Formatters.Binary;

namespace FilmCatalog.Collections
{
    [Serializable]
    public class FilmsDB: List<Film>
    {
        public void AddFilm(Film newFilm)
        {
            this.Add(newFilm);
        }

        public void DeleteFilm(string name)
        {
            foreach (Film film in this)
            {
                if (film.Name == name)
                {
                    this.Remove(film);
                    break;
                }
            }
        }

        public void DeleteFilm(ref Film film)
        {
            this.Remove(film);
        }

        public void EditFilm(string name, Film editedFilm)
        {
            for (int i = 0; i < this.Count; i++)
            {
                if (this[i].Name == name)
                {
                    this[i] = editedFilm;
                    break;
                }
            }
        }
        
        public void Save()
        {
            BinaryFormatter formatter = new BinaryFormatter();
            StorageFolder localFolder = ApplicationData.Current.RoamingFolder;
            using (FileStream fs = new FileStream(localFolder.Path + "/Films.dat", FileMode.Create))
            {
                formatter.Serialize(fs, this);
            }
        }

        public static FilmsDB Load()
        {
            BinaryFormatter formatter = new BinaryFormatter();
            StorageFolder localFolder = ApplicationData.Current.RoamingFolder;                               
            using (FileStream fs = new FileStream(localFolder.Path + "/Films.dat", FileMode.Open))
            {
                return (FilmsDB)formatter.Deserialize(fs);
            }

        }
    }
}
