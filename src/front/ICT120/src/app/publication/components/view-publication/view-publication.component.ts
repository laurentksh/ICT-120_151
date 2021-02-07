import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Params, Router } from '@angular/router';
import { GlobalAppEventsService, MessageType } from 'src/app/services/global-app-events/global-app-events.service';
import { Publication } from '../../models/publication';
import { PublicationService } from '../../services/publication.service';

@Component({
  selector: 'app-view-publication',
  templateUrl: './view-publication.component.html',
  styleUrls: ['./view-publication.component.css']
})
export class ViewPublicationComponent implements OnInit {
  Publication: Publication = null;
  Replies: Publication[] = null;

  constructor(private route: ActivatedRoute, private router: Router, private publicationService: PublicationService, private appEvents: GlobalAppEventsService) { }

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

  public updatePublication(): void {
    this.UpdatePublication().then();
  }

  private async UpdatePublication(): Promise<void> {
    console.log("updated");
    const updatedPublication = await this.publicationService.GetPublication(this.Publication.id);

    if (updatedPublication.Success) {
      this.Publication = updatedPublication.Content;
    } else {
      if (updatedPublication.Error.status == 404) {
        this.appEvents.ShowSnackBarMessage("This publication has been deleted.");
        this.router.navigate(["/login"], { queryParams: { "redirect": this.router.url } });
        return;
      }

      this.appEvents.ShowSnackBarMessage(`Could not update publication '${this.Publication.id}.'`);
    }
  }

  public repost(): void {
    console.log(this.Publication.reposted);
    this.publicationService.Repost(this.Publication.id, !this.Publication.reposted).then(x => {
      if (x.Success) {
        this.appEvents.ShowSnackBarMessage(`You ${this.Publication.reposted ? "unreposted" : "reposted"} ${this.Publication.user.username}'s publication.`);

        this.updatePublication();
      } else {
        this.appEvents.ShowSnackBarMessage(`An error occured while trying to ${this.Publication.reposted ? "unrepost" : "repost"} publication '${this.Publication.id}' (${x.Error.status})`);
      }
    });
  }

  public like(): void {
    this.publicationService.Like(this.Publication.id, !this.Publication.liked).then(x => {
      if (x.Success) {
        this.appEvents.ShowSnackBarMessage(`You ${this.Publication.liked ? "unliked" : "liked"} ${this.Publication.user.username}'s publication.`);

        this.updatePublication();
      } else {
        this.appEvents.ShowSnackBarMessage(`An error occured while trying to ${this.Publication.liked ? "unlike" : "like"} publication '${this.Publication.id}' (${x.Error.status})`);
      }
    });
  }

  public delete(): void {
    this.publicationService.Delete(this.Publication.id).then(x => {
      if (x.Success) {
        this.appEvents.ShowSnackBarMessage("You deleted your publication :(");

        this.updatePublication();
      } else {
        this.appEvents.ShowSnackBarMessage(`An error occured while trying to delete publication '${this.Publication.id}' (${x.Error.status})`);
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
  }
}
