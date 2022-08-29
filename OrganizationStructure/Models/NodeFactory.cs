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

        public void LoadAssignedUsersToNode(IOrganizationNode node)
        {
            if (node.GetType() == typeof(Company))
                node.Employees = _dbContext.Employees.Where(e => e.CompanyId == node.Id).ToList();
            if (node.GetType() == typeof(Division))
                node.Employees = _dbContext.Employees.Where(e => e.DivisionId == node.Id).ToList();
            if (node.GetType() == typeof(Project))
                node.Employees = _dbContext.Employees.Where(e => e.ProjectId == node.Id).ToList();
            if (node.GetType() == typeof(Department))
                node.Employees = _dbContext.Employees.Where(e => e.DepartmentId == node.Id).ToList();
        }

        public void LoadNestedNodes(IOrganizationNode node)
        {

            if (node.GetType() == typeof(Company))
            {
                (node as Company).Divisions = _dbContext.Divisions.Where(x => x.CompanyId == node.Id).ToList();
                LoadAssignedUsersToNode(node);
                foreach (var division in (node as Company).Divisions)
                {
                    LoadNestedNodes(division);
                }
            }
            if (node.GetType() == typeof(Division))
            {
                (node as Division).Projects = _dbContext.Projects.Where(x => x.DivisionId == node.Id).ToList();
                LoadAssignedUsersToNode(node);
                foreach (var project in (node as Division).Projects)
                {
                    LoadNestedNodes(project);
                }
            }
            if (node.GetType() == typeof(Project))
            {
                (node as Project).Departments = _dbContext.Departments.Where(x => x.ProjectId == node.Id).ToList();
                LoadAssignedUsersToNode(node);
                foreach (var deparment in (node as Project).Departments)
                {
                    LoadNestedNodes(deparment);
                }
            }
            if (node.GetType() == typeof(Department))
            {
                LoadAssignedUsersToNode(node);
            }

        }


    }
}
