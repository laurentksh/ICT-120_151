import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Params } from '@angular/router';
import { GlobalAppEventsService, MessageType } from 'src/app/services/global-app-events/global-app-events.service';
import { Publication } from '../../models/publication';
import { PublicationService } from '../../services/publication.service';

@Component({
  selector: 'app-view-publication',
  templateUrl: './view-publication.component.html',
  styleUrls: ['./view-publication.component.css']
})
export class ViewPublicationComponent implements OnInit {
  Publication: Publication = {} as Publication;
  Replies: Publication[] = new Array<Publication>();

  constructor(private route: ActivatedRoute, private publicationService: PublicationService, private appEvents: GlobalAppEventsService) { }

  ngOnInit(): void {
    this.route.params.subscribe(async params => await this.Load(params));
  }

  private async Load(params: Params): Promise<void> {
    this.appEvents.Loading();
    this.appEvents.HideMessage();
    const id = params["id"];
    
    if (id == null) {
      this.appEvents.ShowMessage("Missing parameter 'id'.", MessageType.Error);
      return;
    }

    const result = await this.publicationService.GetPublication(id);
    
    if (result.Success) {
      this.Publication = result.Content;

      await this.LoadReplies();
    } else {
      this.appEvents.ShowMessage(`Could not load specified user. (${result.Error.status ?? ""})`, MessageType.Error);
    }
  }

  private async LoadReplies(): Promise<void> {
    if (this.Publication == null) {
      this.appEvents.ShowMessage("Could not load specified user.", MessageType.Error);
      return;
    }

    const result = await this.publicationService.GetReplies(this.Publication.id);

    this.appEvents.DoneLoading();
    if (result.Success) {
      this.Replies = result.Content;
    } else {
      this.appEvents.ShowMessage("Could not load replies.", MessageType.Error);
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
