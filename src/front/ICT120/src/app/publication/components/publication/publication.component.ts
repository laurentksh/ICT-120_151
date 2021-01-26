import { Component, Input, OnInit, Output, EventEmitter } from '@angular/core';
import { Publication } from '../../models/publication';

@Component({
  selector: 'app-publication',
  templateUrl: './publication.component.html',
  styleUrls: ['./publication.component.css']
})
export class PublicationComponent implements OnInit {
  @Input() Publication: Publication;
  @Output() ViewPublication = new EventEmitter<Publication>()
  @Output() Repost = new EventEmitter<Publication>()
  @Output() Like = new EventEmitter<Publication>()
  
  constructor() { }

  ngOnInit(): void {
  }

  public doViewPublication(): void {
    this.Like.emit(this.Publication);
  }

  public doRepost(): void {
    this.Repost.emit(this.Publication);
  }

  public doLike(): void {
    this.Like.emit(this.Publication);
  }
}
