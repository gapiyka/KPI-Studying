import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { AppComponent } from './app.component';
import { ValueDirective } from './value.directive';
import { InputDirective } from './input.directive';
@NgModule({
  imports: [BrowserModule],
  declarations: [AppComponent, InputDirective, ValueDirective],
  bootstrap: [AppComponent]
})
export class AppModule { }