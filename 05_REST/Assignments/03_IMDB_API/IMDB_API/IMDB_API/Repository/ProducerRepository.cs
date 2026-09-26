using IMDB_API.Models.Db;
using IMDB_API.Models.Requests;
using IMDB_API.Repository.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace IMDB_API.Repository
{
    public class ProducerRepository : IProducerRepository
    {
        private readonly List<Producer> _producers = new List<Producer>();
        private int _id = 1;

        public IEnumerable<Producer> Get()
        {
            return _producers;
        }

        public Producer Get(int id)
        {
            var producer = _producers.SingleOrDefault(p => p.Id == id);
            return producer;
        }

        public int Add(Producer producer)
        {
            producer.Id = _id++;
            _producers.Add(producer);
            return producer.Id;
        }

        public void Update(int id, Producer producer)
        {
            var existingProducer = _producers.SingleOrDefault(p => p.Id == id);

            existingProducer.Name = producer.Name;
            existingProducer.DOB = producer.DOB;
            existingProducer.Bio= producer.Bio;
            existingProducer.Gender = producer.Gender;
            
        }

        public void Delete(int id)
        {
            var producer = _producers.SingleOrDefault(p => p.Id == id);
            _producers.Remove(producer);
        }
    }
}
