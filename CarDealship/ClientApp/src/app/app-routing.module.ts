import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { LoginComponent } from './login/login.component';
import { ForgotpasswordComponent } from './forgotpassword/forgotpassword.component';
import { RegisterComponent } from './register/register.component';
import { IndexComponent } from './index/index.component';
import { HomeComponent } from './home/home.component';
import { AboutComponent } from './about/about.component';
import { SitelayoutComponent } from './sitelayout/sitelayout.component';
import { ApplayoutComponent } from './applayout/applayout.component';import { ProfileComponent } from './profile/profile.component';
import { ProfileeditComponent } from './profileedit/profileedit.component';
import { SupportComponent } from './support/support.component';
import { RegistrationconfirmationComponent } from './registrationconfirmation/registrationconfirmation.component';
import { ResetpasswordComponent } from './resetpassword/resetpassword.component';
import { ResetpasswordrequestComponent } from './resetpasswordrequest/resetpasswordrequest.component';
import { ResetpasswordsuccessComponent } from './resetpasswordsuccess/resetpasswordsuccess.component';
import { BookingComponent } from './booking/booking.component';
import { PatientComponent } from './patient/patient.component';
import { PatientdetailsComponent } from './patientdetails/patientdetails.component';
import { PatienteditComponent } from './patientedit/patientedit.component';
import { PatientsComponent } from './patients/patients.component';
import { RequisitionComponent } from './requisition/requisition.component';
import { RequisitiondetailsComponent } from './requisitiondetails/requisitiondetails.component';
import { RequisitioneditComponent } from './requisitionedit/requisitionedit.component';
import { RequisitionsComponent } from './requisitions/requisitions.component';
import { TestComponent } from './test/test.component';
import { TestdetailsComponent } from './testdetails/testdetails.component';
import { TesteditComponent } from './testedit/testedit.component';
import { TestsComponent } from './tests/tests.component';

const routes: Routes = [

  //Site routes goes here
  {
    path: '',
    component: SitelayoutComponent,
    children: [
      { path: 'about', component: AboutComponent },
      { path: 'login', component: LoginComponent },
      { path: 'forgotpassword', component: ForgotpasswordComponent },
      { path: 'reset/:data', component: ResetpasswordComponent },
      { path: 'register', component: RegisterComponent },
      { path: 'booking', component: BookingComponent },
      { path: '', component: IndexComponent, pathMatch: 'full' },
    ]
  },

  { path: 'confirmregistration', component: RegistrationconfirmationComponent },
  { path: 'passwordresetrequest', component: ResetpasswordrequestComponent },
  { path: 'passwordresetsuccess', component: ResetpasswordsuccessComponent },
  { path: 'support', component: SupportComponent },
  // App routes goes here here
  {
    path: '',
    component: ApplayoutComponent,
    children: [
      // -------------------Patient-------------------------------------------------------
      { path: 'client', component: PatientComponent },
      { path: 'clientdetails/:id', component: PatientdetailsComponent },
      { path: 'editclient/:id', component: PatienteditComponent },
      { path: 'clients', component: PatientsComponent },

      // -------------------Test-------------------------------------------------------
      { path: 'product', component: TestComponent },
      { path: 'productdetails/:id', component: TestdetailsComponent },
      { path: 'editproduct/:id', component: TesteditComponent },
      { path: 'products', component: TestsComponent },

       // -------------------Requisition-------------------------------------------------------
       { path: 'order', component: RequisitionComponent },
       { path: 'orderdetails/:id', component: RequisitiondetailsComponent },
       { path: 'editorder/:id', component: RequisitioneditComponent },
       { path: 'orders', component: RequisitionsComponent },
        // -------------------Profile-------------------------------------------------------
        { path: 'profile', component: ProfileComponent },
        { path: 'editprofile', component: ProfileeditComponent },
      { path: 'home', component: HomeComponent },
    ]
  },

  //no layout routes


  // otherwise redirect to home
  { path: '**', redirectTo: '' }

];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
