namespace Mvc_consume.Models
{
	public class AddFilmVM
	{
		public string? Name { get; set; }

		public string? Description { get; set; }

		public DateTime Date { get; set; }

		public string? Genre { get; set; }

		public decimal? Rate { get; set; }

		public List<int>? Directors { get; set; }

		public List<int>? Actors { get; set; }
	}
}
