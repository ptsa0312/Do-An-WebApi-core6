using web.Models.Domain;

namespace Mvc_consume.Models
{
    public class ActorViewModel
    {
        public int Id { get; set; }

        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public DateTime DateOfBirth { get; set; }

        public List<int>? Film { get; set; }
    }

}
