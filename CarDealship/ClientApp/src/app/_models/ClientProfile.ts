import { Injectable } from "@angular/core";
import { OrderModel } from './OrderModel';
import { ClientModel } from './ClientModel';

@Injectable({
    providedIn:'root'
})

export class ClientProfileModel {

    public  id : string;
    public  clientId :number;
    public  Orders :Array<OrderModel> = [];
    public client:ClientModel;
}
