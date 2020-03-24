import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { FormControl, FormGroup, FormBuilder, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { RegisterService } from '../register.service';
import { PatientService } from '../patient.service';
import { NormalrangevalueService } from '../normalrangevalue.service';
import { ActivatUIService } from '../activat-ui.service';
import { NgxSpinnerService } from 'ngx-spinner';
import { RequisitionService } from '../requisition.service';
import { ProductModel } from '../_models/ProductModel';
import { ProductService } from '../product.service';

@Component({
  selector: 'app-testedit',
  templateUrl: './testedit.component.html',
  styleUrls: ['./testedit.component.css']
})
export class TesteditComponent implements OnInit {

  LoginStatus$: Observable<boolean>;
  IsLoggedIn = false;
  Username$: Observable<string>;
  UserRole: FormControl;
  Name:FormControl;
  Cost:FormControl;
  SellingPrice:FormControl;
  TestResults:FormControl;
  NormalRangeValue:FormControl;
  testResults=<any>[];
  normalRangeValues=<any>[];
  selectedCustomer = '';
  searchTerm : FormControl = new FormControl();
  customerList  = <any>[];
  RequisitionId:string;
  insertForm: FormGroup;
  errorList: string[];
  successMessage;
  invalidUser = false;
  isSuccessful = false;
  selectedTestResultOption:FormControl;
  selectedNormalRangeOption:FormControl;
  User:ProductModel;
  id: string;
  private sub: any;
  constructor(private activeRoute: ActivatedRoute,private register: RegisterService,
    private patientService: PatientService,private testResultService:ProductService,
    private normalRangeValueService:NormalrangevalueService,
    private activateUIService:ActivatUIService,private SpinnerService: NgxSpinnerService,
    private requisionService:RequisitionService,
    private router: Router, private fb: FormBuilder) {

      this.testResultService.getTestResults().subscribe(
        data => {
          this.testResults = data as any[];
          console.log(this.testResults);
        },
        error => {
          console.log("Error:Problem getting the product types" + error);
      });
  }
  ngOnInit()
  {
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
    this.User = new ProductModel();
    this.errorList = [];
    this.Name = new FormControl('', [Validators.required, Validators.maxLength(55), Validators.minLength(3)]);
    this.Cost=new FormControl();
    this.SellingPrice=new FormControl();
    this.TestResults=new FormControl();
    this.searchTerm.valueChanges.subscribe(
      term => {
        if (term !== '') {

          this.requisionService.search(term).subscribe(
            data => {
              debugger;
              this.customerList = data as any[];
          });
        }
    });
    this.insertForm = this.fb.group({
      "Name": this.Name,
      "Cost": this.Cost,
      "SellingPrice": this.SellingPrice,
      "SearchTerm":this.searchTerm,
      "TestResults":this.TestResults,
    });
    this.sub = this.activeRoute.params.subscribe(params => {
      this.id = params.id; // (+) converts string 'id' to a number

      // In a real app: dispatch action to load the details here.
   });
    this.testResultService.getTestDetails(this.id).subscribe(
      data => {
        debugger;
        this.User = data as any;
        console.log(this.User);
        this.Name.setValue(this.User.name);
        this.TestResults.setValue(this.User.productTypeId);
        this.Cost.setValue(this.User.cost);
        this.SellingPrice.setValue(this.User.sellingPrice);
      },
      error => {
        console.log('Error:Problem getting the product details ' + error);
    });
  }

  getRequisitionDetails(requisitionId:string){
    this.RequisitionId=requisitionId;
  }

  onSave() {

    try {
      debugger;
      this.SpinnerService.show();
      this.errorList=[];
      this.invalidUser=false;
      let details = this.insertForm.value;
      if(details.Name===null || details.Name===undefined || details.Name==="" || details.Name===" ")
      {
        this.SpinnerService.hide();
        this.invalidUser=true;
        this.errorList.push("Please provide the product name");
        return;
      }
      if(this.TestResults.value===null || this.TestResults.value===undefined || this.TestResults.value==="" || this.TestResults.value===" ")
      {
        this.SpinnerService.hide();
        this.invalidUser=true;
        this.errorList.push("Please select the product type");
        return;
      }
      if(details.Cost===null || details.Cost===undefined || details.Cost==="" || details.Cost===" ")
      {
        this.SpinnerService.hide();
        this.invalidUser=true;
        this.errorList.push("Please provide the product cost");
        return;
      }
      if(details.SellingPrice===null || details.SellingPrice===undefined || details.SellingPrice==="" || details.SellingPrice===" ")
      {
        this.SpinnerService.hide();
        this.invalidUser=true;
        this.errorList.push("Please provide the selling price");
        return;
      }
      this.User.name=details.Name;
      this.User.cost=details.Cost;
      this.User.sellingPrice=details.SellingPrice;
      this.User.productTypeId=parseInt(details.TestResults);
      this.testResultService.updateTestDetails(this.User).subscribe(
        result => {
          debugger;
          this.SpinnerService.hide();
          if(result.id!==null && result.id!==undefined){
            this.invalidUser = false;
            this.isSuccessful = true;
            this.router.navigate(["/products"]);
          }
          else
          {
            this.invalidUser=true;
            this.errorList.push("Please provide the product name,cost,selling price and select product type");
            return;
          }
        },
        error=>{
          for(var i=0;i<error.error.errors.fullName.length;i++){
            this.SpinnerService.hide();
            this.errorList.push(error.error.errors.FullName[i]);
            console.log(error.error.errors.FullName[i]);
            this.invalidUser = true;
            this.isSuccessful = false;
          }
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
