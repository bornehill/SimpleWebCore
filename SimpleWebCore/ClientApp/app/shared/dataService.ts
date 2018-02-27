import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from 'rxjs';
import 'rxjs/add/operator/map';
import { Product } from "../shared/product";
import { Order, OrderItem } from "../shared/order";

@Injectable()
export class DataService {

    constructor(private http: HttpClient) {
    }

    public order: Order = new Order();

    public products: Product[] = [];

    public loadProducts(): Observable<Product[]> {
        return this.http.get<Product[]>("/api/products");
    }

    public AddToOrder(product: Product) {

        let item: OrderItem = this.order.items.find(i => i.productId == product.id);
        if (item) {
            item.quantity++;
        } else {

            item = new OrderItem();
            item.productId = product.id;
            item.productArtist = product.artist;
            item.productCategoty = product.categoty;
            item.productSize = product.size;
            item.productArtId = product.artId;
            item.productTitle = product.title;
            item.unitPrice = product.price;
            item.quantity = 1;
            this.order.items.push(item);
        }

    }

    //public loadProducts(): Observable<Product[]> {
    //    return this.http.get("/api/products")
    //        .map((result: Response) => {
    //            result.json().then(res => { this.products = res});
    //            return this.products;
    //        });
    //}
}