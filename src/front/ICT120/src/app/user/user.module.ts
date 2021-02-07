import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { UserRoutingModule } from './user-routing.module';
import { UserComponent } from './components/user/user.component';
import { UserSummaryComponent } from './components/user-summary/user-summary.component'
import { FeedModule } from '../feed/feed.module';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { UserEditComponent } from './components/user-edit/user-edit.component';
import { FormsModule } from '@angular/forms';
import { MatCardModule } from '@angular/material/card';
import { ContentLoaderModule } from '@ngneat/content-loader';


@NgModule({
  declarations: [
    UserComponent,
    UserSummaryComponent,
    UserEditComponent
  ],
  imports: [
    CommonModule,
    UserRoutingModule,
    FeedModule,
    MatButtonModule,
    MatIconModule,
    MatCardModule,
    FormsModule,
    ContentLoaderModule
  ]
})
export class UserModule { }