import { Component } from "@angular/core";

@Component({
    selector: "product-list",
    templateUrl: "productList.component.html",
    styleUrls: []
})

export class ProductList {
    public products = [{
        title: "First product",
        price: 18.90
    },
    {
        title: "Second product",
        price: 12.45
    },
    {
        title: "Third product",
        price: 2.5
    }];
}