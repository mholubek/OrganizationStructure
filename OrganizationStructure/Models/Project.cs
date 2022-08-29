using System.ComponentModel.DataAnnotations.Schema;

namespace OrganizationStructure.Models
{
    public class Project : IOrganizationNode
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public int? LeaderId { get; set; }
        public List<Employee>? Employees { get; set; } = new List<Employee>();
        public List<Department> Departments { get; set; } = new List<Department>();
        public int? DivisionId { get; set; }

    }
}
