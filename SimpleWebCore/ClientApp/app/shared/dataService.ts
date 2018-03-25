import { HttpClient, HttpHeaders, HttpResponse } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from 'rxjs';
import 'rxjs/add/operator/map';
import { Product } from "../shared/product";
import { Order, OrderItem } from "../shared/order";
import { TokenResponse } from "../shared/tokenResponse";

@Injectable()
export class DataService {

    constructor(private http: HttpClient) {
    }

    public token: string = "";
    public tokenExpiration: Date;

    public order: Order = new Order();

    public products: Product[] = [];

    public get loginRequired(): boolean {
        return this.token.length == 0 || this.tokenExpiration > new Date();
    }

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

    public login(credentials) {
        return this.http.post("/account/createtoken", credentials)
            .map(response => {
                let tokenResponse: any = response;
                this.token = tokenResponse.token;
                this.tokenExpiration = tokenResponse.expiration;
                return true;
            });
    }

    public checkout() {
        if (!this.order.orderNumber) {
            this.order.orderNumber = this.order.orderDate.getFullYear().toString() + this.order.orderDate.getTime().toString();
        }
        return this.http.post("/api/order", this.order, {
            headers: new HttpHeaders({ "Authorization": "Bearer " + this.token})
        })
            .map(response => {
                this.order = new Order();
                return true;
            });
    }

    //public loadProducts(): Observable<Product[]> {
    //    return this.http.get("/api/products")
    //        .map((result: Response) => {
    //            result.json().then(res => { this.products = res});
    //            return this.products;
    //        });
    //}
}