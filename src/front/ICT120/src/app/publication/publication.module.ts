import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { PublicationComponent } from './components/publication/publication.component';
import { ReplyComponent } from './components/reply/reply.component';
import { PublicationRoutingModule } from './publication-routing.module';
import { NewPublicationComponent } from './components/new-publication/new-publication.component';
import { FormsModule } from '@angular/forms';
import { BrowserModule } from '@angular/platform-browser';
import { ViewPublicationComponent } from './components/view-publication/view-publication.component';
import { FeedModule } from 'src/feed/feed.module';



@NgModule({
  declarations: [
    PublicationComponent,
    ReplyComponent,
    NewPublicationComponent,
    ViewPublicationComponent
  ],
  imports: [
    PublicationRoutingModule,
    FormsModule,
    CommonModule,
    BrowserModule,
    FeedModule
  ],
  exports: [PublicationComponent]
})
export class PublicationModule { }
