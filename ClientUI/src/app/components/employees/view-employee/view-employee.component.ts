import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Employee } from 'src/app/models/employees.model';
import { EmployeesService } from 'src/app/services/employees.service';

@Component({
  selector: 'app-view-employee',
  templateUrl: './view-employee.component.html',
  styleUrls: ['./view-employee.component.css'],
  providers: [EmployeesService]
})
export class ViewEmployeeComponent implements OnInit {
  employeeSelected: Employee = {
      id: '',
      name: '',
      dateOfBirth: new Date(),
      dateOfEmployment: new Date(),
      department: '',
      salary: 0
  }

  constructor(private route: ActivatedRoute, private employeeService: EmployeesService, private router: Router){

  }

  ngOnInit(): void {
      this.route.paramMap.subscribe({
        next: (params) => {
          const id = params.get('id');
          if(id){
           this.employeeService.getEmployeeById(id).subscribe({
            next: (response) => {
              this.employeeSelected = response;
            }
           })
          }
        }})
  }

  updateEmployee(){
    this.employeeService.updateEmployee(this.employeeSelected.id, this.employeeSelected)
    .subscribe({
      next: (employee) => {
        this.router.navigate(['employees'])
      }
    });
  }
}
