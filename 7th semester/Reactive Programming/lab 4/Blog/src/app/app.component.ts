import { Component } from '@angular/core';
export interface Post {
  title: string;
  text: string;
  id?: number;
}
@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  title = 'BlogComponents';
  posts: Post[] = [{ title: 'Вивчаю компоненти', text: 'Створюю проект "Блог"', id: 1 }, { title: 'Вивчаю директиви', text: 'Все ще створюю "Блог"', id: 2 }]
  counter = 3;
  updatePosts(post: Post) {
    post.id = this.counter++; // custom add
    this.posts.unshift(post);
  }
  deletePost(id: number) {
    this.posts = this.posts.filter(p => p.id !== id)
  }
}
