import { Component, OnInit } from '@angular/core';
import { Signup } from '../../models/signup';

@Component({
  selector: 'app-signup',
  templateUrl: './signup.component.html',
  styleUrls: ['./signup.component.css']
})
export class SignupComponent implements OnInit {

  signup = {} as Signup;
  errorOccured = false;
  errorText = "";

  constructor() { }

  ngOnInit(): void {
  }

  onSignup(): void {
    
  }

}
