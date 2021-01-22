import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { PublicationComponent } from './components/publication/publication.component';
import { ReplyComponent } from './components/reply/reply.component';
import { PublicationRoutingModule } from './publication-routing.module';



@NgModule({
  declarations: [
    PublicationComponent,
    ReplyComponent
  ],
  imports: [
    PublicationRoutingModule
  ]
})
export class PublicationModule { }
