import { Injectable } from "@angular/core";
import { ClientProfileModel } from './ClientProfile';
import { OrderItemModel } from './OrderItemModel';

@Injectable({
    providedIn:'root'
})

export class OrderModel {

    public  id : string;
    public  orderNumber :number;
    public  dateCreated:Date;
    public  createdBy :string;
    public  profileId :string;
    public  Profile :ClientProfileModel;
    public orderItems:Array<OrderItemModel> = [];
}
