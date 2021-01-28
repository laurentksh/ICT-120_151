import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { UserRoutingModule } from './user-routing.module';
import { UserComponent } from './components/user/user.component';
import { UserSummaryComponent } from './components/user-summary/user-summary.component'
import { FeedModule } from '../feed/feed.module';


@NgModule({
  declarations: [
    UserComponent,
    UserSummaryComponent
  ],
  imports: [
    CommonModule,
    UserRoutingModule,
    FeedModule
  ]
})
export class UserModule { }