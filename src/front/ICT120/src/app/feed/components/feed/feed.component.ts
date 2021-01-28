import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { Publication } from 'src/app/publication/models/publication';
import { HostListener } from '@angular/core';
import { Router } from '@angular/router';
import { PublicationService } from 'src/app/publication/services/publication.service';

@Component({
  selector: 'app-feed',
  templateUrl: './feed.component.html',
  styleUrls: ['./feed.component.css']
})
export class FeedComponent implements OnInit {
  @Input() publications: Publication[];
  @Output() loadMore = new EventEmitter<void>();

  constructor(private routerService: Router, private publicationService: PublicationService) { }

  ngOnInit(): void {
  }

  public viewPublication(publication: Publication): void {
    this.routerService.navigate(["/publication/id/", { id: publication.id }]);
  }

  public repost(publication: Publication): void {
    this.publicationService.Repost(publication.id, !publication.reposted);
  }

  public like(publication: Publication): void {
    this.publicationService.Like(publication.id, !publication.liked);
  }

  /*@HostListener("window:scroll", ['$event'])*/
  public onWindowScroll(event: any) {
    // do some stuff here when the window is scrolled
    const verticalOffset = window.pageYOffset
      || document.documentElement.scrollTop
      || document.body.scrollTop || 0;

    console.log(verticalOffset);
    console.log(event);
    this.loadMore.emit()
  }
}
