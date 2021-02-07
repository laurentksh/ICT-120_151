import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HomeComponent } from './components/home/home.component';
import { HomeRoutingModule } from './home-routing.module';
import { FeedModule } from '../feed/feed.module';
import { ContentLoaderModule } from '@ngneat/content-loader';
import { MatCardModule } from '@angular/material/card';


@NgModule({
  declarations: [HomeComponent],
  imports: [
    CommonModule,
    HomeRoutingModule,
    FeedModule,
    ContentLoaderModule,
    MatCardModule
  ]
})
export class HomeModule { }
