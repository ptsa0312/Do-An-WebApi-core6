using Microsoft.EntityFrameworkCore;
using System;
using web.Data;
using web.Models.Domain;
using web.Models.DTO;

namespace web.Repositories
{
    public class SQLFilmRepository : IFilmRepository
    {
        private readonly DataDbContext _dbContext;

        public SQLFilmRepository(DataDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public List<FilmIdDTO> GetAllFilms(string? filterOn = null, string? filterQuery = null, string? sortBy = null, bool isAscending = true)
        {
            var films = _dbContext.Films.Include(f => f.FilmActors).Include(f => f.FilmDirectors).ToList();

            if (string.IsNullOrWhiteSpace(filterOn) == false && string.IsNullOrWhiteSpace(filterQuery) == false)
                if (filterOn.Equals("name", StringComparison.OrdinalIgnoreCase))
                    films = (List<Films>)films.Where(x => x.Name.Contains(filterQuery)).ToList();

            List<FilmIdDTO> filmDTOs = (List<FilmIdDTO>)films.Select(f => new FilmIdDTO
            {
                Id = f.Id,
                Name = f.Name,
                Description = f.Description,
                Date = f.Date,
                Genre = f.Genre,
                Rate = f.Rate ?? 0,
                Directors = f.FilmDirectors?.Select(fd => fd.DirectorId).ToList(),
                Actors = f.FilmActors?.Select(fa => fa.ActorId).ToList()
            }).ToList();

            if (string.IsNullOrWhiteSpace(sortBy) == false)
            {
                if (sortBy.Equals("name", StringComparison.OrdinalIgnoreCase))
                {
                    films = (List<Films>)(isAscending ? films.OrderBy(x => x.Name) : films.OrderByDescending(x => x.Name)).ToList();
                }
            }
            return filmDTOs;
        }

        public Films GetFilmById(int id)
        {
            var film = _dbContext.Films.FirstOrDefault(m => m.Id == id);

            if (film != null)
                return film;

            throw new NotImplementedException();
        }

        public FilmDTO AddFilm(FilmDTO filmDTO)
        {
            if (filmDTO != null)
            {
                Films film = new Films();
                film.Name = filmDTO.Name;
                film.Description = filmDTO.Description;
                film.Date = filmDTO.Date;
                film.Genre = filmDTO.Genre;
                film.Rate = filmDTO.Rate;
                _dbContext.Films.Add(film);
                _dbContext.SaveChanges();

                int filmId = film.Id;

                if (filmDTO.Directors != null)
                {
                    foreach (int directorId in filmDTO.Directors)
                    {
                        Directors director = _dbContext.Directors.FirstOrDefault(m => m.Id == directorId);
                        if (director != null)
                        {
                            FilmDirectors filmDirector = new FilmDirectors
                            {
                                FilmId = filmId,
                                DirectorId = directorId,
                                Film = film,
                                Director = director
                            };

                            _dbContext.FilmDirectors.Add(filmDirector);
                        }
                    }
                    _dbContext.SaveChanges();
                }

                if (filmDTO.Actors != null)
                {
                    foreach (int actorId in filmDTO.Actors)
                    {
                        Actors actor = _dbContext.Actors.FirstOrDefault(m => m.Id == actorId);

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
                return filmDTO;
            }
            throw new NotImplementedException();
        }

        public FilmDTO UpdateFilmById(int id, FilmDTO filmDTO)
        {
            var film = _dbContext.Films.FirstOrDefault(m => m.Id == id);

            if (film != null)
            {
                film.Name = filmDTO.Name;
                film.Description = filmDTO.Description;
                film.Date = filmDTO.Date;
                film.Genre = filmDTO.Genre;
                film.Rate = filmDTO.Rate;
                _dbContext.SaveChanges();

                int filmId = film.Id;

                if (filmDTO.Directors != null)
                {
                    var listOld = _dbContext.FilmDirectors.Where(f => f.FilmId == filmId).ToList();

                    foreach (var item in listOld)
                    {
                        _dbContext.FilmDirectors.Remove(item);
                        _dbContext.SaveChanges();
                    }

                    foreach (int directorId in filmDTO.Directors)
                    {
                        Directors director = _dbContext.Directors.FirstOrDefault(m => m.Id == directorId);
                        if (director != null)
                        {
                            FilmDirectors filmDirector = new FilmDirectors
                            {
                                FilmId = filmId,
                                DirectorId = directorId,
                                Film = film,
                                Director = director
                            };

                            _dbContext.FilmDirectors.Add(filmDirector);
                        }
                    }

                    _dbContext.SaveChanges();
                }

                if (filmDTO.Actors != null)
                {
                    var listOld = _dbContext.FilmActors.Where(f => f.FilmId == filmId).ToList();

                    foreach (var item in listOld)
                    {
                        _dbContext.FilmActors.Remove(item);
                        _dbContext.SaveChanges();
                    }

                    foreach (int actorId in filmDTO.Actors)
                    {
                        Actors actor = _dbContext.Actors.FirstOrDefault(m => m.Id == actorId);

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
                return filmDTO;
            }

            throw new NotImplementedException();
        }

        public Films DeleteFilmById(int id)
        {
            var film = _dbContext.Films.FirstOrDefault(m => m.Id == id);

            if (film != null)
            {
                _dbContext.Films.Remove(film);
                _dbContext.SaveChanges();
                return film;
            }

            throw new NotImplementedException();
        }
    }
}
