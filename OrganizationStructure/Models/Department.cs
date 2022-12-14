using System.ComponentModel.DataAnnotations.Schema;

namespace OrganizationStructure.Models
{
    public class Department : IOrganizationNode
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public int? LeaderId { get; set; }
        public List<Employee>? Employees { get; set; } = new List<Employee>();
        public int? ProjectId { get; set; }
    }
}
