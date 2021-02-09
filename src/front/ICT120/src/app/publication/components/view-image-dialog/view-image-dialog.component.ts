import { Component, Inject, OnInit } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { MediaType, MediaViewModel } from 'src/app/media/models/media-view-model';

export interface ViewImageDialogData {
  media: MediaViewModel
}

@Component({
  selector: 'app-view-image-dialog',
  templateUrl: './view-image-dialog.component.html',
  styleUrls: ['./view-image-dialog.component.css']
})
export class ViewImageDialogComponent implements OnInit {

  constructor(
    public dialogRef: MatDialogRef<ViewImageDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: ViewImageDialogData
  ) { }

  ngOnInit(): void {
  }

  isImage(): boolean {
    return this.data.media.mediaType == MediaType.Image;
  }

  isVideo(): boolean {
    return this.data.media.mediaType == MediaType.Video;
  }

  close(): void {
    this.dialogRef.close();
  }
}
