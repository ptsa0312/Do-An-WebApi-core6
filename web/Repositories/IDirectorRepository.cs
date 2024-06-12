using web.Models.Domain;
using web.Models.DTO;

namespace web.Repositories
{
    public interface IDirectorRepository
    {
        List<DirectorIdDTO> GetAllDirectors();

        Directors GetDirectorById(int id);

        DirectorDTO AddDirector(DirectorDTO directorDTO);

        DirectorDTO UpdateDirectorById(int id, DirectorDTO directorDTO);

        Directors DeleteDirectorById(int id);
    }
}
