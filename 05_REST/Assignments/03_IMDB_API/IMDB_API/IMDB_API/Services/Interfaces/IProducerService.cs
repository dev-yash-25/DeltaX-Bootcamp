using IMDB_API.Models.Db;
using IMDB_API.Models.Requests;
using IMDB_API.Models.Responses;
using IMDB_API.Repository;
using Microsoft.AspNetCore.Cors.Infrastructure;
using System.Collections.Generic;

namespace IMDB_API.Services.Interfaces
{
    public interface IProducerService
    {
        IEnumerable<ProducerResponse> Get();
        ProducerResponse Get(int id);

        int Add(ProducerRequest request);
        void Update(int id, ProducerRequest request);
        void Delete(int id);
    }
}
