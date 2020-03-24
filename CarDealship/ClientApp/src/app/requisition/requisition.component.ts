import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { FormControl, FormGroup, FormBuilder, Validators } from '@angular/forms';
import { AddressModel } from '../_models/AddressModel';
import { ActivatedRoute, Router } from '@angular/router';
import { RegisterService } from '../register.service';
import { PatientService } from '../patient.service';
import { ActivatUIService } from '../activat-ui.service';
import { NgxSpinnerService } from 'ngx-spinner';
import { RequisitionService } from '../requisition.service';
import { ClientModel } from '../_models/ClientModel';
import { ProductService } from '../product.service';
import { OrderModel } from '../_models/OrderModel';

@Component({
  selector: 'app-requisition',
  templateUrl: './requisition.component.html',
  styleUrls: ['./requisition.component.css']
})
export class RequisitionComponent implements OnInit {

  LoginStatus$: Observable<boolean>;
  IsLoggedIn = false;
  Username$: Observable<string>;
  UserRole: FormControl;
  Email: FormControl;
  searchTerm : FormControl = new FormControl();
  searchTermProduct : FormControl = new FormControl();
  customerList  = <any>[];
  productList=<any>[]
  orderItems=<any>[];
  clientList  = <any>[];
  FullName:FormControl;
  Address:FormControl;
  ReferringPhysician:FormControl;
  DateSubmitted:FormControl;
  Province:FormControl;
  insertForm: FormGroup;
  errorList: string[];
  successMessage;
  invalidUser = false;
  isSuccessful = false;
  selectedOption:FormControl;
  selectedCustomer = '';
  selectedProduct='';
  User:OrderModel;
  id: number;
  patientProfileId: string;
  productId: string;
  address:AddressModel;
  private sub: any;
  IsRightUser:boolean;
  constructor(private activeRoute: ActivatedRoute,private register: RegisterService,
    private patientService: PatientService,
    private productService:ProductService,
    private requisitionService: RequisitionService,
    private activateUIService:ActivatUIService,private SpinnerService: NgxSpinnerService,
    private router: Router, private fb: FormBuilder) {

      this.productService.getProducts().subscribe(
        data => {
          this.productList = data as any[];
          console.log(this.productList);
        },
        error => {
          console.log("Error:Problem getting the products" + error);
      });
  }

  ngOnInit() {
    this.LoginStatus$ = this.register.isLoggedIn;
    this.Username$ = this.register.currentUsername;

    this.LoginStatus$.subscribe((data: boolean) =>
    {
        console.log('am i logged in ', data);
        this.IsLoggedIn = data;
    });
    if(!this.IsLoggedIn){
      this.router.navigate(['/login']);
    }
    this.activateUIService.initToggle();
    this.User = new OrderModel();
    this.address=new AddressModel();
    this.errorList = [];
    this.ReferringPhysician = new FormControl('', [Validators.required, Validators.maxLength(55), Validators.minLength(3)]);
    this.searchTerm.valueChanges.subscribe(
      term => {
        if (term !== '') {

          this.patientService.search(term).subscribe(
            data => {
              debugger;
              this.customerList = data as any[];
          });
        }
    });
    this.searchTermProduct.valueChanges.subscribe(
      term => {
        if (term !== '') {

          this.productService.search(term).subscribe(
            data => {
              debugger;
              this.productList = data as any[];
          });
        }
    });
    this.insertForm = this.fb.group({
      "product": this.searchTermProduct.value,
      "patient":this.searchTerm.value
    });

  }
  getPatientDetails(profileId:string){
    this.patientProfileId=profileId;
  }
  getProductDetails(productId:string){

    var obj={productId:productId};
    if(!this.orderItems.includes(obj))
    {
      this.orderItems.push(obj);
    }
  }

  onSave() {

    try {
      debugger;
      this.SpinnerService.show();
      this.errorList=[];
      this.invalidUser=false;
      let details = this.insertForm.value;
      if(this.patientProfileId===null || this.patientProfileId===undefined || this.patientProfileId==="" || this.patientProfileId===" ")
      {
        this.SpinnerService.hide();
        this.invalidUser=true;
        this.errorList.push("Please select a client");
        return;
      }
      if(this.orderItems===null || this.orderItems===undefined || this.orderItems.length==0)
      {
        this.SpinnerService.hide();
        this.invalidUser=true;
        this.errorList.push("Please select product(s)");
        return;
      }
      this.User.profileId=this.patientProfileId;
      this.User.orderItems=this.orderItems;
      this.requisitionService.addRequisition(this.User).subscribe(
        result => {
          debugger;
          this.SpinnerService.hide();
          if(result.id!==null && result.id!==undefined){
            this.invalidUser = false;
            this.isSuccessful = true;
            this.router.navigate(["/orders"]);
          }
          else{
        this.invalidUser=true;
        this.errorList.push("Please provide the refering physician");
        return;
          }

        },
        error=>{
          this.SpinnerService.hide();
          this.errorList.push(error.error.value.message);
          this.invalidUser = true;
            this.isSuccessful = false;
        }
      );

    } catch (e) {
      this.SpinnerService.hide();
      this.errorList.push(e.Message);
      this.invalidUser = true;
      this.isSuccessful = false;

    }
    return;
  }

}
