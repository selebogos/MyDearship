import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { map } from 'rxjs/operators';
import { debounceTime } from 'rxjs/internal/operators/debounceTime';
import { Router } from '@angular/router';
import { Observable } from 'rxjs';
import { OrderModel } from './_models/OrderModel';

@Injectable({
  providedIn: 'root'
})
export class RequisitionService {

  private baseUrllRequisition: string = "/api/order/add";
  private baseUrlPatientDetails: string = "/api/order/";
  private baseUrllPatientEdit: string = "/api/order/update/";
  private baseUrllRequisitionSearch: string = "/api/order/search?orderNumber=";
  constructor(private http: HttpClient, private router: Router) {
  }

  public addRequisition(patient: OrderModel) {

try {
      return this.http.post<any>(this.baseUrllRequisition,
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

  public getRequisitionDetails(id:string){
    try{

      debugger;
      var  uri = this.baseUrlPatientDetails + id;

        let headers = new HttpHeaders();
        headers.set('Content-Type', 'application/json');

        return this.http.get(uri, { headers });

    } catch(e){
      console.log("Error:problem getting customer details "+ e.Message);
      return e.Message;
    }
  }

  public search(requisitionNumber: number){

    try{
      debugger;
      var  uri = this.baseUrllRequisitionSearch + requisitionNumber;

        let headers = new HttpHeaders();
        headers.set('Content-Type', 'application/json');
        return this.http.get(uri, { headers }).pipe(debounceTime(500));

    } catch(e){
      console.log("Error:problem searching for requisition "+ e.Message);
      return e.Message;
    }

  }

}
