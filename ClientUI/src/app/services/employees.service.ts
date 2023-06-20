import { HttpClient} from '@angular/common/http';
import { Injectable, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { Employee } from '../models/employees.model';

@Injectable()

export class EmployeesService {
  apiUriBase: string = environment.apiUrlBase;
  constructor(private http: HttpClient){

  }
  getEmployees() : Observable<Employee[]>{
      return this.http.get<Employee[]>(this.apiUriBase + '/Employees');
  }
  getEmployeesWithSalaryParam() : Observable<Employee[]>{
      return this.http.get<Employee[]>(this.apiUriBase + '/Employees/&salary=' + environment.selectedSalaryParam);
  }
  getEmployeeById(id: string) : Observable<Employee>{
      return this.http.get<Employee>(this.apiUriBase + '/Employees/&id=' + id);
  }
  setEmployeesMinimalSalary(){
      return this.http.post(this.apiUriBase + '/Employees/&set-minimal-salary=' + environment.minimalSalary,'');
  }
  addEmployees(addEmployeeEntity: Employee) : Observable<Employee>{
      addEmployeeEntity.id = environment.basicGuidTemplate;
      return this.http.post<Employee>(this.apiUriBase + '/Employees/&add-new-employee', addEmployeeEntity);
  }
  updateEmployee(id: string, updateEmployee: Employee) : Observable<Employee>{
      return this.http.put<Employee>(this.apiUriBase + '/Employees/&id=' + id, updateEmployee);
  }
  deleteEmployee(id: string) : Observable<Employee>{
    return this.http.delete<Employee>(this.apiUriBase + '/Employees/&delete-employee=' + id);
  }
  deleteOldEmployees(){
      return this.http.delete(this.apiUriBase + '/Employees/&delete-old-employees=' + environment.ageValueForDel);
  }
}
