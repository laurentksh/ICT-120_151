import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { UploadMediaComponent } from './components/upload-media/upload-media.component';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatCardModule } from '@angular/material/card';
import { MatInputModule } from '@angular/material/input';

@NgModule({
  declarations: [
    UploadMediaComponent
  ],
  imports: [
    CommonModule,
    MatFormFieldModule,
    MatCardModule,
    MatInputModule
  ],
  exports: [
    UploadMediaComponent
  ]
})
export class MediaModule { }
