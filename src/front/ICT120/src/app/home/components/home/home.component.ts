import { Component, OnInit } from '@angular/core';
import { Publication } from 'src/app/publication/models/publication';
import { ApiService } from 'src/app/services/api/api.service';
import { OperationResult } from 'src/app/services/models/operation-result';
import { FeedService } from '../../../feed/services/feed.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {
  amount: number = 50;
  lastPublication: string = null;

  publications: Publication[];
  errorOccured = false;
  errorText = "";

  constructor(private feedService: FeedService, private api: ApiService) { }

  ngOnInit(): void {
    this.feedService.GetMainFeed(this.amount, null).then((x) => this.LoadPublications(x));
  }

  private LoadPublications(result: OperationResult<Publication[]>) {
    if (result.Success) {
      this.errorOccured = false;
      this.publications = result.Content;

      if (this.publications.length == 0)
        this.lastPublication = null;
      else
        this.lastPublication = result.Content[result.Content.length - 1].id;
    } else {
      this.errorOccured = true;
      this.publications = new Array<Publication>();
      
      switch (result.Error?.status) {
        default:
          this.errorText = `An unexpected error occured, please try again later. (${result.Error?.status} ${result.Error?.statusText})`;
          break;
      }
    }
  }
}
