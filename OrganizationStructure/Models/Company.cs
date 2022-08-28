using System.ComponentModel.DataAnnotations.Schema;

namespace OrganizationStructure.Models
{
    public class Company : IOrganizationNode
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public int? LeaderId { get; set; }



    }
}
