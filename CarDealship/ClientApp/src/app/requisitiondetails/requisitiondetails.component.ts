import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { FormControl, FormGroup, FormBuilder } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { RegisterService } from '../register.service';
import { PatientService } from '../patient.service';
import { ActivatUIService } from '../activat-ui.service';
import { RequisitionService } from '../requisition.service';
import { OrderModel } from '../_models/OrderModel';

@Component({
  selector: 'app-requisitiondetails',
  templateUrl: './requisitiondetails.component.html',
  styleUrls: ['./requisitiondetails.component.css']
})
export class RequisitiondetailsComponent implements OnInit {

  LoginStatus$: Observable<boolean>;
  IsLoggedIn = false;
  Username$: Observable<string>;
  customerTypes =  [] as any;
  User: OrderModel;
  Contact: FormControl;
  patientId:string;
  insertForm: FormGroup;
  patient:string;
  sellingPrice:string;
  cost:string;
  errorList: string[];
  orderItems=<any>[];
  successMessage;
  invalidUser = false;
  isSuccessful = false;
  selectedOption: FormControl;
  IsRightUser:boolean;
  id: string;
  private sub: any;
  constructor(private activeRoute: ActivatedRoute, private register: RegisterService,
     private patientService: PatientService, private activateUIService:ActivatUIService,
     private requisitionService: RequisitionService,
     private router: Router, private fb: FormBuilder) {

    this.User = new OrderModel();
  }

  ngOnInit() {
    this.LoginStatus$ = this.register.isLoggedIn;
    this.Username$ = this.register.currentUsername;

    this.LoginStatus$.subscribe((data: boolean) => {
        console.log('am i logged in ', data);
        this.IsLoggedIn = data;
    });

    if (!this.IsLoggedIn) {
      this.router.navigate(['/login']);
    }
    this.activateUIService.initToggle();
    this.sub = this.activeRoute.params.subscribe(params => {
      this.id = params.id; // (+) converts string 'id' to a number

      // In a real app: dispatch action to load the details here.
   });
   debugger;
    this.requisitionService.getRequisitionDetails(this.id).subscribe(
      data => {
        debugger;
        this.User = data as any;
        this.patient=data.profile.client.fullName;
        this.patientId=data.profile.client.id;
        this.sellingPrice=data.totalSellingPrice;
        this.cost=data.totalCost;
        this.orderItems=data.orderItems;
        console.log(this.User);
      },
      error => {
        this.register.logout();
        console.log('Error:Problem getting the order details ' + error);
    });
  }
}
