using Microsoft.EntityFrameworkCore;
using OrganizationStructure.Data;

namespace OrganizationStructure.Models
{
    public class NodeFactory
    {

        public OrganizationStructureDbContext _dbContext;

        public NodeFactory(OrganizationStructureDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IOrganizationNode CreateNode(string type)
        {
            if (type.ToLower() == "company")
                return new Company();
            if (type.ToLower() == "department")
                return new Department();
            if (type.ToLower() == "division")
                return new Division();
            if (type.ToLower() == "project")
                return new Project();

            throw new ArgumentException("Invalid type");
        }

        public async Task<IOrganizationNode> CreateConcreteNode(string type, int id)
        {
            if (type.ToLower() == "company")
                return await _dbContext.Companies.FirstOrDefaultAsync(c => c.Id == id);
            if (type.ToLower() == "department")
                return await _dbContext.Departments.FirstOrDefaultAsync(c => c.Id == id);
            if (type.ToLower() == "division")
                return await _dbContext.Divisions.FirstOrDefaultAsync(c => c.Id == id);
            if (type.ToLower() == "project")
                return await _dbContext.Projects.FirstOrDefaultAsync(c => c.Id == id);

            throw new ArgumentException("Invalid type");
        }

    }
}
