import { Injectable } from "@angular/core";
import { ProductTypeModel } from './ProductTypeModel';

@Injectable({
    providedIn:'root'
})

export class ProductModel {

    public id : string;
    public name:string;
    public cost :number;
    public sellingPrice :number;
    public productTypeId :number;
    public imagePath:string;
    public productType:ProductTypeModel;
}
