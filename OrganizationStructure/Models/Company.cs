using System.ComponentModel.DataAnnotations.Schema;

namespace OrganizationStructure.Models
{
    public class Company : IOrganizationNode
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public int? LeaderId { get; set; }
        [NotMapped]
        public Employee Leader { get; set; }
        [NotMapped]
        public List<Employee> Employees { get; set; }

        public void ChangeLeader(int employeeId)
        {
            LeaderId = employeeId;
        }
    }
}
