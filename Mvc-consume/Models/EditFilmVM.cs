using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using web.Models.Domain;

namespace Mvc_consume.Models
{
    public class EditFilmVM
    {
		/*public int? Id { get; set; }*/
		public string? Name { get; set; }

        public string? Description { get; set; }

        public DateTime Date { get; set; }

        public string? Genre { get; set; }

        public decimal? Rate { get; set; }

        public List<int>? Directors { get; set; }

        public List<int>? Actors { get; set; }
    }
   /* public class FilmIdDTO : EditFilmVM
    {
        public int? Id { get; set; }
    }*/
}
