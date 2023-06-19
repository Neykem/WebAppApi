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
      return this.http.get<Employee[]>(this.apiUriBase + '/Employeet');
  }
}
