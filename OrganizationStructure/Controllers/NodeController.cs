using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OrganizationStructure.Data;
using OrganizationStructure.Models;
using Microsoft.EntityFrameworkCore;

namespace OrganizationStructure.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NodeController : ControllerBase
    {
        private readonly OrganizationStructureDbContext _dbContext;
        private readonly NodeFactory _nodeFactory;

        public NodeController(OrganizationStructureDbContext dbContext)
        {
            _dbContext = dbContext;
            _nodeFactory = new NodeFactory(dbContext);
        }

        [HttpGet("{nodeType}")]
        public IActionResult Get(string nodeType)
        {
            IOrganizationNode? node = null;
            try  //pre pripad zle zadaneho typu uzlu
            {
                node = _nodeFactory.CreateNode(nodeType);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            if (node.GetType() == typeof(Company))
            {
                /*Pri vypise vsetkych firiem nechcem zobrazovat zamestnancov firmy,
                  preto vytvaram anonymnu triedu bez nich  
                */
                var companies = from c in _dbContext.Companies
                                select new
                                {
                                    Id = c.Id,
                                    Name = c.Name,
                                    Code = c.Code,
                                    LeaderId = c.LeaderId
                                };
                return Ok(companies);
            }
            else if (node.GetType() == typeof(Division))
            {
                var divisions = from c in _dbContext.Divisions
                                select new
                                {
                                    Id = c.Id,
                                    Name = c.Name,
                                    Code = c.Code,
                                    LeaderId = c.LeaderId
                                };
                return Ok(divisions);
            }
            else if (node.GetType() == typeof(Project))
            {
                var projects = from c in _dbContext.Projects
                               select new
                               {
                                   Id = c.Id,
                                   Name = c.Name,
                                   Code = c.Code,
                                   LeaderId = c.LeaderId
                               };
                return Ok(projects);
            }
            else if (node.GetType() == typeof(Department))
            {
                var departments = from c in _dbContext.Departments
                                  select new
                                  {
                                      Id = c.Id,
                                      Name = c.Name,
                                      Code = c.Code,
                                      LeaderId = c.LeaderId
                                  };
                return Ok(departments);
            }
            return BadRequest("Invalid type");

        }

        [HttpGet("{id}/{nodeType}")]
        public async Task<IActionResult> Get(int id, string nodeType)
        {
            IOrganizationNode? node = null;
            try  //pre pripad zle zadaneho typu uzlu
            {
                node = _nodeFactory.CreateConcreteNode(nodeType, id).Result;
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            if (node == null)
                return NotFound("No record found against this id");

            return Ok(node);
        }

        [HttpPost("{nodeType}/{name}/{code}/{leaderId}")]
        public async Task<IActionResult> Post(string nodeType, string name, string code, int leaderId = 0)
        {
            IOrganizationNode? newNode = null;
            try  //pre pripad zle zadaneho typu uzlu
            {
                newNode = _nodeFactory.CreateNode(nodeType);
                newNode.Name = name;
                newNode.Code = code;
                newNode.LeaderId = leaderId;
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

        [HttpPut("{nodeType}/{id}/{name}/{code}/{leaderId}")]
        public async Task<IActionResult> Put(string nodeType, int id, string name, string code, int leaderId = 0)
        {
            IOrganizationNode? node = null;
            try  //pre pripad zle zadaneho typu uzlu
            {
                node = _nodeFactory.CreateConcreteNode(nodeType, id).Result;
                node.Name = name;
                node.Code = code;
                node.LeaderId = leaderId;
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

        [HttpDelete("{nodeType}/{id}")]
        public async Task<IActionResult> Delete(string nodeType, int id)
        {
            IOrganizationNode? node = null;
            try
            {
                node = _nodeFactory.CreateConcreteNode(nodeType, id).Result;
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
