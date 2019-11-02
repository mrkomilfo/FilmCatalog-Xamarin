using System;
using System.Collections.Generic;
using System.Text;

namespace FilmCatalog.Models
{
    [Serializable]
    public class Producer
    {
        public string Name { get; set; }
        public List<string> Films { get; set; }        
    }
}
