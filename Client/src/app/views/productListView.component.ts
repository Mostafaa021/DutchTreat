import { Component } from "@angular/core";
import { Store } from "../Services/store.service";

@Component({
    selector: "prodcut-list",
    templateUrl: "productListView.component.html"

})
export default class ProductListView {
    public products: { title: string; price: string; }[];
  
    constructor(private store: Store) {
        this.products = store.products;
    }
    
}