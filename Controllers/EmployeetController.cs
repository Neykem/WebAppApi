using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAppApi.Data.DataModels;

namespace WebAppApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EmployeetController : ControllerBase
    {
        private readonly ILogger<EmployeetController> _logger;

        public EmployeetController(ILogger<EmployeetController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public void Get()
        {
            throw new NotImplementedException();
        }
    }
}
