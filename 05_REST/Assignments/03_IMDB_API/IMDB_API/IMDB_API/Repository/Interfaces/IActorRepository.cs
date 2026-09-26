using IMDB_API.Models.Db;
using System.Collections;
using System.Collections.Generic;

namespace IMDB_API.Repository.Interfaces
{
    public interface IActorRepository
    {
        IEnumerable<Actor> Get();
        Actor Get(int id);
        IEnumerable<Actor> Get(IEnumerable<int> ids);

        void Add(Actor actor);
        void Update(int id, Actor actor);
        void Delete(int id);
    }
}
