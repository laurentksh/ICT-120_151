import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { CreatePublication } from '../../models/create-publication';
import { PublicationService } from '../../services/publication.service';

@Component({
  selector: 'app-new-publication',
  templateUrl: './new-publication.component.html',
  styleUrls: ['./new-publication.component.css']
})
export class NewPublicationComponent implements OnInit {
  publication = {} as CreatePublication;
  errorOccured = false;
  errorText = "";

  constructor(private publicationService: PublicationService, private router: Router) { }

  ngOnInit(): void {
  }

  onSubmit(): void {
    this.errorOccured = false;
    if (!this.validateInput())
      return;

    this.publicationService.CreateNew(this.publication).then((x) => {
      if (x.Success) {
        this.router.navigate(["publication/id", { id: x.Content.id }])
      } else {
        this.errorOccured = true;

        switch (x.Error.status) {
          case 400:
            this.errorText = "Invalid fields, please make sure you filled the form properly and try again.";
            break;
          case 401:
            this.errorText = "It appears you are not logged in, this shouldn't be possible. Please try logging out and logging in again.";
          default:
            this.errorText = `An unexpected error occured, please try again later. (${x.Error.status} ${x.Error.statusText})`;
            break;
        }
      }
    })
  }

  validateInput(): boolean {
    if (this.publication.TextContent == null || this.publication.SubmissionType == null) {
      this.errorOccured = true;
      this.errorText = "Please fill the required fields.";
      return false;
    }

    return true;
  }
}
