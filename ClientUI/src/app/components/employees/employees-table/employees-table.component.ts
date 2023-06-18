import { Component, OnInit } from '@angular/core';
import { Employee } from '../../../models/employees.model';

@Component({
  selector: 'app-employees-table',
  templateUrl: './employees-table.component.html',
  styleUrls: ['./employees-table.component.css']
})
export class EmployeesTableComponent {
  employees: Employee[] = [
    {
      id: '1234-1234-1234-1234',
      name: 'Иванов Иван Иванович',
      dateOfBirth: new Date("2018-02-08T10:30:35"),
      dateOfEmployment: new Date("2018-02-08T10:30:35"),
      department: 'IT',
      salary: 80000
    }
  ];
  constructor() { }

  ngOnInit(): void {
    
  }
}
