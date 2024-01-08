import { Component, Input, OnDestroy, EventEmitter, Output } from '@angular/core';
import { Post } from '../app.component';
@Component({
  selector: 'app-post',
  templateUrl: './post.component.html',
  styleUrls: ['./post.component.css']
})
export class PostComponent implements OnDestroy {
  @Input('toPost') myPost!: Post;
  @Output() onRemove = new EventEmitter<number>()
  constructor() { }

  ngOnDestroy() {
    console.log('метод ngOnDestroy');
  }
  removePost() {
    this.onRemove.emit(this.myPost.id)
  }
}
