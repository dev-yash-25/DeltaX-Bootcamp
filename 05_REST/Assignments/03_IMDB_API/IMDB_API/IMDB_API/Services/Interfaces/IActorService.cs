using IMDB_API.Models.Requests;
using IMDB_API.Models.Responses;
using System.Collections;
using System.Collections.Generic;

namespace IMDB_API.Services.Interfaces
{
    public interface IActorService
    {
        IEnumerable<ActorResponse> Get();
        ActorResponse Get(int id);
        IEnumerable<ActorResponse> Get(IEnumerable<int> ids);
        
        int Add(ActorRequest request);
        void Update(int id, ActorRequest request);   
        void Delete(int id);
    }
}
