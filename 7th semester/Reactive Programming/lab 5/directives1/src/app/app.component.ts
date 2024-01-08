import { Component } from '@angular/core';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  title = 'directives1';
  isSegoe = true;
  isVerdana = true;
  isNavy = true;
  currentClasses = {
    verdanaFont: this.isVerdana,
    navyColor: this.isNavy
  }
  visibility: boolean = true;
  toggle() {
    this.visibility = !this.visibility;
  }
}