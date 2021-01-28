import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Params } from '@angular/router';
import { Publication } from '../../models/publication';
import { PublicationService } from '../../services/publication.service';

@Component({
  selector: 'app-view-publication',
  templateUrl: './view-publication.component.html',
  styleUrls: ['./view-publication.component.css']
})
export class ViewPublicationComponent implements OnInit {
  error: boolean = false;
  errorMsg: string = "";

  Publication: Publication;
  Replies: Publication[];

  constructor(private route: ActivatedRoute, private publicationService: PublicationService) { }

  ngOnInit(): void {
    this.route.params.subscribe(async params => await this.Load(params));
  }

  private async Load(params: Params): Promise<void> {
    const id = params["id"];

    if (id == null) {
      this.error = true;
      this.errorMsg = "Missing parameter 'id'.";
      return;
    }

    const result = await this.publicationService.GetPublication(id);
    
    if (result.Success) {
      this.Publication = result.Content;

      await this.LoadReplies();
    } else {
      this.error = false;
      this.errorMsg = `Could not load specified user. (${result.Error.status ?? ""})`;
    }
  }

  private async LoadReplies(): Promise<void> {
    if (this.Publication == null) {
      this.error = true;
      this.errorMsg = "Could not load specified user.";
      return;
    }

    const result = await this.publicationService.GetReplies(this.Publication.id);

    if (result.Success) {
      this.Replies = result.Content;
    } else {
      this.error = false;
      this.errorMsg = "Could not load replies.";
    }
  }

  /*@HostListener("window:scroll", ['$event'])*/
  public onWindowScroll(event: any) {
    // do some stuff here when the window is scrolled
    const verticalOffset = window.pageYOffset
      || document.documentElement.scrollTop
      || document.body.scrollTop || 0;

    console.log(verticalOffset);
    console.log(event);
  }
}
