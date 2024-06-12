namespace web.Models.DTO
{
    public class FilmActors
    {
        public int FilmId { get; set; }

        public int ActorId { get; set; }

        public Films? Film { get; set; }

        public Actors? Actor { get; set; }
    }
}
