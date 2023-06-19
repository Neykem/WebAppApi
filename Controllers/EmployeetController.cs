using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
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
    public class EmployeetController : ControllerBase
    {
        private readonly ILogger<EmployeetController> _logger;
        private readonly ApplicationDbContext _applicationDbContext;

        public EmployeetController(ILogger<EmployeetController> logger, ApplicationDbContext applicationDbContext)
        {
            _logger = logger;
            _applicationDbContext = applicationDbContext;
        }

        [HttpGet]
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

        [HttpPost]
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
    }
}
