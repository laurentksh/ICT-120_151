import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Params } from '@angular/router';
import { UserSummary } from '../../models/user-summary';
import { UserService } from '../../services/user.service';

@Component({
  selector: 'app-user',
  templateUrl: './user.component.html',
  styleUrls: ['./user.component.css']
})
export class UserComponent implements OnInit {
  User: UserSummary;

  error: boolean = false;
  errorMsg: string;

  constructor(private route: ActivatedRoute, private userService: UserService) {

  }

  ngOnInit(): void {
    this.route.params.toPromise().then(x => this.Load(x));
  }

  private async Load(params: Params) {
    const id = params['id'];

    this.User = await this.userService.GetUser(id);
  }
}
