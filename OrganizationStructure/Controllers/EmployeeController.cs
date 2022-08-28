using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OrganizationStructure.Data;
using OrganizationStructure.Models;
using System.Text.RegularExpressions;

namespace OrganizationStructure.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {

        private readonly OrganizationStructureDbContext _dbContext;

        public EmployeeController(OrganizationStructureDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public IActionResult GetAllEmployees()
        {
            return Ok(_dbContext.Employees);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetEmployee(int id)
        {
            Employee employee = await _dbContext.Employees.FindAsync(id);
            if (employee != null)
                return Ok(employee);

            return NotFound("No record found against this id");
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Employee employee)
        {
            if (employee == null)
                return BadRequest("Employee cannot be null");
            if (string.IsNullOrEmpty(employee.FirstName))
                return BadRequest("First name cannot be empty");
            if (string.IsNullOrEmpty(employee.LastName))
                return BadRequest("Last name cannot be empty");
            if (string.IsNullOrEmpty(employee.Email))
                return BadRequest("Email name cannot be empty");
            if (!ValidateEmail(employee.Email))
                return BadRequest("Invalid email address");

            _dbContext.Employees.Add(employee);
            await _dbContext.SaveChangesAsync();
            return StatusCode(StatusCodes.Status201Created, "Successfully created");
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] Employee employee)
        {
            Employee? savedEmployee = _dbContext.Employees.FindAsync(employee.Id).Result;
            if (savedEmployee == null)
            {
                return NotFound("No record found against this id");
            }
            if (string.IsNullOrEmpty(employee.FirstName))
                return BadRequest("First name cannot be empty");
            if (string.IsNullOrEmpty(employee.LastName))
                return BadRequest("Last name cannot be empty");
            if (string.IsNullOrEmpty(employee.Email))
                return BadRequest("Email name cannot be empty");
            if (!ValidateEmail(employee.Email))
                return BadRequest("Invalid email address");

            savedEmployee.Title = employee.Title;
            savedEmployee.FirstName = employee.FirstName;
            savedEmployee.LastName = employee.LastName;
            savedEmployee.PhoneNumber = employee.PhoneNumber;
            savedEmployee.Email = employee.Email;
            await _dbContext.SaveChangesAsync();
            return Ok("Record updated successfully");
        }

        [HttpPut("{employeeId}/{nodeType}/{nodeId}")]
        public async Task<IActionResult> AssignEmployeeToNode(int employeeId, string nodeType, int nodeId)
        {
            Employee employee = await _dbContext.Employees.FirstOrDefaultAsync(e => e.Id == employeeId);
            if (employee == null)
            {
                return NotFound("No employee found against this id");
            }

            IOrganizationNode node = null;
            NodeFactory nodeFactory = new NodeFactory(_dbContext);

            try  //pre pripad zle zadaneho typu
            {
                node = nodeFactory.CreateConcreteNode(nodeType, nodeId).Result;
                if (node == null)
                {
                    return NotFound("No node found against this id");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            node.Employees.Add(employee);
            await _dbContext.SaveChangesAsync();
            return Ok("Successfully updated");

        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            Employee? savedEmployee = _dbContext.Employees.FindAsync(id).Result;
            if (savedEmployee == null)
            {
                return NotFound("No record found against this id");
            }

            _dbContext.Employees.Remove(savedEmployee);
            _dbContext.SaveChanges();
            return Ok("Record deleted successfully");

        }

        /// <summary>
        /// Validate the email address
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        private bool ValidateEmail(string email)
        {
            return Regex.IsMatch(email,
    @"\A(?:[A-Za-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[A-Za-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[A-Za-z0-9](?:[A-Za-z0-9-]*[A-Za-z0-9])?\.)+[A-Za-z0-9](?:[A-Za-z0-9-]*[A-Za-z0-9])?)\Z");

        }


    }
}
