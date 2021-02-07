import { Component, OnInit } from '@angular/core';
import { UpdateUser } from '../../models/update-user';
import { UserSummary } from '../../models/user-summary';

@Component({
  selector: 'app-user-edit',
  templateUrl: './user-edit.component.html',
  styleUrls: ['./user-edit.component.css']
})
export class UserEditComponent implements OnInit {

  User: UserSummary;
  UserChange: UpdateUser;
  //UserProfilePicture: MediaViewModel;
  
  constructor() { }

  ngOnInit(): void {
  }

}
