import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FeedComponent } from './components/feed/feed.component';
import { PublicationModule } from 'src/app/publication/publication.module';
import { HtmlAddonsModule } from 'src/app/html-addons/html-addons.module';



@NgModule({
  declarations: [
    FeedComponent
  ],
  imports: [
    CommonModule,
    PublicationModule,
    HtmlAddonsModule
  ],
  exports: [
    FeedComponent
  ]
})
export class FeedModule { }
