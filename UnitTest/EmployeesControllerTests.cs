using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using WebAppApi.Controllers;
using WebAppApi.Data.DataModels;
using WebAppApi.Domain.Contracts;

namespace WebAppApi
{
    //Сенерировал несколько тестов для проверки изменений
    /// <summary>
    /// Тестовый клас для проверки контроллера по работе с сотрудниками
    /// </summary>
    [TestClass]
    public class EmployeesControllerTests
    {
        private EmployeesController _controller;
        private Mock<IEmployeesRepository> _employeesRepositoryMock;
        private Mock<IMapper> _mapperMock;
        private Mock<ILogger<EmployeesController>> _loggerMock;

        [TestInitialize]
        public void Setup()
        {
            _employeesRepositoryMock = new Mock<IEmployeesRepository>();
            _mapperMock = new Mock<IMapper>();
            _loggerMock = new Mock<ILogger<EmployeesController>>();

            _controller = new EmployeesController(_loggerMock.Object, _employeesRepositoryMock.Object, _mapperMock.Object);
        }

        [TestMethod]
        public async Task GetEmployees_Should_ReturnEmployeesList_When_RepositoryReturnsData()
        {
            // Arrange
            var employees = new List<Employee>
        {
            new Employee { Id = Guid.NewGuid(), Name = "John Doe", Salary = 50000 },
            new Employee { Id = Guid.NewGuid(), Name = "Jane Smith", Salary = 60000 }
        };

            _employeesRepositoryMock.Setup(r => r.GetEmployeesAsync()).ReturnsAsync(employees);

            // Act
            var result = await _controller.GetEmployees() as OkObjectResult;
            var returnedEmployees = result.Value as List<Employee>;

            // Assert
            _employeesRepositoryMock.Verify(r => r.GetEmployeesAsync(), Times.Once);

            Assert.IsNotNull(result);
            Assert.AreEqual(StatusCodes.Status200OK, result.StatusCode);

            Assert.IsNotNull(returnedEmployees);
            Assert.AreEqual(2, returnedEmployees.Count);
            Assert.AreEqual("John Doe", returnedEmployees[0].Name);
            Assert.AreEqual("Jane Smith", returnedEmployees[1].Name);
        }

        [TestMethod]
        public async Task GetEmployees_Should_ReturnEmptyList_When_RepositoryReturnsEmptyList()
        {
            // Arrange
            var employees = new List<Employee>();

            _employeesRepositoryMock.Setup(r => r.GetEmployeesAsync()).ReturnsAsync(employees);

            // Act
            var result = await _controller.GetEmployees() as OkObjectResult;
            var returnedEmployees = result.Value as List<Employee>;

            // Assert
            _employeesRepositoryMock.Verify(r => r.GetEmployeesAsync(), Times.Once);

            Assert.IsNotNull(result);
            Assert.AreEqual(StatusCodes.Status200OK, result.StatusCode);

            Assert.IsNotNull(returnedEmployees);
            Assert.AreEqual(0, returnedEmployees.Count);
        }

        [TestMethod]
        public async Task GetEmployees_Should_ReturnBadRequest_When_RepositoryThrowsDbException()
        {
            // Arrange
            var errorMessage = "Database error.";

            _employeesRepositoryMock.Setup(r => r.GetEmployeesAsync()).Throws(new Exception(errorMessage));

            // Act
            var result = await _controller.GetEmployees() as BadRequestObjectResult;
            var error = result.Value as string;

            // Assert
            _employeesRepositoryMock.Verify(r => r.GetEmployeesAsync(), Times.Once);

            Assert.IsNotNull(result);
            Assert.AreEqual(StatusCodes.Status400BadRequest, result.StatusCode);

            Assert.IsNotNull(error);
            Assert.AreEqual(errorMessage, error);
        }

        [TestMethod]
        public async Task SetEmployeesMinimalSalary_Should_UpdateEmployeesSalaryAndReturnOk()
        {
            // Arrange
            var newSalary = 50000;

            var employees = new List<Employee>
            {
                new Employee { Id = Guid.NewGuid(), Name = "John Doe", Salary = 10000 },
                new Employee { Id = Guid.NewGuid(), Name = "Jane Smith", Salary = 12000 }
            };

            _employeesRepositoryMock.Setup(r => r.GetEmployeesAsync()).ReturnsAsync(employees);

            // Act
            var result = await _controller.SetEmployeesMinimalSalary(newSalary) as OkObjectResult;
            var updatedEmployees = result.Value as List<Employee>;

            // Assert

            Assert.IsNotNull(result);
            Assert.AreEqual(StatusCodes.Status200OK, result.StatusCode);

            Assert.IsNotNull(updatedEmployees);
            Assert.AreEqual(2, updatedEmployees.Count);
            Assert.AreEqual(newSalary, updatedEmployees[0].Salary);
            Assert.AreEqual(newSalary, updatedEmployees[1].Salary);
        }

