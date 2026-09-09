using IMDBConsoleApp.Models;
using System.Collections.Generic;

public interface IMovieRepository
{
    void Add(Movie movie);
    IEnumerable<Movie> Get();
    Movie Get(int id);
    void Delete(int id);
}