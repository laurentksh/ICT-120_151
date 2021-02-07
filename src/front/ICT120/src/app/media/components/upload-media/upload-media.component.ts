import { Component, ElementRef, EventEmitter, Input, OnInit, Output, ViewChild } from '@angular/core';
import { GlobalAppEventsService } from 'src/app/services/global-app-events/global-app-events.service';
import { OperationResult } from 'src/app/services/models/operation-result';
import { MediaContainer, MediaType, MediaViewModel } from '../../models/media-view-model';
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
  @ViewChild("previewImg") PreviewImg: ElementRef;
  @ViewChild("previewVid") PreviewVid: ElementRef;
  @ViewChild("previewVidSrc") PreviewVidSrc: ElementRef;

  showImgPreview = false;
  showVidPreview = false;

  constructor(private mediaService: MediaService, private appEvents: GlobalAppEventsService) { }

  ngOnInit(): void {
  }

  doUpload(): void {
    debugger;
    const dom = this.FileInput.nativeElement as HTMLInputElement;
    const previewImgDom = this.PreviewImg.nativeElement as HTMLImageElement;
    const previewVidDom = this.PreviewVid.nativeElement as HTMLVideoElement;
    const file = dom.files[0];
    
    this.mediaService.UploadMedia(file, this.Container).then(x => {
      this.UploadResult.emit(x);

      //previewDom.src = URL.createObjectURL(file);
      if (x.Content.mediaType == MediaType.Image) {
        this.showImgPreview = true;

        previewImgDom.src = x.Content.blobFullUrl;
      } else {
        this.showVidPreview = true;
        
        const src = this.PreviewVidSrc.nativeElement as HTMLSourceElement;
        src.src = x.Content.blobFullUrl;
        src.type = x.Content.mimeType;
      }
    });
  }

}
