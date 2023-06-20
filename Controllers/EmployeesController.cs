using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAppApi.Data;
using WebAppApi.Data.DataModels;

namespace WebAppApi.Controllers
{
    /// <summary>
    /// Контроллер отвечающий за управление данными сотрудников
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class EmployeesController : ControllerBase
    {
        private readonly ILogger<EmployeesController> _logger;
        private readonly ApplicationDbContext _applicationDbContext;

        public EmployeesController(ILogger<EmployeesController> logger, ApplicationDbContext applicationDbContext)
        {
            _logger = logger;
            _applicationDbContext = applicationDbContext;
        }

        /// <summary>
        /// Запрос списка всех сотрудников
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(List<Employee>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetEmployees()
        {
            try
            {
                return Ok(await _applicationDbContext.Employees.ToListAsync());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        ///  Запрос на вывод всех сотрудников с указанной зп
        /// <paramref name="salary"/>
        /// </summary>
        /// <returns>Список сотрудников.</returns>
        [HttpGet("&salary={salary}")]
        [ProducesResponseType(typeof(List<Employee>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetEmployeesWithSalaryParam(int salary)
        {
            try
            {
                var employees = await _applicationDbContext.Employees.Where(o => o.Salary >= salary).ToListAsync();

                if (employees == null || employees.Count == 0)
                {
                    return NotFound();
                }

                else
                {
                    return Ok(employees);
                }

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        ///  Запрос на вывод сотрудника с указанным id
        /// <paramref name="id"/>
        /// </summary>
        /// <returns>Сотрудник с текущим id</returns>
        [HttpGet("&id={id}")]
        [ProducesResponseType(typeof(Employee), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetEmployeeById(Guid id)
        {
            try
            {
                var employeeSelect = await _applicationDbContext.Employees.FirstOrDefaultAsync(o=>o.Id == id);

                if (employeeSelect == null)
                {
                    return NotFound();
                }

                else
                {
                    return Ok(employeeSelect);
                }

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Установка новой минимальной зп
        /// <paramref name="new_salary"/>
        /// </summary>
        /// 
        [HttpPost("&set-minimal-salary={new_salary}")]
        public async Task<IActionResult> SetEmployeesMinimalSalary(int new_salary)
        {
            try
            {
                var buffList = await _applicationDbContext.Employees.Where(o => o.Salary < new_salary).ToListAsync();
                buffList.ForEach(o => o.Salary = new_salary);
                _applicationDbContext.UpdateRange(buffList);
                await _applicationDbContext.SaveChangesAsync();
                return Ok(await _applicationDbContext.Employees.ToListAsync());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Добавление нового сотрудника
        /// <paramref name="employee"/>
        /// </summary>
        [HttpPost("&add-new-employee")]
        public async Task<IActionResult> AddEmployee([FromBody] Employee employee)
        {
            try
            {
                employee.Id = Guid.NewGuid();
                await _applicationDbContext.Employees.AddAsync(employee);
                await _applicationDbContext.SaveChangesAsync();

                return Ok(employee);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Обновление сотрудника оп id
        /// <paramref name="id"/>
        /// <paramref name="employeeUpdate"/>
        /// </summary>
        [HttpPut("&id={id}")]
        public async Task<IActionResult> UpdateEmployeeById(Guid id, Employee employeeUpdate)
        {
            try
            {
                var employeeSelect = await _applicationDbContext.Employees.FindAsync(id);

                if (employeeSelect == null)
                {
                    return NotFound();
                }

                employeeSelect.Name = employeeUpdate.Name;
                employeeSelect.Department = employeeUpdate.Department;
                employeeSelect.DateOfBirth = employeeUpdate.DateOfBirth;
                employeeSelect.DateOfEmployment = employeeUpdate.DateOfEmployment;
                employeeSelect.Salary = employeeUpdate.Salary;

                await _applicationDbContext.SaveChangesAsync();
                return Ok(employeeSelect);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        
        /// <summary>
        /// Удаление сотрудников старше указзаного возаста
        /// <paramref name="age"/>
        /// </summary>
        [HttpDelete("&delete-old-employees={age}")]
        public async Task<IActionResult> DeleteOldEmployees(int age)
        {
            try
            {
                var buffList = await _applicationDbContext.Employees.Where(o => (DateTime.Now.Year - o.DateOfBirth.Year) >= age).ToListAsync();
                if (buffList == null || buffList.Count <= 0)
                {
                    return NotFound();
                }
                _applicationDbContext.RemoveRange(buffList);
                _applicationDbContext.SaveChanges();
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Удаление сотрудника по (id)
        /// <paramref name="id"/>
        /// </summary>
        [HttpDelete("&delete-employee={id}")]
        public async Task<IActionResult> DeleteEmployee(Guid id)
        {
            try
            {
                var employeeSelect = await _applicationDbContext.Employees.FindAsync(id);
                if (employeeSelect == null)
                {
                    return NotFound();
                }
                _applicationDbContext.Remove(employeeSelect);
                await _applicationDbContext.SaveChangesAsync();
                return Ok(employeeSelect);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
