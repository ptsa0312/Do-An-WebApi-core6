namespace web.Models.Domain
{
    public class DirectorDTO
    {
        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public DateTime DateOfBirth { get; set; }

        public List<int>? Film { get; set; }
    }

    public class DirectorIdDTO : DirectorDTO
    {
        public int? Id { get; set; }
    }
}
