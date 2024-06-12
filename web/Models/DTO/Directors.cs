using System.ComponentModel.DataAnnotations;

namespace web.Models.DTO
{
    public class Directors
    {
        [Key]
        public int Id { get; set; }

        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public DateTime DateOfBirth { get; set; }

        public List<FilmDirectors>? FilmDirectors { get; set; }
    }
}
