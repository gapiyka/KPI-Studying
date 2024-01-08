import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule } from '@angular/forms';
import { AppComponent } from './app.component';
import { DataService } from './data.service';
import { LogService } from './log.service';
import { DataModule } from './data.module';

@NgModule({
  declarations: [AppComponent],
  imports: [BrowserModule, FormsModule, DataModule],
  providers: [DataService, LogService], // comment to separate services
  bootstrap: [AppComponent],
})
export class AppModule { }
