import { Component, Input, OnInit, Output, EventEmitter } from '@angular/core';
import { Router } from '@angular/router';
import { Publication } from '../../models/publication';
import { ClipboardModule } from '@angular/cdk/clipboard';
import { AuthService } from 'src/app/auth/services/auth.service';
import { Location } from '@angular/common';

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
  
  constructor(private authService: AuthService, public Router: Router, private locationService: Location) { }

  ngOnInit(): void {
    
  }

  public getPath(): string {
    return this.locationService.path(false); //DEBUG
  }

  public isOwner(): boolean {
    return this.authService.LocalUser.id == this.Publication.user.id;
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
