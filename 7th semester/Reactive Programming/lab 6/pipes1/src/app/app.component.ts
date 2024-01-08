import { Component } from '@angular/core';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  title = 'pipes1';
  myDate = new Date(1945, 3, 3);
  welcome: string = "Hello Angular!";
  percentage: number = 0.69;
  myNewDate = Date.now();
  pi: number = 3.1415;
  money: number = 23.45;
  message = "Hello World";
  x: number = 15.45;
  users = ["Tom", "Alice", "Sam", "Kate", "Bob"];
  xSqrt: number = 16;
}