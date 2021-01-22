import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { BrowserModule } from '@angular/platform-browser';
import { AppComponent } from './app.component';
import { AuthModule } from './auth/auth.module';
import { UserModule } from './user/user.module';
import { AppRoutingModule } from './app-routing.module';
import { HomeModule } from './home/home.module';
import { PublicationComponent } from './publication/components/publication/publication.component';
import { ReplyComponent } from './publication/components/reply/reply.component';
import { LeftSidebarComponent } from './main-components/left-sidebar/left-sidebar.component';
import { RightSidebarComponent } from './main-components/right-sidebar/right-sidebar.component';



@NgModule({
  declarations: [
    AppComponent,
    LeftSidebarComponent,
    RightSidebarComponent
  ],
  imports: [
    AuthModule,
    UserModule,
    HomeModule,
    CommonModule,
    BrowserModule,
    AppRoutingModule
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
