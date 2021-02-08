import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from 'src/app/auth/services/auth.service';
import { GlobalAppEventsService, MessageType } from 'src/app/services/global-app-events/global-app-events.service';
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
  //UserProfilePicture: MediaViewModel;
  
  constructor(private userService: UserService, private routerService: Router, private authService: AuthService, private appEvents: GlobalAppEventsService) { }

  ngOnInit(): void {
    this.User = this.authService.LocalUser;
    this.UserChange.Biography = this.User.biography;
  }

  submit(): void {
    this.userService.Edit(this.UserChange).then(x => {
      if (x.Success) {
        this.authService.UpdateUser();
        this.routerService.navigate(["/u", this.User.id]);
      } else {
        if (x.Error.error.errors.Username)
          this.appEvents.ShowMessage("Invalid username.", MessageType.Error);
        else if (x.Error.error.errors.BirthDay)
          this.appEvents.ShowMessage("Invalid birthday.", MessageType.Error);
        else
          this.appEvents.ShowMessage(`Could not update user: (${x.Error.status})`, MessageType.Error);
      }
    });
  }
}
