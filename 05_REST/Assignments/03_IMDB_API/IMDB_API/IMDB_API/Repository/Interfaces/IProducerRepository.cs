using IMDB_API.Models.Db;
using System.Collections;
using System.Collections.Generic;

namespace IMDB_API.Repository.Interfaces
{
    public interface IProducerRepository
    {
        IEnumerable<Producer> Get();
        Producer Get(int id);

        int Add(Producer producer);
        void Update(int id, Producer producer);
        void Delete(int id);
    }
}
