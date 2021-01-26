import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Login } from '../../models/login';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {

  login = {} as Login;
  errorOccured = false;
  errorText = "";
  
  constructor(private authService: AuthService, private routeService: Router) { }

  ngOnInit(): void {
    if (this.authService.IsAuthenticated) {
      this.routeService.navigate(["home"]);
    }
  }

  onLogin(): void {
    this.errorOccured = false;
    if (!this.validateInput())
      return;
    
    this.authService.Authenticate(this.login).then((x) => {
      if (x.Success) {
        //Go to /home
        this.routeService.navigate(["home"]);
      } else {
        this.errorOccured = true;

        switch (x.Error.status) {
          case 400:
            this.errorText = "Invalid email or password.";
            break;
          case 401:
          case 404:
            this.errorText = "Email/Password does not match any account.";
            break;
          default:
            this.errorText = `An unexpected error occured, please try again later. (${x.Error.status} ${x.Error.statusText})`;
            break;
        }
      }
    });
  }

  validateInput(): boolean {
    if (this.login.email == null || this.login.password == null) {
      this.errorOccured = true;
      this.errorText = "Please fill the required fields.";
      return false;
    }

    return true;
  }
}
