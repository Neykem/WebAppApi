using System;

namespace WebAppApi.Data.DataModels
{
    public class EmployeeUpdateDto
    {
        public string Name { get; set; }
        public DateTime DateOfBirth { get; set; }
        public DateTime DateOfEmployment { get; set; }
        public string Department { get; set; }
        public int Salary { get; set; }
    }
}
