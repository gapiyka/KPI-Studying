import { Component, OnInit } from '@angular/core';
import { HttpService } from './http.service';
import { Observable } from 'rxjs';
@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css'],
  providers: [HttpService]
})
export class AppComponent implements OnInit {
  title = 'pipes3';
  users: Observable<Object> | any;
  constructor(private httpService: HttpService) { }
  ngOnInit() {
    this.users = this.httpService.getUsers();
  }
}
