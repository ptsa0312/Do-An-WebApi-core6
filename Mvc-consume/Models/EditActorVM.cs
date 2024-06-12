using web.Models.Domain;

namespace Mvc_consume.Models
{
    public class EditActorVM
    {
        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public DateTime DateOfBirth { get; set; }

        public List<int>? Film { get; set; }
    }
    public class ActorIdDTO : EditActorVM
    {
        public int? Id { get; set; }
    }
}
