import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Router } from '@angular/router';
import { map, debounceTime } from 'rxjs/operators';
import { ProductModel } from './_models/ProductModel';
@Injectable({
  providedIn: 'root'
})
export class ProductService {

  private baseUrllGetTestResults: string = "/api/producttype/getall";
  private baseUrllGetProducts: string = "/api/product/getall";
  private baseUrllAddTest: string = "/api/product/add";
  private baseUrllGetTest: string = "/api/product/";
  private baseUrllTestEdit:string="/api/product/update/";
  private baseUrllProductSearch: string = "/api/product/search/?name=";
  constructor(private http: HttpClient, private router: Router) {
  }
  public search(name: string){

    try{

      var  uri = this.baseUrllProductSearch + name;

        let headers = new HttpHeaders();
        headers.set('Content-Type', 'application/json');
        return this.http.get(uri, { headers }).pipe(debounceTime(500));

    } catch(e){
      console.log("Error:problem searching for client "+ e.Message);
      return e.Message;
    }

  }
  public getTestResults(){

    try {
      let headers = new HttpHeaders();
      headers.set('Content-Type', 'application/json');
      return this.http.get(this.baseUrllGetTestResults , {headers});
    } catch (e) {
      console.log("Error:problem getting the test results "+ e.Message);
      return e.Message;
    }

  }
  public getProducts(){

    try {
      let headers = new HttpHeaders();
      headers.set('Content-Type', 'application/json');
      return this.http.get(this.baseUrllGetProducts , {headers});
    } catch (e) {
      console.log("Error:problem getting the products "+ e.Message);
      return e.Message;
    }

  }
  public addTest(test: ProductModel) {
    debugger;
    try {
      return this.http.post<any>(this.baseUrllAddTest, test).pipe(
        map(result => {

          return result;

        }, error => {
          return error;
        }));

    } catch (e) {

    }

  }
  public getTestDetails(id:string){
    try{

      var  uri = this.baseUrllGetTest + id;

        let headers = new HttpHeaders();
        headers.set('Content-Type', 'application/json');

        return this.http.get(uri, { headers });

    } catch(e){
      console.log("Error:problem getting test details "+ e.Message);
      return e.Message;
    }
  }
  public updateTestDetails(test:ProductModel) {

    try {
      var  uri = this.baseUrllTestEdit + test.id;
      return this.http.put<any>(uri, test).pipe(
        map(result => {

          return result;

        }, error => {
          return error;
        }));

    } catch (e) {

    }


  }
}
