using IMDBConsoleApp.Models;
using IMDBConsoleApp.Repository.Interface;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace IMDBConsoleApp.Repository
{
    internal class ActorRepository : IActorRepository
    {
        private readonly List<Actor> _actors = new List<Actor>();
        private int _id = 1;
        
        public void Add(Actor actor)
        {
            actor.Id = _id++;
            _actors.Add(actor);
        }

        public IEnumerable<Actor> Get()
        {
            return _actors.ToList();
        }

        public Actor Get(int id)
        {
            return _actors.SingleOrDefault(a => a.Id == id);
        }

        public IEnumerable<Actor> Get(IEnumerable<int> ids)
        {
            return _actors
                .Where(actor => ids.Contains(actor.Id))
                .ToList();
        }
    }
}
