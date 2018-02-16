using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPI.Models;
using WebAPI.Models.Entities;
using WebAPI.Models.ViewModels;

namespace WebAPI.Controllers
{
    //[Produces("application/json")]
    [Route("api/Employee")]
    public class EmployeeController : Controller
    {
        private readonly WebApiContext _context;

        public EmployeeController(WebApiContext context)
        {
            _context = context;
        }

        // GET: api/Employee
        [HttpGet]
        [Route("Employees")]
        public IEnumerable<Employee> GetEmployees()
        {
            return _context.Employee.ToList();
        }

        // GET: api/Employee/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetEmployee([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var employee = await _context.Employee.SingleOrDefaultAsync(m => m.employeeId == id);

            if (employee == null)
            {
                return NotFound();
            }

            return Ok(employee);
        }

        // PUT: api/Employee/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEmployee([FromRoute] int id, [FromBody] Employee employee)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != employee.employeeId)
            {
                return BadRequest();
            }

            _context.Entry(employee).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EmployeeExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Employee
        [HttpPost]
        [Route("AddEmployee")]
        public async Task<IActionResult> PostEmployee([FromBody] EmployeeViewModel employee)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var newEmployee = new Employee
            {
                firstName = employee.FirstName,
                lastName = employee.LastName,
                empCode = employee.EmpCode,
                position = employee.Position,
                office = employee.Office
            };

            _context.Employee.Add(newEmployee);
            await _context.SaveChangesAsync();

            //var retEmployee = new Employee
            //{
            //    EmployeeId = newEmployee.EmployeeId,
            //    FirstName = newEmployee.FirstName,
            //    LastName = newEmployee.LastName,
            //    EmpCode = newEmployee.EmpCode,
            //    Position = newEmployee.Position,
            //    Office = newEmployee.Office
            //};
            //var vm = new Employee
            //{
            //    orderId = newOrder.Id,
            //    orderDate = newOrder.OrderDate,
            //    orderNumber = newOrder.OrderNumber
            //};

            return Created($"/api/Employee/{newEmployee.employeeId}", newEmployee);
        }

        // DELETE: api/Employee/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmployee([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var employee = await _context.Employee.SingleOrDefaultAsync(m => m.employeeId == id);
            if (employee == null)
            {
                return NotFound();
            }

            _context.Employee.Remove(employee);
            await _context.SaveChangesAsync();

            return Ok(employee);
        }

        private bool EmployeeExists(int id)
        {
            return _context.Employee.Any(e => e.employeeId == id);
        }
    }
}