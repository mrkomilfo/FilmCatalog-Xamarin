using System;
using System.Collections.Generic;
using System.Text;

namespace FilmCatalog.Models
{
    [Serializable]
    public class Actor
    {
        public string Name { get; set; }
        public DateTime Birthday { get; set; }
        public string Biography { get; set; }
        public List<string> Films { get; set; }
        public string Wiki { get; set; }
    }
}
