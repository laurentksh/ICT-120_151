import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { Publication } from 'src/app/publication/models/publication';
import { HostListener } from '@angular/core';
import { Router } from '@angular/router';
import { PublicationService } from 'src/app/publication/services/publication.service';
import { OperationResult } from 'src/app/services/models/operation-result';
import { GlobalAppEventsService } from 'src/app/services/global-app-events/global-app-events.service';

@Component({
  selector: 'app-feed',
  templateUrl: './feed.component.html',
  styleUrls: ['./feed.component.css']
})
export class FeedComponent implements OnInit {
  @Input() publications: Publication[];
  @Output() RefreshPublication = new EventEmitter<Publication>();
  @Output() LoadMore = new EventEmitter<void>();

  constructor(private routerService: Router, private publicationService: PublicationService, private appEvents: GlobalAppEventsService) { }

  ngOnInit(): void {
  }

  public refreshPublication(publication: Publication): void {
    this.RefreshPublication.emit(publication);
  }

  public repost(publication: Publication): void {
    console.log(publication.reposted);
    this.publicationService.Repost(publication.id, !publication.reposted).then(x => {
      if (x.Success) {
        this.appEvents.ShowSnackBarMessage(`You ${publication.reposted ? "unreposted" : "reposted"} ${publication.user.username}'s publication.`);

        this.refreshPublication(publication);
      } else {
        this.appEvents.ShowSnackBarMessage(`An error occured while trying to ${publication.reposted ? "unrepost" : "repost"} publication '${publication.id}' (${x.Error.status})`);
      }
    });
  }

  public like(publication: Publication): void {
    this.publicationService.Like(publication.id, !publication.liked).then(x => {
      if (x.Success) {
        this.appEvents.ShowSnackBarMessage(`You ${publication.liked ? "unliked" : "liked"} ${publication.user.username}'s publication.`);

        this.refreshPublication(publication);
      } else {
        this.appEvents.ShowSnackBarMessage(`An error occured while trying to ${publication.liked ? "unlike" : "like"} publication '${publication.id}' (${x.Error.status})`);
      }
    });
  }

  public delete(publication: Publication): void {
    this.publicationService.Delete(publication.id).then(x => {
      if (x.Success) {
        this.appEvents.ShowSnackBarMessage("You deleted your publication :(");

        this.refreshPublication(publication);
      } else {
        this.appEvents.ShowSnackBarMessage(`An error occured while trying to delete publication '${publication.id}' (${x.Error.status})`);
      }
    });
  }

  /*@HostListener("window:scroll", ['$event'])*/
  public onWindowScroll(event: any) {
    // do some stuff here when the window is scrolled
    const verticalOffset = window.pageYOffset
      || document.documentElement.scrollTop
      || document.body.scrollTop || 0;

    console.log(verticalOffset);
    console.log(event);
    this.LoadMore.emit()
  }
}
