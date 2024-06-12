using web.Models.Domain;
using web.Models.DTO;

namespace web.Repositories
{
    public interface IActorRepository
    {
        List<ActorIdDTO> GetAllActors();

        Actors GetActorById(int id);

        ActorDTO AddActor(ActorDTO actorDTO);

        ActorDTO UpdateActorById(int id, ActorDTO actorDTO);

        Actors DeleteActorById(int id);
    }
}
