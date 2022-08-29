using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OrganizationStructure.Data;
using OrganizationStructure.Models;
using Microsoft.EntityFrameworkCore;

namespace OrganizationStructure.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrganizationNodeController : ControllerBase
    {
        private readonly OrganizationStructureDbContext _dbContext;
        private readonly NodeFactory _nodeFactory;

        public OrganizationNodeController(OrganizationStructureDbContext dbContext)
        {
            _dbContext = dbContext;
            _nodeFactory = new NodeFactory(dbContext);
        }

        /// <summary>
        /// Get all nodes
        /// </summary>
        /// <param name="nodeType">Type of node ('company', 'division', 'project', 'department')</param>
        /// <returns></returns>
        [HttpGet("{nodeType}")]
        public IActionResult Get(string nodeType)
        {

            if (nodeType.ToLower() == "company")
            {
                List<Company> companies = _dbContext.Companies.ToList();
                foreach (var company in companies)
                {
                    _nodeFactory.LoadNestedNodes(company);
                }
                return Ok(companies);
            }
            if (nodeType.ToLower() == "division")
            {
                List<Division> divisions = _dbContext.Divisions.ToList();
                foreach (var division in divisions)
                {
                    _nodeFactory.LoadNestedNodes(division);
                }
                return Ok(divisions);
            }
            if (nodeType.ToLower() == "project")
            {
                List<Project> projects = _dbContext.Projects.ToList();
                foreach (var project in projects)
                {
                    _nodeFactory.LoadNestedNodes(project);
                }
                return Ok(projects);
            }
            if (nodeType.ToLower() == "department")
            {
                List<Department> departments = _dbContext.Departments.ToList();
                foreach (var department in departments)
                {
                    _nodeFactory.LoadNestedNodes(department);
                }
                return Ok(departments);
            }

            return BadRequest("Invalid type");
        }

        /// <summary>
        /// Get concrete node
        /// </summary>
        /// <param name="id">Id of concrete node</param>
        /// <param name="nodeType">Type of node ('company', 'division', 'project', 'department')</param>
        /// <returns></returns>
        [HttpGet("{id}/{nodeType}")]
        public async Task<IActionResult> Get(int id, string nodeType)
        {
            IOrganizationNode? node = null;
            try  //pre pripad zle zadaneho typu uzlu
            {
                node = await _nodeFactory.CreateConcreteNode(nodeType, id);
                _nodeFactory.LoadNestedNodes(node);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            if (node == null)
                return NotFound("No record found against this id");

            return Ok(node);
        }

        /// <summary>
        /// Create new node
        /// </summary>
        /// <param name="nodeType">Type of node ('company', 'division', 'project', 'department')</param>
        /// <param name="name">Name of node</param>
        /// <param name="code">Code of node</param>
        /// <returns></returns>
        [HttpPost("{nodeType}/{name}/{code}")]
        public async Task<IActionResult> Post(string nodeType, string name, string code, int leaderId = 0)
        {
            IOrganizationNode? newNode = null;
            try  //pre pripad zle zadaneho typu uzlu
            {
                newNode = _nodeFactory.CreateNode(nodeType);
                newNode.Name = name;
                newNode.Code = code;

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            if (string.IsNullOrEmpty(newNode.Name))
                return BadRequest("Company name cannot be empty");
            if (string.IsNullOrEmpty(newNode.Code))
                return BadRequest("Company code cannot be empty");

            _dbContext.AddNode(newNode);
            await _dbContext.SaveChangesAsync();
            return StatusCode(StatusCodes.Status201Created, "Successfully created");
        }

        /// <summary>
        /// Edit concrete node
        /// </summary>
        /// <param name="nodeType">Type of node ('company', 'division', 'project', 'department')</param>
        /// <param name="id">Id of concrete node</param>
        /// <param name="name">Name of node</param>
        /// <param name="code">Code of node</param>
        /// <param name="leaderId">Leaders ID (optional)</param>
        /// <returns></returns>
        [HttpPut("{nodeType}/{id}/{name}/{code}/{leaderId}")]
        public async Task<IActionResult> Put(string nodeType, int id, string name, string code, int leaderId = 0)
        {
            IOrganizationNode? node = null;
            try  //pre pripad zle zadaneho typu uzlu
            {
                node = await _nodeFactory.CreateConcreteNode(nodeType, id);

                if (node == null)
                {
                    return NotFound("No record found against this id");
                }

                node.Name = name;
                node.Code = code;
                if (node.Employees.Find(e => e.Id == leaderId) != null)  //kontrola ci zamestnanec pracuje v spolocnosti
                    node.LeaderId = leaderId;
                else
                    return BadRequest("The selected employee is not an employee of the node");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            if (string.IsNullOrEmpty(node.Name))
                return BadRequest("Company name cannot be empty");
            if (string.IsNullOrEmpty(node.Code))
                return BadRequest("Company code cannot be empty");

            await _dbContext.SaveChangesAsync();
            return StatusCode(StatusCodes.Status201Created, "Successfully updated");
        }

        /// <summary>
        /// Assign employee into a node
        /// </summary>
        /// <param name="nodeType">Type of node ('company', 'division', 'project', 'department')</param>
        /// <param name="nodeId">Id of concrete node</param>
        /// <param name="employeeId">Id of employee</param>
        /// <param name="remove">Remove employee from node (optional)</param>
        /// <returns></returns>
        [HttpPut("{nodeType}/{nodeId}/{employeeId}/{remove}")]
        public async Task<IActionResult> AssignEmployee(string nodeType, int nodeId, int employeeId, bool remove = false)
        {
            IOrganizationNode? node = null;
            try  //pre pripad zle zadaneho typu uzlu
            {
                node = await _nodeFactory.CreateConcreteNode(nodeType, nodeId);
                if (node == null)
                {
                    return NotFound("No record found against this id");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            Employee employee = await _dbContext.Employees.FirstOrDefaultAsync(e => e.Id == employeeId);
            if (employee == null)
            {
                return NotFound("No record found against this id");
            }
            if (remove)
            {
                node.Employees.Remove(employee);
            }
            else
            {
                if (!node.Employees.Contains(employee))  //check if user is already there
                    node.Employees.Add(employee);
                else
                    return BadRequest("User is already in company");
            }
            if (node.GetType() == typeof(Company))
            {
                if (!remove)
                    employee.CompanyId = node.Id;
                else
                    employee.CompanyId = null;
            }
            else if (node.GetType() == typeof(Division))
            {
                if (!remove)
                    employee.DivisionId = node.Id;
                else
                    employee.DivisionId = null;
            }
            else if (node.GetType() == typeof(Project))
            {
                if (!remove)
                    employee.ProjectId = node.Id;
                else
                    employee.ProjectId = null;
            }
            else if (node.GetType() == typeof(Department))
            {
                if (!remove)
                    employee.DepartmentId = node.Id;
                else
                    employee.DepartmentId = null;
            }

            await _dbContext.SaveChangesAsync();

            return Ok("Successfully updated");

        }

        /// <summary>
        /// Assign nested node to parent node
        /// </summary>
        /// <param name="nodeType">Type of parent node ('company', 'division', 'project', 'department')</param>
        /// <param name="nodeId">Id of concrete node</param>
        /// <param name="nestedNodeId">Id of nested node</param>
        /// <param name="remove">Remove nested node from parent node (optional)</param>
        /// <returns></returns>
        [HttpPut("{nodeType}/{nodeId}/{nestedNodeId}")]
        public async Task<IActionResult> AddNestedNode(string nodeType, int nodeId, int nestedNodeId, bool remove = false)
        {
            IOrganizationNode? node = null;
            IOrganizationNode? nestedNode = null;
            string nestedNodeType = "";
            if (nodeType.ToLower() == "company")
            {
                nestedNodeType = "division";
            }
            if (nodeType.ToLower() == "division")
            {
                nestedNodeType = "project";
            }
            if (nodeType.ToLower() == "project")
            {
                nestedNodeType = "department";
            }
            if (nodeType.ToLower() == "department")
            {
                return BadRequest("The department cannot have a nested object");
            }

            try  //pre pripad zle zadaneho typu uzlu
            {
                node = await _nodeFactory.CreateConcreteNode(nodeType, nodeId);
                if (node == null)
                {
                    return NotFound("No node found against this id");
                }
                nestedNode = await _nodeFactory.CreateConcreteNode(nestedNodeType, nestedNodeId);
                if (nestedNode == null)
                {
                    return NotFound("No nested node found against this id");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            if (node.GetType() == typeof(Company))
            {
                if (!remove)
                    ((Company)node).Divisions.Add((Division)nestedNode);
                else
                    ((Company)node).Divisions.Remove((Division)nestedNode);
            }
            if (node.GetType() == typeof(Division))
            {
                if (!remove)
                    ((Division)node).Projects.Add((Project)nestedNode);
                else
                    ((Division)node).Projects.Remove((Project)nestedNode);
            }
            if (node.GetType() == typeof(Project))
            {
                if (!remove)
                    ((Project)node).Departments.Add((Department)nestedNode);
                else
                    ((Project)node).Departments.Remove((Department)nestedNode);
            }

            await _dbContext.SaveChangesAsync();
            return Ok("Successfully saved");

        }

        /// <summary>
        /// Delete node
        /// </summary>
        /// <param name="nodeType">Type of node ('company', 'division', 'project', 'department')</param>
        /// <param name="id">Id of concrete node</param>
        /// <returns></returns>
        [HttpDelete("{nodeType}/{id}")]
        public async Task<IActionResult> Delete(string nodeType, int id)
        {
            IOrganizationNode? node = null;
            try
            {
                node = await _nodeFactory.CreateConcreteNode(nodeType, id);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            _dbContext.RemoveNode(node);
            await _dbContext.SaveChangesAsync();
            return StatusCode(StatusCodes.Status201Created, "Successfully deleted");
        }

    }
}
