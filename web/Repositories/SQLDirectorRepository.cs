using Microsoft.EntityFrameworkCore;
using System;
using web.Data;
using web.Models.Domain;
using web.Models.DTO;

namespace web.Repositories
{
    public class SQLDirectorRepository : IDirectorRepository
    {
        private readonly DataDbContext _dbContext;

        public SQLDirectorRepository(DataDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public List<DirectorIdDTO> GetAllDirectors()
        {
            var directors = _dbContext.Directors.Include(f => f.FilmDirectors).ToList();

            List<DirectorIdDTO> directorDTOs = directors.Select(f => new DirectorIdDTO
            {
                Id = f.Id,
                FirstName = f.FirstName,
                LastName = f.LastName,
                DateOfBirth = f.DateOfBirth,
                Film = f.FilmDirectors?.Select(fa => fa.FilmId).ToList()
            }).ToList();
            return directorDTOs;
        }

        public Directors GetDirectorById(int id)
        {
            var director = _dbContext.Directors.FirstOrDefault(m => m.Id == id);

            if (director != null)
                return director;

            throw new NotImplementedException();
        }

        public DirectorDTO AddDirector(DirectorDTO directorDTO)
        {
            if (directorDTO != null)
            {
                Directors director = new Directors();
                director.FirstName = directorDTO.FirstName;
                director.LastName = directorDTO.LastName;
                director.DateOfBirth = directorDTO.DateOfBirth;
                _dbContext.Directors.Add(director);
                _dbContext.SaveChanges();

                int directorId = director.Id;

                if (directorDTO.Film != null)
                {
                    foreach (int filmId in directorDTO.Film)
                    {
                        Films film = _dbContext.Films.FirstOrDefault(m => m.Id == filmId);
                        if (film != null)
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
                return directorDTO;
            }
            throw new NotImplementedException();
        }

        public DirectorDTO UpdateDirectorById(int id, DirectorDTO directorDTO)
        {
            var director = _dbContext.Directors.FirstOrDefault(m => m.Id == id);

            if (director != null)
            {
                director.FirstName = directorDTO.FirstName;
                director.LastName = directorDTO.LastName;
                director.DateOfBirth = directorDTO.DateOfBirth;
                _dbContext.SaveChanges();

                int directorId = director.Id;

                if (directorDTO.Film != null)
                {
                    var listOld = _dbContext.FilmDirectors.Where(f => f.DirectorId == directorId).ToList();

                    foreach (var item in listOld)
                    {
                        _dbContext.FilmDirectors.Remove(item);
                        _dbContext.SaveChanges();
                    }

                    foreach (int filmId in directorDTO.Film)
                    {
                        Films film = _dbContext.Films.FirstOrDefault(m => m.Id == filmId);
                        if (film != null)
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
                return directorDTO;
            }
            throw new NotImplementedException();
        }

        public Directors DeleteDirectorById(int id)
        {
            var director = _dbContext.Directors.FirstOrDefault(m => m.Id == id);

            if (director != null)
            {
                _dbContext.Directors.Remove(director);
                _dbContext.SaveChanges();
                return director;
            }

            throw new NotImplementedException();
        }
    }
}
