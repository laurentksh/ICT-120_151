import { Component, ElementRef, EventEmitter, Input, OnInit, Output, ViewChild } from '@angular/core';
import { Router } from '@angular/router';
import { UploadMediaComponent } from 'src/app/media/components/upload-media/upload-media.component';
import { MediaContainer, MediaType, MediaViewModel } from 'src/app/media/models/media-view-model';
import { GlobalAppEventsService, MessageType } from 'src/app/services/global-app-events/global-app-events.service';
import { OperationResult } from 'src/app/services/models/operation-result';
import { CreatePublication, SubmissionType } from '../../models/create-publication';
import { Publication } from '../../models/publication';
import { PublicationService } from '../../services/publication.service';

@Component({
  selector: 'app-reply',
  templateUrl: './reply.component.html',
  styleUrls: ['./reply.component.css']
})
export class ReplyComponent implements OnInit {
  @Input() Publication: Publication;
  @Output() Result = new EventEmitter<OperationResult<Publication>>();
  @ViewChild(UploadMediaComponent) uploadMedia: UploadMediaComponent;
  Reply = {} as CreatePublication;
  locked = false;

  constructor(private publicationService: PublicationService, private router: Router, private appEvents: GlobalAppEventsService) { }

  ngOnInit(): void {
    this.Reply.replyPublicationId = this.Publication.id;
  }

  doReply(): void {
    this.appEvents.Loading();
    this.appEvents.HideMessage();
    
    if (!this.validateInput()) {
      this.appEvents.DoneLoading();
      return;
    }

    this.publicationService.CreateNew(this.Reply).then((x) => {
      if (x.Success) {
        this.appEvents.ShowSnackBarMessage(`Your replied to ${this.Publication.user.username}.`)
      } else {
        let errorText = "";

        switch (x.Error.status) {
          case 400:
            errorText = "Invalid fields, please make sure you filled the form properly and try again.";
            break;
          case 401:
            this.router.navigate(["/login", { redirect: this.router.url, reason: "Authentication error (401)." }])
            break;
          default:
            errorText = `An unexpected error occured, please try again later. (${x.Error.status} ${x.Error.statusText})`;
            break;
        }

        this.appEvents.ShowSnackBarMessage(errorText, "Ok", 5000);
      }

      this.Result.next(x);
      this.appEvents.DoneLoading();
    })
  }

  validateInput(): boolean {
    if (this.Reply.textContent == null) {
      this.appEvents.ShowMessage("Please fill the required fields.", MessageType.Error);
      return false;
    }

    return true;
  }

  resetForm(): void {
    this.uploadMedia.resetInput();
    this.Reply.textContent = "";
    this.Reply.submissionType = SubmissionType.Text;
    this.Reply.mediaId = null;
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
        this.Reply.submissionType = SubmissionType.Image;
        this.Reply.mediaId = media.Content.id;
      } else if (media.Content.mediaType == MediaType.Video) {
        this.Reply.submissionType = SubmissionType.Video;
        this.Reply.mediaId = media.Content.id;
      } else
        this.Reply.submissionType = SubmissionType.Text;
    } else {
      this.Reply.submissionType = SubmissionType.Text;
      this.Reply.mediaId = null;
      this.appEvents.ShowSnackBarMessage(`An error occured while uploading the media, please try again. (${media.Error.status})`);
    }
  }
}
