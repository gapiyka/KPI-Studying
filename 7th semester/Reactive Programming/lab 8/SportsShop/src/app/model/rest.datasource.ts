import { Injectable } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { Observable, map } from "rxjs";
import { Product } from "./product.model";
import { Order } from "./order.model";
const PROTOCOL = "http";
const PORT = 3500;
interface JsonObject {
  products: Product[],
  orders: Order[]
}
@Injectable()
export class RestDataSource {
  baseUrl: string;
  saveProducts!: JsonObject;
  constructor(private http: HttpClient) {
    this.baseUrl = `https://firebasestorage.googleapis.com/v0/b/hapiiip05laba4.appspot.com/o/data.json?alt=media&token=363ccac9-88b4-4520-8f95-07f296d49f0b&_gl=1*yxzb9j*_ga*NzExMjUwNjAwLjE2OTQ1ODkwOTI.*_ga_CW55HF8NVT*MTY5ODYxNzE4MC4yNS4xLjE2OTg2MTc2MDUuNDQuMC4w/`;
  }
  getProducts(): Observable<Product[]> {
    return this.http.get<JsonObject>(this.baseUrl).pipe(
      map(item => { this.saveProducts = item; return item.products }));
  }
  saveOrder(order: Order): Observable<Order> {
    this.saveProducts.orders.push(order);
    let res = this.http.post<JsonObject>(this.baseUrl, this.saveProducts)
    return res.pipe(
      map(item =>
        this.saveProducts.orders[this.saveProducts.orders.length - 1]));
  }
}