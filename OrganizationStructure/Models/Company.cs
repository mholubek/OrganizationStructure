using System.ComponentModel.DataAnnotations.Schema;

namespace OrganizationStructure.Models
{
    public class Company : IOrganizationNode
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public int? LeaderId { get; set; }
        public List<Employee>? Employees { get; set; } = new List<Employee>();
        public List<Division>? Divisions { get; set; } = new List<Division>();
    }
}
