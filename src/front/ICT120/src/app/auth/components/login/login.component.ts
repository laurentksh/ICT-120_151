import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { GlobalAppEventsService, MessageType } from 'src/app/services/global-app-events/global-app-events.service';
import { Login } from '../../models/login';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {

  login = {} as Login;
  redirect: string = "/home";
  hidePassword = true;
  
  constructor(private authService: AuthService, private routeService: Router, private route: ActivatedRoute, private appEvents: GlobalAppEventsService) { }

  ngOnInit(): void {
    this.route.queryParams.subscribe(x => {
      const redirectTo = x["redirect"];

      if (redirectTo != null)
        this.redirect = redirectTo;
    });
    
    if (this.authService.IsAuthenticated) {
      this.routeService.navigate([this.redirect]);
    }
  }

  onLogin(): void {
    this.appEvents.HideMessage();
    this.appEvents.Loading();

    if (!this.validateInput()) {
      this.appEvents.DoneLoading();
      return;
    }
    
    this.authService.Authenticate(this.login).then((x) => {
      this.appEvents.DoneLoading();
      if (x.Success) {
        //Go to /home or redirect
        this.routeService.navigate([this.redirect]);
      } else {
        let errorText = "undefined";

        switch (x.Error.status) {
          case 400:
            errorText = "Invalid email or password.";
            break;
          case 401:
          case 404:
            errorText = "Email/Password does not match any account.";
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
    if (this.login.email == null || this.login.password == null) {
      this.appEvents.ShowMessage("Please fill the required fields.", MessageType.Error);
      return false;
    }

    return true;
  }
}
