import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Params } from '@angular/router';
import { MediaService } from 'src/app/media/services/media.service';
import { Publication } from 'src/app/publication/models/publication';
import { PublicationService } from 'src/app/publication/services/publication.service';
import { GlobalAppEventsService, MessageType } from 'src/app/services/global-app-events/global-app-events.service';
import { FeedService } from '../../../feed/services/feed.service';
import { UserSummary } from '../../models/user-summary';
import { UserService } from '../../services/user.service';

@Component({
  selector: 'app-user',
  templateUrl: './user.component.html',
  styleUrls: ['./user.component.css']
})
export class UserComponent implements OnInit {
  amount: number = 50;
  lastPublication: string = null;

  User: UserSummary;
  UserProfilePictureUrl: string;
  UserProfilePictureMime: string;
  Publications: Publication[];

  constructor(
    private route: ActivatedRoute,
    private userService: UserService,
    private pubService: PublicationService,
    private feedService: FeedService,
    private mediaService: MediaService,
    private appEvents: GlobalAppEventsService
  ) {}

  ngOnInit(): void {
    this.appEvents.Loading();
      this.route.params.subscribe(async params => {
        await this.Load(params);
      });
  }

  private async Load(params: Params): Promise<void> {
    const id = params['identifier'];
    
    if (id == null) {
      this.appEvents.ShowMessage("Missing parameter 'identifier'.", MessageType.Error);
      return;
    }

    const result = await this.userService.GetUser(id);

    if (result.Success) {
      this.User = result.Content;

      await this.LoadPublications();
    } else {
      this.appEvents.ShowMessage(`Could not load specified user. (${result.Error.status ?? ""})`, MessageType.Error);
    }

    this.appEvents.DoneLoading();
  }

  private async LoadPublications(): Promise<void> {
    if (this.User == null) {
      this.appEvents.ShowMessage("Could not load specified user.", MessageType.Error);
      return;
    }

    const request = await this.feedService.GetFeed(this.amount, this.lastPublication, this.User.id);

    if (request.Success) {
      this.Publications = request.Content;

      await this.LoadProfilePicture();
    } else {
      this.Publications = null;
      this.appEvents.ShowMessage(`Could not load publications (${request.Error.status ?? ""})`, MessageType.Error);
    }
  }

  private async LoadProfilePicture(): Promise<void> {
    if (this.User == null) {
      this.appEvents.ShowMessage("Could not load profile picture.", MessageType.Error);
      return;
    }

    const request = await this.mediaService.GetMedia(this.User.profilePictureId);
    
    if (request.Success) {
      this.UserProfilePictureUrl = request.Content.blobFullUrl;
      this.UserProfilePictureMime = request.Content.mimeType;
    } else {
      this.appEvents.ShowMessage(`Could not load profile picture (${request.Error.status ?? ""})`, MessageType.Error);
    }
  }

  public updatePublication(publication: Publication): void {
    this.UpdatePublication(publication).then();
  }

  private async UpdatePublication(publication: Publication): Promise<void> {
    if (this.Publications == null) {
      return;
    }

    const pos = this.Publications.findIndex(x => x.id == publication.id);

    if (pos == -1) {
      console.warn("Publication not found");
      return;
    }

    const updatedPublication = await this.pubService.GetPublication(publication.id);

    if (updatedPublication.Success) {
      this.Publications.splice(pos, 1, updatedPublication.Content);
    } else {
      if (updatedPublication.Error.status == 404) {
        this.Publications.splice(pos, 1);
        return;
      }

      this.appEvents.ShowSnackBarMessage(`Could not update publication '${publication.id}.'`);
    }
  }
}
