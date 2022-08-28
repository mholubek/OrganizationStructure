namespace OrganizationStructure.Models
{
    public interface IOrganizationNode
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public int? LeaderId { get; set; }

        void ChangeLeader(int employeeId);
    }
}
