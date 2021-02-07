import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { PublicationComponent } from './components/publication/publication.component';
import { ReplyComponent } from './components/reply/reply.component';
import { PublicationRoutingModule } from './publication-routing.module';
import { NewPublicationComponent } from './components/new-publication/new-publication.component';
import { FormsModule } from '@angular/forms';
import { BrowserModule } from '@angular/platform-browser';
import { ViewPublicationComponent } from './components/view-publication/view-publication.component';
import { ClipboardModule } from '@angular/cdk/clipboard';
import { MatMenuModule } from '@angular/material/menu';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatIconModule } from '@angular/material/icon';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { UploadMediaComponent } from '../media/components/upload-media/upload-media.component';


@NgModule({
  declarations: [
    PublicationComponent,
    ReplyComponent,
    NewPublicationComponent,
    ViewPublicationComponent,
    UploadMediaComponent
  ],
  imports: [ //WARNING: Importing FeedModule from here causes a "Maximum call stack size exceeded" exception !
    PublicationRoutingModule,
    FormsModule,
    CommonModule,
    BrowserModule,
    ClipboardModule,
    MatMenuModule,
    MatButtonModule,
    MatCardModule,
    MatIconModule,
    MatFormFieldModule,
    MatInputModule
  ],
  exports: [
    PublicationComponent
  ]
})
export class PublicationModule { }
