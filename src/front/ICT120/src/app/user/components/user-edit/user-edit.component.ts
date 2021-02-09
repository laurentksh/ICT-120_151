import { Component, OnInit } from '@angular/core';
import { DateAdapter } from '@angular/material/core';
import { Router } from '@angular/router';
import { Subject } from 'rxjs';
import { AuthService } from 'src/app/auth/services/auth.service';
import { MediaContainer, MediaType, MediaViewModel } from 'src/app/media/models/media-view-model';
import { MediaService } from 'src/app/media/services/media.service';
import { SubmissionType } from 'src/app/publication/models/create-publication';
import { GlobalAppEventsService, MessageType } from 'src/app/services/global-app-events/global-app-events.service';
import { OperationResult } from 'src/app/services/models/operation-result';
import { UpdateUser } from '../../models/update-user';
import { UserSummary } from '../../models/user-summary';
import { UserService } from '../../services/user.service';

@Component({
  selector: 'app-user-edit',
  templateUrl: './user-edit.component.html',
  styleUrls: ['./user-edit.component.css']
})
export class UserEditComponent implements OnInit {

  User: UserSummary;
  UserChange: UpdateUser = {} as UpdateUser;
  startDate = new Date(2000, 0);
  locked = false;
  openFileInput = new Subject<void>();
  resetFileInput = new Subject<void>();
  currentProfilePicUrl: string = null;
  containerType = MediaContainer.ProfilePicture;
  //UserProfilePicture: MediaViewModel;
  
  constructor(
    private userService: UserService,
    private routerService: Router,
    private authService: AuthService,
    private mediaService: MediaService,
    private appEvents: GlobalAppEventsService) {

  }

  ngOnInit(): void {
    this.refreshUser();
  }

  refreshUser(): void {
    this.RefreshUserAsync().then();
  }

  async RefreshUserAsync(): Promise<void> {
    const x = await this.authService.UpdateUser();
      if (!x.Success) {
        this.appEvents.ShowMessage("Could not load user data.", MessageType.Error);
        return;
      }

      this.User = this.authService.LocalUser;
      console.log(this.User);
      this.UserChange.Username = this.User.username;
      this.UserChange.Email = this.User.email;
      this.UserChange.BirthDay = this.User.birthday;
      this.UserChange.Biography = this.User.biography;
      
      const media = await this.mediaService.GetMedia(this.User.profilePictureId);
      this.setProfilePictureCallback(media);
  }

  submit(): void {
    this.appEvents.Loading();
    this.userService.Edit(this.UserChange).then(x => {
      this.appEvents.DoneLoading();
      if (x.Success) {
        this.authService.UpdateUser().then(() => {
          this.routerService.navigate(["/u", this.User.id]);
        });
      } else {
        if (x.Error.error.errors.Username) {
          this.appEvents.ShowMessage("Invalid username.", MessageType.Error);
        } else if (x.Error.error.errors.BirthDay) {
          this.appEvents.ShowMessage("Invalid birthday.", MessageType.Error);
        } else {
          this.appEvents.ShowMessage(`Could not update user: (${x.Error.status})`, MessageType.Error);
        }
      }
    });
  }

  delete():void {
    this.appEvents.Loading();
    this.resetFileInput.next();
    
    this.userService.DeleteProfilePicture().then(x => {
      this.appEvents.DoneLoading();
      if (x.Success) {
        this.refreshUser();
        this.appEvents.ShowSnackBarMessage("Profile picture successfully removed.");
      } else {
        this.appEvents.ShowSnackBarMessage(`An error occured while deleting your profile picture, please try again later. (${x.Error.status})`)
      }
    });
  }

  beginUpload(): void {
    this.locked = true;
    this.appEvents.Loading();
  }

  mediaUploaded(media: OperationResult<MediaViewModel>): void {
    this.locked = false;
    this.appEvents.DoneLoading();

    if (media.Success) {
      if (media.Content.mediaType == MediaType.Image) {
        this.userService.SetProfilePicture(media.Content.id).then(async x => {
          await this.RefreshUserAsync();
        });
      } else {
        this.appEvents.ShowMessage(`Invalid media type '${media.Content.mimeType}'`, MessageType.Error);
      }
    } else {
      this.appEvents.ShowSnackBarMessage(`An error occured while uploading the media, please try again. (${media.Error.status})`);
    }
  }

  setProfilePictureCallback(result: OperationResult<MediaViewModel>): void {
    if (result.Success) {
      this.currentProfilePicUrl = result.Content.blobFullUrl;
    } else {
      this.appEvents.ShowSnackBarMessage(`An error occured while loading your profile picture. (${result.Error.status})`)
    }
  }

  doOpenFileInput(): void {
    this.openFileInput.next();
  }
}
