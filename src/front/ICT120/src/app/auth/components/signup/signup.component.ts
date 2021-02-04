import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Signup } from '../../models/signup';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-signup',
  templateUrl: './signup.component.html',
  styleUrls: ['./signup.component.css']
})
export class SignupComponent implements OnInit {

  signup: Signup = {} as Signup;
  errorOccured: boolean = false;
  errorText: string = "";

  constructor(private authService: AuthService, private routeService: Router) { }

  ngOnInit(): void {
  }

  onSignup(): void {
    this.errorOccured = false;
    if (!this.validateInput())
      return;

    this.authService.Signup(this.signup).then((x) => {
      if (x.Success) {
        //Go to /u/{username}
        debugger;
        this.routeService.navigate(["u/", x.Content.username]);
      } else {
        this.errorOccured = true;

        switch (x.Error.status) {
          case 400:
            this.errorText = "Invalid fields, please make sure you filled the form properly and try again.";
            break;
          default:
            this.errorText = `An unexpected error occured, please try again later. (${x.Error.status} ${x.Error.statusText})`;
            break;
        }
      }
    }).catch((x) => console.warn(x));
  }

  validateInput(): boolean {
    if (this.signup.email == null ||
      this.signup.username == null ||
      this.signup.password == null ||
      this.signup.birthDay == null) {
      this.errorOccured = true;
      this.errorText = "Please fill the required fields.";
      
      return false;
    }

    return true;
  }
}
