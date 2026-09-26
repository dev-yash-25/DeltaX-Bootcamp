using IMDB_API.Models.Db;
using IMDB_API.Repository.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace IMDB_API.Repository
{
    public class ActorRepository : IActorRepository
    {
        private readonly List<Actor> _actors = new List<Actor>();
        private int _id = 1;

        public IEnumerable<Actor> Get()
        {
            return _actors;
        }

        public Actor Get(int id)
        {
            var actor = _actors.SingleOrDefault(a => a.Id == id);
            return actor;
        }

        public IEnumerable<Actor> Get(IEnumerable<int> actorIds)
        {
            var actors = _actors
                .Where(actor => actorIds.Contains(actor.Id))
                .ToList();

            return actors;
        }

        public void Add(Actor actor)
        {
            actor.Id = _id++;
            _actors.Add(actor);
        }

        public void Update(int id, Actor actor)
        {
            var existingActor = _actors.SingleOrDefault(a => a.Id == id);

            existingActor.Name = actor.Name;
            existingActor.DOB = actor.DOB;
            existingActor.Bio = actor.Bio;
            existingActor.Gender = actor.Gender;
        }

        public void Delete(int id)
        {
            var actor = _actors.SingleOrDefault(a => a.Id == id);
            _actors.Remove(actor);
        }
    }
}
