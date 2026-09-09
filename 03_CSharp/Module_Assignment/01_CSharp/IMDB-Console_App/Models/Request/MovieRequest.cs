using System;
using System.Collections.Generic;

namespace IMDBConsoleApp.Models.Request
{
    public class MovieRequest
    {
        public string Name { get; set; }
        public int YearOfRelease { get; set; }
        public string Plot { get; set; }
        public List<int> ActorIds{ get; set; }
        public int ProducerId { get; set; }
    }
}
