using System;
using System.Collections.Generic;
using System.Text;

namespace FilmCatalog.Models
{
    [Serializable]
    public class Film
    {
        public string Name { get; set; }
        public string Genre { get; set; }
        public int AgeLimit { get; set; }
        public int Release { get; set; }
        public string Producer { get; set; }
        public string Poster { get; set; }
        public List<string> Actors { get; set; }
        public bool Favorite { get; set; }
        public float Rating { get; set; }
        public string IMDB { get; set; }

        public string ActorsToString()
        {
            string actors = "";
            for (int i = 0; i < Actors.Count; i++)
            {
                actors += Actors[i];
                if (i < Actors.Count-1)
                {
                    actors += '\r';
                }
            }
            return actors;               
        }
    }
}
