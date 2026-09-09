using IMDBConsoleApp.Models;
using System;
using System.Collections.Generic;

namespace IMDBConsoleApp.Models
{
    public class Movie
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int YearOfRelease { get; set; }
        public string Plot { get; set; }
        public List<int> ActorIds { get; set; }
        public int ProducerId { get; set; }
        
        public Movie()
        {
            ActorIds = new List<int>();
        }
    }
}
