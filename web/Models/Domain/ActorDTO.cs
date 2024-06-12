namespace web.Models.Domain
{
    public class ActorDTO
    {
        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public DateTime DateOfBirth { get; set; }

        public List<int>? Film { get; set; }
    }

    public class ActorIdDTO : ActorDTO
    {
        public int? Id { get; set; }
    }
}
