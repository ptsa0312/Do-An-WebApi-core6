using Microsoft.EntityFrameworkCore;
using System;
using web.Data;
using web.Models.Domain;
using web.Models.DTO;

namespace web.Repositories
{
    public class SQLActorRepository : IActorRepository
    {
        private readonly DataDbContext _dbContext;

        public SQLActorRepository(DataDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public List<ActorIdDTO> GetAllActors()
        {
            var actors = _dbContext.Actors.Include(m => m.FilmActors).ToList();

            List<ActorIdDTO> actorDTOs = actors.Select(f => new ActorIdDTO
            {
                Id = f.Id,
                FirstName = f.FirstName,
                LastName = f.LastName,
                DateOfBirth = f.DateOfBirth,
                Film = f.FilmActors?.Select(fa => fa.FilmId).ToList()
            }).ToList();
            return actorDTOs;
        }

        public Actors GetActorById(int id)
        {
            var actor = _dbContext.Actors.SingleOrDefault(m => m.Id == id);

            if (actor != null)
                return actor;

            throw new NotImplementedException();
        }

        public ActorDTO AddActor(ActorDTO actorDTO)
        {
            if (actorDTO != null)
            {
                Actors actor = new Actors();
                actor.FirstName = actorDTO.FirstName;
                actor.LastName = actorDTO.LastName;
                actor.DateOfBirth = actorDTO.DateOfBirth;
                _dbContext.Actors.Add(actor);
                _dbContext.SaveChanges();

                int actorId = actor.Id;

                if (actorDTO.Film != null)
                {
                    foreach (int filmId in actorDTO.Film)
                    {
                        Films film = _dbContext.Films.FirstOrDefault(m => m.Id == filmId);

                        if (film != null)
                        {
                            FilmActors filmActor = new FilmActors
                            {
                                FilmId = filmId,
                                ActorId = actorId,
                                Film = film,
                                Actor = actor
                            };
                            _dbContext.FilmActors.Add(filmActor);
                        }
                    }
                    _dbContext.SaveChanges();
                }
                return actorDTO;
            }
            throw new NotImplementedException();
        }

        public ActorDTO UpdateActorById(int id, ActorDTO actorDTO)
        {
            var actor = _dbContext.Actors.FirstOrDefault(m => m.Id == id);

            if (actor != null)
            {
                actor.FirstName = actorDTO.FirstName;
                actor.LastName = actorDTO.LastName;
                actor.DateOfBirth = actorDTO.DateOfBirth;
                _dbContext.SaveChanges();

                int actorId = actor.Id;

                if (actorDTO.Film != null)
                {
                    var listOld = _dbContext.FilmActors.Where(f => f.ActorId == actorId).ToList();

                    foreach (var item in listOld)
                    {
                        _dbContext.FilmActors.Remove(item);
                        _dbContext.SaveChanges();
                    }

                    foreach (int filmId in actorDTO.Film)
                    {
                        Films film = _dbContext.Films.FirstOrDefault(m => m.Id == filmId);

                        if (actor != null)
                        {
                            FilmActors filmActor = new FilmActors
                            {
                                FilmId = filmId,
                                ActorId = actorId,
                                Film = film,
                                Actor = actor
                            };

                            _dbContext.FilmActors.Add(filmActor);
                        }
                    }
                    _dbContext.SaveChanges();
                }

                return actorDTO;
            }
            throw new NotImplementedException();
        }

        public Actors DeleteActorById(int id)
        {
            var actor = _dbContext.Actors.FirstOrDefault(m => m.Id == id);

            if (actor != null)
            {
                _dbContext.Actors.Remove(actor);
                _dbContext.SaveChanges();
                return actor;
            }

            throw new NotImplementedException();
        }
    }
}
