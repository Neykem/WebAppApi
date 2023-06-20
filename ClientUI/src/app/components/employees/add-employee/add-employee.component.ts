import { Component, OnInit } from '@angular/core';
import { Route, Router } from '@angular/router';
import { Employee } from 'src/app/models/employees.model';
import { EmployeesService } from 'src/app/services/employees.service';

@Component({
  selector: 'app-add-employee',
  templateUrl: './add-employee.component.html',
  styleUrls: ['./add-employee.component.css'],
  providers: [EmployeesService]
})
export class AddEmployeeComponent implements OnInit {
  addEmployeeEntity: Employee = {
      id: '',
      name: '',
      dateOfBirth: new Date(),
      dateOfEmployment: new Date(),
      department: '',
      salary: 0
  }

  constructor(private employeesService: EmployeesService, private router: Router ){}

  ngOnInit(): void {
      
  }
  addEmployee(){
    this.employeesService.addEmployees(this.addEmployeeEntity)
    .subscribe({
      next: (employee) => {
        this.router.navigate(['employees'])
      }
    });
  }
}
