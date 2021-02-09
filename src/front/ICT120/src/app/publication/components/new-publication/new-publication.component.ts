import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { MediaContainer, MediaType, MediaViewModel } from 'src/app/media/models/media-view-model';
import { GlobalAppEventsService, MessageType } from 'src/app/services/global-app-events/global-app-events.service';
import { OperationResult } from 'src/app/services/models/operation-result';
import { CreatePublication, SubmissionType } from '../../models/create-publication';
import { PublicationService } from '../../services/publication.service';

@Component({
  selector: 'app-new-publication',
  templateUrl: './new-publication.component.html',
  styleUrls: ['./new-publication.component.css']
})
export class NewPublicationComponent implements OnInit {
  publication = {} as CreatePublication;
  locked = false;

  constructor(private publicationService: PublicationService, private router: Router, private appEvents: GlobalAppEventsService) { }

  ngOnInit(): void {
  }

  onSubmit(): void {
    this.appEvents.Loading();
    this.appEvents.HideMessage();
    
    if (!this.validateInput()) {
      this.appEvents.DoneLoading();
      return;
    }

    this.publicationService.CreateNew(this.publication).then((x) => {
      if (x.Success) {
        this.router.navigate(["publication/id", x.Content.id]);
      } else {
        let errorText = "";

        switch (x.Error.status) {
          case 400:
            errorText = "Invalid fields, please make sure you filled the form properly and try again.";
            break;
          case 401:
            this.router.navigate(["/login", { redirect: this.router.url }])
            break;
          default:
            errorText = `An unexpected error occured, please try again later. (${x.Error.status} ${x.Error.statusText})`;
            break;
        }

        this.appEvents.ShowMessage(errorText, MessageType.Error);
      }
      this.appEvents.DoneLoading();
    })
  }

  validateInput(): boolean {
    if (this.publication.textContent == null) {
      this.appEvents.ShowMessage("Please fill the required fields.", MessageType.Error);
      return false;
    }

    return true;
  }

  getContainer(): MediaContainer {
    return MediaContainer.Publication;
  }

  beginUpload(): void {
    this.locked = true;
    this.appEvents.Loading();
  }

  mediaUploaded(media: OperationResult<MediaViewModel>): void {
    this.locked = false;
    this.appEvents.DoneLoading();
    if (media.Success) {
      if (media.Content.mediaType == MediaType.Image) {
        this.publication.submissionType = SubmissionType.Image;
        this.publication.mediaId = media.Content.id;
      } else if (media.Content.mediaType == MediaType.Video) {
        this.publication.submissionType = SubmissionType.Video;
        this.publication.mediaId = media.Content.id;
      } else
        this.publication.submissionType = SubmissionType.Text;
    } else {
      this.publication.submissionType = SubmissionType.Text;
      this.publication.mediaId = null;
      this.appEvents.ShowSnackBarMessage(`An error occured while uploading the media, please try again. (${media.Error.status})`);
    }
  }
}
