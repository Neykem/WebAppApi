using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using WebAppApi.Data.DataModels;
using WebAppApi.Domain;
using WebAppApi.Domain.Contracts;

namespace WebAppApi.Controllers
{
    /// <summary>
    /// Контроллер отвечающий за управление данными сотрудников
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class EmployeesController : ControllerBase
    {
        private readonly ILogger<EmployeesController> _logger; //Добавил запись логов
        private readonly IEmployeesRepository _employeesRepository; //Обращение к контексту переделал через репозиторий
        private readonly IMapper _mapper; //Обновление сущностей переделал через AutoMapper

        public EmployeesController(ILogger<EmployeesController> logger, IEmployeesRepository employeesRepository, IMapper mapper)
        {
            _logger = logger;
            _employeesRepository = employeesRepository;
            _mapper = mapper;
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
                var employeeList = await _employeesRepository.GetEmployeesAsync();
                if (employeeList == null || employeeList.Count == 0)
                {
                    _logger.LogWarning("По запросу возвращен пустой результат!");
                    return Ok(employeeList);
                }
                return Ok(employeeList);
            }
            catch (DbException ex)
            {
                _logger.LogError(ex, "Ошибка обращения к Базе Данных!");
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Неизвестная ошибка при получений данных!");
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
                var employees = await _employeesRepository.GetEmployeesAsync();

                if (employees == null || employees.Count == 0)
                {
                    return NotFound();
                }

                else
                {
                    return Ok(employees.Where(o => o.Salary >= salary));
                }

            }
            catch (DbException ex)
            {
                _logger.LogError(ex, "Ошибка обращения к Базе Данных!");
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Неизвестная ошибка при получений данных!");
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
                var employeeSelect = await _employeesRepository.GetEmployeeAsync(id);

                if (employeeSelect == null)
                {
                    return NotFound();
                }

                else
                {
                    return Ok(employeeSelect);
                }

            }
            catch (DbException ex)
            {
                _logger.LogError(ex, "Ошибка обращения к Базе Данных!");
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Неизвестная ошибка при получений данных!");
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
                var buffList = await _employeesRepository.GetEmployeesAsync();
                buffList.Where(o => o.Salary < new_salary).ToList().ForEach(o => o.Salary = new_salary);
                await _employeesRepository.UpdateEmployeeRangeAsync(buffList);
                return Ok(await _employeesRepository.GetEmployeesAsync());
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Ошибка при обновлений данных!");
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Неизвестная ошибка!");
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
                if (await _employeesRepository.CreateEmployeeAsync(employee) == false)
                {
                    _logger.LogWarning("Ошибка при попытке создание нового сотрудника!");
                    return BadRequest("Не удалось добавить нового сторудника!");
                }
                return Ok(employee);
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Ошибка при обновлений данных!");
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Неизвестная ошибка!");
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Обновление сотрудника оп id
        /// <paramref name="id"/>
        /// <paramref name="employeeUpdate"/>
        /// </summary>
        [HttpPut("&id={id}")]
        public async Task<IActionResult> UpdateEmployeeById(Guid id, [FromBody] EmployeeUpdateDto employeeUpdate)
        {
            try
            {
                var employeeSelect = await _employeesRepository.GetEmployeeAsync(id);

                if (employeeSelect == null)
                {
                    return NotFound();
                }
                _mapper.Map(employeeSelect, employeeUpdate);
                if (await _employeesRepository.UpdateEmployeeAsync(employeeSelect) == false)
                {
                    _logger.LogWarning("Ошибка при обновлений данных сторудника!");
                    return BadRequest(employeeSelect);
                }
                return Ok(employeeSelect);
            }
            catch (DbException ex)
            {
                _logger.LogError(ex, "Ошибка при обрашений к Базе Данных!");
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Неизвестная ошибка!");
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
                var buffList = await _employeesRepository.GetEmployeesAsync();
                if (buffList == null || buffList.Count <= 0)
                {
                    return NotFound();
                }
                if (await _employeesRepository.DeleteEmployeeRangeAsync(buffList
                    .Where(o => (DateTime.Now.Year - o.DateOfBirth.Year) >= age).ToList())
                    == false)
                {
                    _logger.LogWarning("Ошибка при удалений старых сторудников!");
                    return BadRequest(buffList);
                }
                _logger.LogInformation("Был удален список старых сотрудников!");
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Неизвестная ошибка при удалений старых сотрудников!");
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
                var employeeSelect = await _employeesRepository.GetEmployeeAsync(id);
                if (employeeSelect == null)
                {
                    return NotFound();
                }
                if (await _employeesRepository.DeleteEmployeeAsync(employeeSelect) == false)
                {
                    _logger.LogWarning("Ошибка при обновлений данных сторудника!");
                    return BadRequest(employeeSelect);
                }
                _logger.LogInformation("Был удален объект БД!" + employeeSelect);
                return Ok(employeeSelect);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Неизвестная ошибка при удалений сотрудника!");
                return BadRequest(ex.Message);
            }
        }
    }
}
