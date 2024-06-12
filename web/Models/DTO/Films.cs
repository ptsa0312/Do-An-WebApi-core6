using System.ComponentModel.DataAnnotations;

namespace web.Models.DTO
{
    public class Films
    {
        [Key]
        public int Id { get; set; }

        public string? Name { get; set; }

        public string? Description { get; set; }

        public DateTime Date { get; set; }

        public string? Genre { get; set; }

        public decimal? Rate { get; set; }

        public List<FilmActors>? FilmActors { get; set; }

        public List<FilmDirectors>? FilmDirectors { get; set; }
    }
}
