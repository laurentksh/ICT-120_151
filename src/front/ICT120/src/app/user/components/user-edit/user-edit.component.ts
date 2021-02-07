import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
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
  UserChange: UpdateUser;
  //UserProfilePicture: MediaViewModel;
  
  constructor(private userService: UserService, private routerService: Router, private appEvents: GlobalAppEventsService) { }

  ngOnInit(): void {
  }

  submit(): void {
    this.userService.Edit(this.UserChange).then(x => {
      if (x.Success) {
        this.routerService.navigate(["/u", this.User.id]);
      } else {
        this.appEvents.ShowMessage(`Could not update user (${x.Error.status})`, MessageType.Error);
      }
    });
  }

}
