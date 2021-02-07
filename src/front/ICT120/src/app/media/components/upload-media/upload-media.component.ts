import { Component, ElementRef, EventEmitter, Input, OnInit, Output, ViewChild } from '@angular/core';
import { GlobalAppEventsService } from 'src/app/services/global-app-events/global-app-events.service';
import { OperationResult } from 'src/app/services/models/operation-result';
import { MediaContainer, MediaViewModel } from '../../models/media-view-model';
import { MediaService } from '../../services/media.service';

@Component({
  selector: 'app-upload-media',
  templateUrl: './upload-media.component.html',
  styleUrls: ['./upload-media.component.css']
})
export class UploadMediaComponent implements OnInit {
  @Input() Container: MediaContainer;
  @Output() UploadResult = new EventEmitter<OperationResult<MediaViewModel>>();
  @ViewChild("fileInput") FileInput: ElementRef;

  constructor(private mediaService: MediaService, private appEvents: GlobalAppEventsService) { }

  ngOnInit(): void {
  }

  doUpload(): void {
    const dom = this.FileInput.nativeElement as HTMLInputElement;
    const file = dom.files[0];
    
    this.mediaService.UploadMedia(file, this.Container).then(x => {
      this.UploadResult.emit(x);
    });
  }

}
