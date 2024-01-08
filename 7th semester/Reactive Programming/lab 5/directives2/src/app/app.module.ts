import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { AppComponent } from './app.component';
import { BoldDirective } from './bold.directive';
import { ItalicDirective } from './italic.directive';
import { MouseBoldDirective } from './mousebold.directive';
import { MouseItalicDirective } from './mouseitalic.directive';
import { HostMouseBoldDirective } from './hostmousebold.directive';
@NgModule({
  imports: [BrowserModule],
  declarations: [AppComponent, BoldDirective, ItalicDirective, MouseBoldDirective, MouseItalicDirective, HostMouseBoldDirective],
  bootstrap: [AppComponent]
})
export class AppModule { }