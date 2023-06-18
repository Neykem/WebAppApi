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
      dateOfBirth: Date.prototype,
      dateOfEmployment: Date.prototype,
      department: 'IT',
      salary: 80000
    }
  ];
  constructor() { }

  ngOnInit(): void {
    
  }
}
