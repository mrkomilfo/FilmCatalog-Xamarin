using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

using FilmCatalog.Models;
using Windows.Storage;

namespace FilmCatalog.Collections
{
    [Serializable]
    public class ProducersDB : List<Producer>
    {
        public void AddProducer(Producer newProducer)
        {
            this.Add(newProducer);
        }

        public void DeleteProducer(string name)
        {
            foreach (Producer producer in this)
            {
                if (producer.Name == name)
                {
                    this.Remove(producer);
                    break;
                }
            }
        }

        public void Save()
        {
            BinaryFormatter formatter = new BinaryFormatter();
            StorageFolder localFolder = ApplicationData.Current.RoamingFolder;
            using (FileStream fs = new FileStream(localFolder.Path + "/Producers.dat", FileMode.Create))
            {
                formatter.Serialize(fs, this);
            }
        }

        public static ProducersDB Load()
        {
            BinaryFormatter formatter = new BinaryFormatter();
            StorageFolder localFolder = ApplicationData.Current.RoamingFolder;
            using (FileStream fs = new FileStream(localFolder.Path + "/Producers.dat", FileMode.Open))
            {
                return (ProducersDB)formatter.Deserialize(fs);
            }

        }
    }  
}
