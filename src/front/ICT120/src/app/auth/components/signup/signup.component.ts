import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { GlobalAppEventsService, MessageType } from 'src/app/services/global-app-events/global-app-events.service';
import { Signup } from '../../models/signup';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-signup',
  templateUrl: './signup.component.html',
  styleUrls: ['./signup.component.css']
})
export class SignupComponent implements OnInit {

  signup: Signup = {} as Signup;
  hidePassword = true;
  
  constructor(private authService: AuthService, private routeService: Router, private appEvents: GlobalAppEventsService) { }

  ngOnInit(): void {
  }

  onSignup(): void {
    this.appEvents.HideMessage();
    this.appEvents.Loading();

    if (!this.validateInput()) {
      this.appEvents.DoneLoading();
      return;
    }

    this.authService.Signup(this.signup).then((x) => {
      this.appEvents.DoneLoading();
      if (x.Success) {
        //Go to /u/{username}
        debugger;
        this.routeService.navigate(["u/", x.Content.username]);
      } else {
        let errorText = "";

        switch (x.Error.status) {
          case 400:
            if (x.Error.error.errors.Email)
              errorText = "Invalid email.";
            else if (x.Error.error.errors.Username)
              errorText = "Invalid username.";
            else if (x.Error.error.errors.Password)
              errorText = "Invalid password.";
            else if (x.Error.error.errors.BirthDay)
              errorText = "Invalid birthday (you must be 13 years old to use the service).";
            else
              errorText = "Invalid fields, please make sure you filled the form properly and try again.";
            break;
          default:
            errorText = `An unexpected error occured, please try again later. (${x.Error.status} ${x.Error.statusText})`;
            break;
        }

        this.appEvents.ShowMessage(errorText, MessageType.Error);
      }
    });
  }

  validateInput(): boolean {
    if (this.signup.email == null ||
      this.signup.username == null ||
      this.signup.password == null ||
      this.signup.birthDay == null) {
      this.appEvents.ShowMessage("Please fill the required fields.", MessageType.Error);
      return false;
    }

    return true;
  }
}
