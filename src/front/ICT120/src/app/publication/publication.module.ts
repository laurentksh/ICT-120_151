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
import { ContentLoaderModule } from '@ngneat/content-loader';
import { MatDividerModule } from '@angular/material/divider';
import { MediaModule } from '../media/media.module';
import { ViewImageDialogComponent } from './components/view-image-dialog/view-image-dialog.component';
import { MatDialogModule } from '@angular/material/dialog';
import { VgCoreModule } from '@videogular/ngx-videogular/core';
import { VgControlsModule } from '@videogular/ngx-videogular/controls';
import { VgOverlayPlayModule } from '@videogular/ngx-videogular/overlay-play';
import { VgBufferingModule } from '@videogular/ngx-videogular/buffering';

@NgModule({
  declarations: [
    PublicationComponent,
    ReplyComponent,
    NewPublicationComponent,
    ViewPublicationComponent,
    ViewImageDialogComponent
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
    MatInputModule,
    MatDividerModule,
    MatDialogModule,
    MediaModule,

    ContentLoaderModule,

    VgCoreModule,
    VgControlsModule,
    VgOverlayPlayModule,
    VgBufferingModule
  ],
  exports: [
    PublicationComponent,
    ReplyComponent
  ]
})
export class PublicationModule { }
