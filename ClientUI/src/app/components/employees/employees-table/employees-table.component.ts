import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { EmployeesService } from 'src/app/services/employees.service';
import { Employee } from '../../../models/employees.model';

@Component({
  selector: 'app-employees-table',
  templateUrl: './employees-table.component.html',
  styleUrls: ['./employees-table.component.css'],
  providers: [EmployeesService]
})
export class EmployeesTableComponent implements OnInit {
  employees: Employee[] = [];
  constructor(private employeesService: EmployeesService, private router: Router) { }

  ngOnInit(): void {
    this.employeesService.getEmployees()
    .subscribe({
      next: (employees) => {
        this.employees = employees;
    },
    error: (response) => {
      console.log(response);
      this.router.navigate(['employees'])
    }})
  }
  getEmployeesWithSalaryParam(): void{
    this.employeesService.getEmployeesWithSalaryParam()
    .subscribe({
      next: (employees) => {
        this.employees = employees;
      },
    error: (response) => {
      console.log(response);
      this.router.navigate(['employees'])
    }})
  }

  setEmployeesMinimalSalary(): void {
    this.employeesService.setEmployeesMinimalSalary()
    .subscribe(
      response => {
        this.router.navigate(['employees'])
      },
      error => {
        console.error(error);
        this.router.navigate(['employees'])
      })
  }

  deleteOldEmployees(): void {
    this.employeesService.deleteOldEmployees()
    .subscribe(
      response => {
        this.router.navigate(['employees']);
      },
      error => {
        console.error(error);
        this.router.navigate(['employees']);
      }
    );
  }
  deleteEmployee(id: string){
    this.employeesService.deleteEmployee(id)
    .subscribe(
      response => {
        window.location.reload();
      },
      error => {
        console.error(error);
        this.router.navigate(['employees']);
      }
    );
  }
}
