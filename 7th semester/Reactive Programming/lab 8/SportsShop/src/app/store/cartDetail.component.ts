import { Component } from '@angular/core';
import { Cart } from "../model/cart.model";

@Component({
  // moduleId: module.id, // error TS2591: Cannot find name 'module'. Do you need to install type definitions for node? 
                          // Try `npm i --save-dev @types/node` and then add 'node' to the types field in your tsconfig.
  templateUrl: "cartDetail.component.html"
})
export class CartDetailComponent {
  constructor(public cart: Cart) {}
}
