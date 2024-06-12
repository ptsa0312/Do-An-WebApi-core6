using System.ComponentModel.DataAnnotations;

namespace web.Models.Domain
{
    public class FilmDTO
    {
        [Required]
        [MaxLength(30)]
        public string? Name { get; set; }

        public string? Description { get; set; }

        public DateTime Date { get; set; }

        public string? Genre { get; set; }

        public decimal? Rate { get; set; }

        public List<int>? Directors { get; set; }

        public List<int>? Actors { get; set; }
    }

    public class FilmIdDTO : FilmDTO
    {
        public int? Id { get; set; }
    }
}
