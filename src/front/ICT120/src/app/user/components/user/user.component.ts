import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Params } from '@angular/router';
import { Publication } from 'src/app/publication/models/publication';
import { FeedService } from 'src/feed/services/feed.service';
import { UserSummary } from '../../models/user-summary';
import { UserService } from '../../services/user.service';

@Component({
  selector: 'app-user',
  templateUrl: './user.component.html',
  styleUrls: ['./user.component.css']
})
export class UserComponent implements OnInit {
  amount: number = 50;
  lastPublication: string = null;

  User: UserSummary = {} as UserSummary;
  Publications: Publication[];

  error: boolean = false;
  errorMsg: string = "";

  constructor(private route: ActivatedRoute, private userService: UserService, private feedService: FeedService) {

  }

  ngOnInit(): void {
      this.route.params.subscribe(async params => {
        await this.Load(params);
      });
  }

  private async Load(params: Params): Promise<void> {
    const id = params['identifier'];
    
    if (id == null) {
      this.error = true;
      this.errorMsg = "Missing parameter 'identifier'.";
      return;
    }

    const result = await this.userService.GetUser(id);
    const success = result.Success;
    const user = result.Content;
    const error = result.Error;

    if (success) {
      this.User = user;

      await this.LoadPublications();
    } else {
      this.error = true;
      this.errorMsg = `Could not load specified user. (${error.status ?? ""})`;
    }
  }

  private async LoadPublications(): Promise<void> {
    if (this.User == null) {
      this.error = true;
      this.errorMsg = `Could not load specified user.`;
      return;
    }

    const request = await this.feedService.GetFeed(this.amount, this.lastPublication, this.User.id);

    if (request.Success) {
      this.Publications = request.Content;
    } else {
      this.Publications = null;
      this.error = true;
      this.errorMsg = `Could not load publications (${request.Error.status ?? ""} ${request.Error.statusText ?? ""})`;
    }
  }
}
