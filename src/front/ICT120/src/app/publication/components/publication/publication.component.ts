import { Component, Input, OnInit, Output, EventEmitter } from '@angular/core';
import { Router } from '@angular/router';
import { Publication } from '../../models/publication';
import { ClipboardModule } from '@angular/cdk/clipboard';
import { AuthService } from 'src/app/auth/services/auth.service';
import { Location } from '@angular/common';
import { MediaService } from 'src/app/media/services/media.service';
import { MediaViewModel } from 'src/app/media/models/media-view-model';

@Component({
  selector: 'app-publication',
  templateUrl: './publication.component.html',
  styleUrls: ['./publication.component.css']
})
export class PublicationComponent implements OnInit {
  @Input() Publication: Publication;
  @Output() Repost = new EventEmitter<Publication>();
  @Output() Like = new EventEmitter<Publication>();
  @Output() Delete = new EventEmitter<Publication>();
  
  profilePicture: MediaViewModel = null;
  media: MediaViewModel = null;

  constructor(private authService: AuthService, private mediaService: MediaService, private Router: Router) { }

  ngOnInit(): void {
    this.LoadMedia().then();
  }

  public async LoadMedia(): Promise<void> {
    this.mediaService.GetMedia(this.Publication.user.profilePictureId).then(x => {
      if (x.Success) {
        this.profilePicture = x.Content;
      } else {
        this.profilePicture = {} as MediaViewModel;
        this.profilePicture.blobFullUrl = "/assets/media-load-error.png";
      }
    });

    if (this.Publication.mediaId != null) {
      this.mediaService.GetMedia(this.Publication.mediaId).then(x => {
        if (x.Success) {
          this.media = x.Content;
        } else {
          this.media = {} as MediaViewModel;
          this.media.blobFullUrl = "/assets/media-load-error.png";
        }
      });
    }
  }

  public getPath(): string {
    const url = globalThis.window.location; //Idk if this counts as raw JavaScript but it was the only way I found to get the current host...

    return `${url.protocol}//${url.host}/publication/id/${this.Publication.id}`;
  }

  public isOwner(): boolean {
    if (!this.authService.IsAuthenticated)
      return false;
    
    return this.authService.LocalUser.id == this.Publication.user?.id;
  }

  public getDate(): Date {
    return new Date(this.Publication.creationDate);
  }

  public doViewUser(): void {
    this.Router.navigate(["/u", this.Publication.user.username]);
  }
  
  public doViewPublication(): void {
    this.Router.navigate(["/publication/id", this.Publication.id]);
  }

  public doRepost(): void {
    if (!this.authService.IsAuthenticated) {
      this.Router.navigate(["/login"], { queryParams: { "redirect": this.Router.url } });
      return;
    }
    
    this.Repost.emit(this.Publication);
  }

  public doLike(): void {
    if (!this.authService.IsAuthenticated) {
      this.Router.navigate(["/login"], { queryParams: { "redirect": this.Router.url } });
      return;
    }
    
    this.Like.emit(this.Publication);
  }

  public doDelete(): void {
    if (!this.authService.IsAuthenticated) {
      this.Router.navigate(["/login"], { queryParams: { "redirect": this.Router.url } });
      return;
    }
    
    this.Delete.emit(this.Publication);
  }
}
