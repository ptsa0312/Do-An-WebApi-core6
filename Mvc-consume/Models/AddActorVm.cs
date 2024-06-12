namespace Mvc_consume.Models
{
    public class AddActorVm
    {
        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public DateTime DateOfBirth { get; set; }

        public List<int>? Film { get; set; }
    }
}
