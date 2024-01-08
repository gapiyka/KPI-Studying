import { Component, OnInit } from '@angular/core';
import { DataService } from './data.service';
import { LogService } from './log.service';
import { Phone } from './phone';

@Component({
  selector: 'data-comp',
  templateUrl: './data.component.html',
  styleUrls: ['./app.component.css'],
  //providers: [DataService, LogService], // uncomment to separate services
})
export class DataComponent implements OnInit {
  name: string = '';
  price!: number;
  items: Phone[] = [];

  constructor(private dataService: DataService) { }

  addItem(name: string, price: number) {
    this.dataService.addData(name, price);
  }

  ngOnInit() {
    this.items = this.dataService.getData();
  }
}
