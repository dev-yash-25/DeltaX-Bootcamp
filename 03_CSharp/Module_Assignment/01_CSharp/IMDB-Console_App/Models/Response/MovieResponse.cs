    using System;
    using System.Collections.Generic;

    namespace IMDBConsoleApp.Models.Response
    {
        public class MovieResponse
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public int YearOfRelease { get; set; }
            public string Plot { get; set; }
            public List<Actor> Actors { get; set; }  
            public Producer Producer { get; set; }
            
            public MovieResponse()
            {
                Actors = new List<Actor>();
            }
        }
    }
