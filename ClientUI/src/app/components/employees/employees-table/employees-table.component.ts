import { Component, OnInit } from '@angular/core';
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
  constructor(private employeesService: EmployeesService) { }

  ngOnInit(): void {
    this.employeesService.getEmployees()
    .subscribe({
      next: (employees) => {
        this.employees = employees;
    },
    error: (response) => {
      console.log(response);
    }
    })

    
  }
}
