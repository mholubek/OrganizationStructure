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

        /// <summary>
        /// Get all users
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetAllEmployees()
        {
            return Ok(_dbContext.Employees);
        }

        /// <summary>
        /// Get concrete user by ID
        /// </summary>
        /// <param name="id">Users ID</param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetEmployee(int id)
        {
            Employee employee = await _dbContext.Employees.FindAsync(id);
            if (employee != null)
                return Ok(employee);

            return NotFound("No record found against this id");
        }

        /// <summary>
        /// Create new user
        /// </summary>
        /// <param name="employee">Users data</param>
        /// <returns></returns>
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

            if (employee.CompanyId == 0) 
                employee.CompanyId = null;
            if (employee.DepartmentId == 0)
                employee.DepartmentId = null;
            if (employee.DivisionId == 0)
                employee.DivisionId = null;
            if (employee.ProjectId == 0)
                employee.ProjectId = null;



            _dbContext.Employees.Add(employee);
            try
            {

                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {

                throw;
            }
            return StatusCode(StatusCodes.Status201Created, "Successfully created");
        }

        /// <summary>
        /// Change users data
        /// </summary>
        /// <param name="employee">Users data</param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IActionResult> Put([FromBody] Employee employee)
        {
            Employee? savedEmployee = await _dbContext.Employees.FindAsync(employee.Id);
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
            savedEmployee.CompanyId = employee.CompanyId;
            savedEmployee.DepartmentId = employee.DepartmentId;
            savedEmployee.DivisionId = employee.DivisionId;
            savedEmployee.ProjectId = employee.ProjectId;

            await _dbContext.SaveChangesAsync();
            return Ok("Record updated successfully");
        }

        /// <summary>
        /// Add user into a node
        /// </summary>
        /// <param name="employeeId">Users id</param>
        /// <param name="nodeType">Type of node ('company', 'division', 'project', 'department')</param>
        /// <param name="nodeId">Id of concrete node</param>
        /// <returns></returns>
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

                node = await nodeFactory.CreateConcreteNode(nodeType, nodeId);
                if (node == null)
                {
                    return NotFound("No node found against this id");
                }
                node.Employees.Add(employee);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            await _dbContext.SaveChangesAsync();
            return Ok("Successfully updated");

        }

        /// <summary>
        /// Delete user by ID
        /// </summary>
        /// <param name="id">Users id</param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            Employee? savedEmployee = await _dbContext.Employees.FindAsync(id);
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
