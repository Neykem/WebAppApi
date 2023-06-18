using System;
using System.ComponentModel.DataAnnotations;

namespace WebAppApi.Data.DataModels
{
    public class Employee
    {
        [Key]
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTime DateOfBirth { get; set; }
        public DateTime DateOfEmployment { get; set; }
        public string Department { get; set; }
        public int Salary { get; set; }
    }
}
