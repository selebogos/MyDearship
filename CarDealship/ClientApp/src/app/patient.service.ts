import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { map } from 'rxjs/operators';
import { debounceTime } from 'rxjs/internal/operators/debounceTime';
import { Router } from '@angular/router';
import { Observable } from 'rxjs';
import { ClientModel } from './_models/ClientModel';

@Injectable({
  providedIn: 'root'
})
export class PatientService {

  private baseUrllPatient: string = "/api/client/add";
  private baseUrlPatientDetails: string = "/api/client/";
  private baseUrllPatientEdit: string = "/api/client/update/";
  private baseUrllPatientSearch: string = "/api/client/search/?name=";
  constructor(private http: HttpClient, private router: Router) {

  }
  public addPatient(patient: ClientModel)
  {
  try {
      return this.http.post<any>(this.baseUrllPatient,
        patient
       ).pipe(
        map(result => {

          return result;

        }, error => {
          return error;
        }));

    } catch (e) {

    }
  }

  public getPatientDetails(id:string){
    try{

      var  uri = this.baseUrlPatientDetails + id;

        let headers = new HttpHeaders();
        headers.set('Content-Type', 'application/json');

        return this.http.get(uri, { headers });

    } catch(e){
      console.log("Error:problem getting client details "+ e.Message);
      return e.Message;
    }
  }
  public updatePatientDetails(patient:ClientModel) {

    try {
      var  uri = this.baseUrllPatientEdit + patient.Id;
      return this.http.put<any>(uri, patient).pipe(
        map(result => {

          return result;

        }, error => {
          return error;
        }));

    } catch (e) {

    }


  }
  public search(name: string){

    try{

      var  uri = this.baseUrllPatientSearch + name;

        let headers = new HttpHeaders();
        headers.set('Content-Type', 'application/json');

        return this.http.get(uri, { headers }).pipe(debounceTime(500));

    } catch(e){
      console.log("Error:problem searching for client "+ e.Message);
      return e.Message;
    }

  }

}
