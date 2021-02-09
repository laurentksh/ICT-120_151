import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute, Params, Router } from '@angular/router';
import { GlobalAppEventsService, MessageType } from 'src/app/services/global-app-events/global-app-events.service';
import { Publication } from '../../models/publication';
import { PublicationService } from '../../services/publication.service';
import { ReplyComponent } from '../reply/reply.component';

@Component({
  selector: 'app-view-publication',
  templateUrl: './view-publication.component.html',
  styleUrls: ['./view-publication.component.css']
})
export class ViewPublicationComponent implements OnInit {
  Publication: Publication = null;
  Replies: Publication[] = null;
  @ViewChild(ReplyComponent) replyForm: ReplyComponent;

  constructor(private route: ActivatedRoute, private router: Router, private publicationService: PublicationService, private appEvents: GlobalAppEventsService) { }

  ngOnInit(): void {
    this.route.params.subscribe(async params => await this.Load(params));
  }

  public replySent(): void {
    this.refreshReplies();
    this.replyForm.resetForm();
  }

  public refreshReplies(): void {
    this.LoadReplies().then();
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

  public updatePublication(publication: Publication = null): void {
    this.UpdatePublication(publication).then();
  }

  private async UpdatePublication(publication: Publication = null): Promise<void> {
    if (publication != null && this.Publication.id != publication.id) {
      await this.UpdateReply(publication);
      return;
    }

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

  private async UpdateReply(publication: Publication): Promise<void> {
    if (this.Replies == null) {
      return;
    }

    const pos = this.Replies.findIndex(x => x.id == publication.id);

    if (pos == -1) {
      console.warn("Publication not found");
      return;
    }

    const updatedPublication = await this.publicationService.GetPublication(publication.id);

    if (updatedPublication.Success) {
      this.Replies.splice(pos, 1, updatedPublication.Content);
    } else {
      if (updatedPublication.Error.status == 404) {
        this.Replies.splice(pos, 1);
        return;
      }

      this.appEvents.ShowSnackBarMessage(`Could not update publication '${publication.id}.'`);
    }
  }

  public repost(publication: Publication): void {
    this.publicationService.Repost(publication.id, !publication.reposted).then(x => {
      if (x.Success) {
        this.appEvents.ShowSnackBarMessage(`You ${publication.reposted ? "unreposted" : "reposted"} ${publication.user.username}'s publication.`);

        this.updatePublication(publication);
      } else {
        this.appEvents.ShowSnackBarMessage(`An error occured while trying to ${publication.reposted ? "unrepost" : "repost"} publication '${publication.id}' (${x.Error.status})`);
      }
    });
  }

  public like(publication: Publication): void {
    this.publicationService.Like(publication.id, !publication.liked).then(x => {
      if (x.Success) {
        this.appEvents.ShowSnackBarMessage(`You ${publication.liked ? "unliked" : "liked"} ${publication.user.username}'s publication.`);

        this.updatePublication(publication);
      } else {
        this.appEvents.ShowSnackBarMessage(`An error occured while trying to ${publication.liked ? "unlike" : "like"} publication '${publication.id}' (${x.Error.status})`);
      }
    });
  }

  public delete(publication: Publication): void {
    this.publicationService.Delete(publication.id).then(x => {
      if (x.Success) {
        this.appEvents.ShowSnackBarMessage("You deleted your publication :(");

        if (this.Publication.id == publication.id) {
          this.router.navigate(["/home"]);
          return;
        }

        this.updatePublication(publication);
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
  }
}
