import { Component, OnInit } from '@angular/core';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Publication } from 'src/app/publication/models/publication';
import { PublicationService } from 'src/app/publication/services/publication.service';
import { ApiService } from 'src/app/services/api/api.service';
import { GlobalAppEventsService, MessageType } from 'src/app/services/global-app-events/global-app-events.service';
import { OperationResult } from 'src/app/services/models/operation-result';
import { FeedService } from '../../../feed/services/feed.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {
  amount: number = 50;
  lastPublication: string = null;

  publications: Publication[];

  constructor(
    private feedService: FeedService,
    private pubService: PublicationService,
    private api: ApiService,
    private appEvents: GlobalAppEventsService) { }

  ngOnInit(): void {
    this.appEvents.Loading();
    this.feedService.GetMainFeed(this.amount, null).then((x) => this.LoadPublications(x));
  }

  private LoadPublications(result: OperationResult<Publication[]>): void {
    this.appEvents.DoneLoading();
    if (result.Success) {
      this.publications = result.Content;

      if (this.publications.length == 0)
        this.lastPublication = null;
      else
        this.lastPublication = result.Content[result.Content.length - 1].id;
    } else {
      let errorText = "";
      switch (result.Error?.status) {
        default:
          console.log(result.Error);
          errorText = `An unexpected error occured while loading the main feed, please try again later. (${result.Error?.status})`;
          break;
      }

      this.appEvents.ShowMessage(errorText, MessageType.Error);
    }
  }

  public updatePublication(publication: Publication): void {
    this.UpdatePublication(publication).then();
  }

  private async UpdatePublication(publication: Publication): Promise<void> {
    if (this.publications == null) {
      return;
    }

    const pos = this.publications.findIndex(x => x.id == publication.id);

    if (pos == -1) {
      console.warn("Publication not found");
      return;
    }

    const updatedPublication = await this.pubService.GetPublication(publication.id);

    if (updatedPublication.Success) {
      this.publications.splice(pos, 1, updatedPublication.Content);
    } else {
      if (updatedPublication.Error.status == 404) {
        this.publications.splice(pos, 1);
        return;
      }

      this.appEvents.ShowSnackBarMessage(`Could not update publication '${publication.id}.'`);
    }
  }
}
