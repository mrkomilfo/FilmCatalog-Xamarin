using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

using FilmCatalog.Models;
using Windows.Storage;

namespace FilmCatalog.Collections
{
    [Serializable]
    public class ActorsDB : List<Actor>
    {
        public void AddActor(Actor newActor)
        {
            this.Add(newActor);
        }

        public void DeleteActor(string name)
        {
            foreach (Actor actor in this)
            {
                if (actor.Name == name)
                {
                    this.Remove(actor);
                    break;
                }
            }
        }

        public void DeleteActor(ref Actor actor)
        {
            this.Remove(actor);
        }

        public void EditActor(string name, Actor editedActor)
        {
            for (int i = 0; i < this.Count; i++)
            {
                if (this[i].Name == name)
                {
                    this[i] = editedActor;
                    break;
                }
            }
        }

        public void Save()
        {
            BinaryFormatter formatter = new BinaryFormatter();
            StorageFolder localFolder = ApplicationData.Current.RoamingFolder;
            using (FileStream fs = new FileStream(localFolder.Path + "/Actors.dat", FileMode.Create))
            {
                formatter.Serialize(fs, this);
            }
        }

        public static ActorsDB Load()
        {
            BinaryFormatter formatter = new BinaryFormatter();
            StorageFolder localFolder = ApplicationData.Current.RoamingFolder;
            using (FileStream fs = new FileStream(localFolder.Path + "/Actors.dat", FileMode.Open))
            {
                return (ActorsDB)formatter.Deserialize(fs);
            }

        }
    }
}
