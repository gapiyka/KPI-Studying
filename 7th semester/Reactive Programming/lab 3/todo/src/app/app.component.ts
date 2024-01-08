import { Component } from '@angular/core';
import { TodoList } from "./todoList";
import { TodoItem } from "./todoItem";
@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  private list = new TodoList("Denys", [
    new TodoItem("Morning jogging", true),
    new TodoItem("Visit local bakery"),
    new TodoItem("Make cleaning at home"),
  ]);
  get username(): string {
    return this.list.user;
  }
  get itemCount(): number {
    return this.list.items
      .filter(item => !item.complete).length;
  }
  get items(): readonly TodoItem[] {
    return this.list.items.filter(item => this.showComplete || !item.complete);
  }
  addItem(newItem: string) {
    if (newItem != "") {
      this.list.addItem(newItem);
    }
  }
  showComplete: boolean = false;
} 