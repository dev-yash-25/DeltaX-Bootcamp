using IMDBConsoleApp.Models;
using IMDBConsoleApp.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace IMDBConsoleApp.Repository
{
    internal class ProducerRepository : IProducerRepository
    {
        private readonly List<Producer> _producers = new List<Producer>();
        private int _id = 1;
        
        public void Add(Producer producer)
        {
            producer.Id = _id++;
            _producers.Add(producer);
        }

        public IEnumerable<Producer> Get()
        {
            return _producers.ToList();
        }

        public Producer Get(int id)
        {
            return _producers.SingleOrDefault(p => p.Id == id);
        }
    }
}
