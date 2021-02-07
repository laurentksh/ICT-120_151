import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Params, Router } from '@angular/router';
import { AuthService } from 'src/app/auth/services/auth.service';
import { MediaViewModel } from 'src/app/media/models/media-view-model';
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

  User: UserSummary = null;
  UserProfilePicture: MediaViewModel = null;
  Publications: Publication[] = null;

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private userService: UserService,
    private authService: AuthService,
    private pubService: PublicationService,
    private feedService: FeedService,
    private mediaService: MediaService,
    private appEvents: GlobalAppEventsService
  ) {}

  ngOnInit(): void {
    this.appEvents.Loading();
      this.route.params.subscribe(async params => {
        await this.Load(params).then(() => this.LoadPublications());
      });
  }

  isSelf(): boolean {
    if (!this.authService.IsAuthenticated)
      return false;
    
    return this.authService.LocalUser.id == this.User.id;
  }

  doFollow(): void {
    if (!this.authService.IsAuthenticated) {
      this.router.navigate(["/login"], { queryParams: { "redirect": this.router.url } });
      return;
    }

    if (this.isSelf()) {
      this.appEvents.ShowSnackBarMessage("You cannot follow yourself !");
      return;
    }

    this.userService.Follow(this.User.id, !this.User.following).then(x => {
      if (x.Success) {
        this.appEvents.ShowSnackBarMessage(`You ${this.User.following ? 'unfollowed' : 'followed'} ${this.User.username} !`);
      } else {
        this.appEvents.ShowSnackBarMessage(`Could not follow/unfollow ${this.User.username} (${x.Error.status})`);
      }

      this.updateUser();
    });
  }

  doBlock(): void {
    if (!this.authService.IsAuthenticated) {
      this.router.navigate(["/login"], { queryParams: { "redirect": this.router.url } });
      return;
    }

    if (this.isSelf()) {
      this.appEvents.ShowSnackBarMessage("You cannot block yourself !");
      return;
    }

    this.userService.Block(this.User.id, !this.User.blocking).then(x => {
      if (x.Success) {
        this.appEvents.ShowSnackBarMessage(`You ${this.User.blocking ? 'unblocked' : 'blocked'} ${this.User.username} !`);
      } else {
        this.appEvents.ShowSnackBarMessage(`Could not block/unblock ${this.User.username} (${x.Error.status})`);
      }

      this.updateUser();
    })
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
      this.User.birthday = new Date(this.User.birthday);

      await this.LoadProfilePicture();
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
      this.UserProfilePicture = request.Content;
    } else {
      this.appEvents.ShowMessage(`Could not load profile picture (${request.Error.status ?? ""})`, MessageType.Error);
    }
  }

  public updateUser(): void {
    this.appEvents.Loading();
    this.UpdateUser().then();
  }

  public updatePublication(publication: Publication): void {
    this.appEvents.Loading();
    this.UpdatePublication(publication).then();
  }

  private async UpdatePublication(publication: Publication): Promise<void> {
    if (this.Publications == null) {
      this.appEvents.DoneLoading();
      return;
    }

    const pos = this.Publications.findIndex(x => x.id == publication.id);

    if (pos == -1) {
      console.warn("Publication not found");
      this.appEvents.DoneLoading();
      return;
    }

    const updatedPublication = await this.pubService.GetPublication(publication.id);

    if (updatedPublication.Success) {
      this.Publications.splice(pos, 1, updatedPublication.Content);
    } else {
      if (updatedPublication.Error.status == 404) {
        this.Publications.splice(pos, 1);
        this.appEvents.DoneLoading();
        return;
      }

      this.appEvents.ShowSnackBarMessage(`Could not update publication '${publication.id}.'`);
    }
    this.appEvents.DoneLoading();
  }

  private async UpdateUser(): Promise<void> {
    if (this.User == null) {
      this.appEvents.ShowMessage("User is null", MessageType.Error);
      this.appEvents.DoneLoading();
      return;
    }

    const result = await this.userService.GetUser(this.User.id);

    if (result.Success) {
      this.User = result.Content;
      this.User.birthday = new Date(this.User.birthday);
      this.User.creationDate = new Date(this.User.creationDate);
    } else {
      this.appEvents.ShowMessage(`Could not update user. (${result.Error.status ?? ""})`, MessageType.Error);
    }

    this.appEvents.DoneLoading();
  }
}
