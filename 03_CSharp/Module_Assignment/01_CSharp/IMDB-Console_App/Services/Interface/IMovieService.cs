using System;
using IMDBConsoleApp.Models;
using System.Collections.Generic;
using IMDBConsoleApp.Models.Response;
using IMDBConsoleApp.Models.Request;

namespace IMDBConsoleApp.Services.Interface
{
    public interface IMovieService 
    {
        void Add(MovieRequest movieRequest);
        Movie Get(int id);
        IEnumerable<MovieResponse> Get();
        void Delete(int id);
    }
}
