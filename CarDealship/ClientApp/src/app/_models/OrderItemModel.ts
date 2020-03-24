import { Injectable } from "@angular/core";
import { ProductModel } from './ProductModel';
import { OrderModel } from './OrderModel';



@Injectable({
    providedIn:'root'
})

export class OrderItemModel{
  public  id:string;
  public  orderId:string;
  public  productId:string;
  public  product:ProductModel;
  public  order:OrderModel;
}