        [TestMethod]
        public async Task UpdateEmployeeById_Should_UpdateEmployeeAndReturnOk()
        {
            // Arrange
            var employeeId = Guid.NewGuid();
            var employeeUpdateDto = new EmployeeUpdateDto
            {
                Name = "Updated Employee",
                Department = "Updated Department",
                DateOfBirth = new DateTime(1990, 1, 1),
                DateOfEmployment = new DateTime(2020, 1, 1),
                Salary = 50000
            };

            var employee = new Employee
            {
                Id = employeeId,
                Name = "John Doe",
                Department = "IT",
                DateOfBirth = new DateTime(1980, 1, 1),
                DateOfEmployment = new DateTime(2010, 1, 1),
                Salary = 40000
            };

            _employeesRepositoryMock.Setup(r => r.GetEmployeeAsync(employeeId)).ReturnsAsync(employee);

            // Act
            var result = await _controller.UpdateEmployeeById(employeeId, employeeUpdateDto) as OkObjectResult;
            var updatedEmployee = result.Value as Employee;

            // Assert

            _mapperMock.Verify(m => m.Map(employee, employeeUpdateDto), Times.Once);

            Assert.IsNotNull(result);
            Assert.AreEqual(StatusCodes.Status200OK, result.StatusCode);

            Assert.IsNotNull(updatedEmployee);
            Assert.AreEqual(employeeId, updatedEmployee.Id);
            Assert.AreEqual(employeeUpdateDto.Name, updatedEmployee.Name);
            Assert.AreEqual(employeeUpdateDto.Department, updatedEmployee.Department);
            Assert.AreEqual(employeeUpdateDto.DateOfBirth, updatedEmployee.DateOfBirth);
            Assert.AreEqual(employeeUpdateDto.DateOfEmployment, updatedEmployee.DateOfEmployment);
            Assert.AreEqual(employeeUpdateDto.Salary, updatedEmployee.Salary);
        }

        [TestMethod]
        public async Task DeleteOldEmployees_Should_DeleteOldEmployeesAndReturnOk()
        {
            // Arrange
            var age = 40;

            var employees = new List<Employee>
        {
        new Employee
        {
            Id = Guid.NewGuid(),
            Name = "John Doe",
            Department = "IT",
            DateOfBirth = new DateTime(1979, 1, 1),
            DateOfEmployment = new DateTime(2010, 1, 1),
            Salary = 40000
        },
        new Employee
        {
            Id = Guid.NewGuid(),
            Name = "Jane Smith",
            Department = "HR",
            DateOfBirth = new DateTime(1985, 6, 15),
            DateOfEmployment = new DateTime(2012, 5, 1),
            Salary = 50000
        },
        new Employee
        {
            Id = Guid.NewGuid(),
            Name = "Mark Johnson",
            Department = "Sales",
            DateOfBirth = new DateTime(1970, 10, 20),
            DateOfEmployment = new DateTime(2005, 3, 10),
            Salary = 60000
        }
    };

            _employeesRepositoryMock.Setup(r => r.GetEmployeesAsync()).ReturnsAsync(employees);
            _employeesRepositoryMock.Setup(r => r.DeleteEmployeeRangeAsync(It.IsAny<List<Employee>>())).ReturnsAsync(true);

            // Act
            var result = await _controller.DeleteOldEmployees(age) as OkResult;

            // Assert
            _employeesRepositoryMock.Verify(r => r.GetEmployeesAsync(), Times.Once);
            _employeesRepositoryMock.Verify(r => r.DeleteEmployeeRangeAsync(It.IsAny<List<Employee>>()), Times.Once);

            Assert.IsNotNull(result);
            Assert.AreEqual(StatusCodes.Status200OK, result.StatusCode);
        }

        [TestMethod]
        public async Task DeleteEmployee_Should_DeleteEmployeeAndReturnOk()
        {
            // Arrange
            var id = Guid.NewGuid();

            var employee = new Employee
            {
                Id = id,
                Name = "John Doe",
                Department = "IT",
                DateOfBirth = new DateTime(1980, 1, 1),
                DateOfEmployment = new DateTime(2010, 1, 1),
                Salary = 50000
            };

            _employeesRepositoryMock.Setup(r => r.GetEmployeeAsync(id)).ReturnsAsync(employee);
            _employeesRepositoryMock.Setup(r => r.DeleteEmployeeAsync(employee)).ReturnsAsync(true);

            // Act
            var result = await _controller.DeleteEmployee(id) as OkObjectResult;

            // Assert
            _employeesRepositoryMock.Verify(r => r.GetEmployeeAsync(id), Times.Once);
            _employeesRepositoryMock.Verify(r => r.DeleteEmployeeAsync(employee), Times.Once);

            Assert.IsNotNull(result);
            Assert.AreEqual(StatusCodes.Status200OK, result.StatusCode);
            Assert.AreEqual(employee, result.Value);
        }
    }
}
