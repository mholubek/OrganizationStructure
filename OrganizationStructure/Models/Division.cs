using System.ComponentModel.DataAnnotations.Schema;

namespace OrganizationStructure.Models
{
    public class Division : IOrganizationNode
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public int? LeaderId { get; set; }



    }
}
