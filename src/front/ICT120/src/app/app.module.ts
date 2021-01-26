import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { BrowserModule } from '@angular/platform-browser';
import { AppComponent } from './app.component';
import { AuthModule } from './auth/auth.module';
import { UserModule } from './user/user.module';
import { AppRoutingModule } from './app-routing.module';
import { HomeModule } from './home/home.module';
import { LeftSidebarComponent } from './main-components/left-sidebar/left-sidebar.component';
import { RightSidebarComponent } from './main-components/right-sidebar/right-sidebar.component';
import { PublicationModule } from './publication/publication.module';
import { HTTP_INTERCEPTORS } from '@angular/common/http';
import { SessionTokenInterceptor } from './http-interceptors/session-token.interceptor';
import { HtmlAddonsModule } from './html-addons/html-addons.module';



@NgModule({
  declarations: [
    AppComponent,
    LeftSidebarComponent,
    RightSidebarComponent,
  ],
  imports: [
    AuthModule,
    UserModule,
    HomeModule,
    PublicationModule,
    HtmlAddonsModule,
    
    CommonModule,
    BrowserModule,
    AppRoutingModule
  ],
  bootstrap: [AppComponent],
  providers: [
    {
      provide: HTTP_INTERCEPTORS,
      useClass: SessionTokenInterceptor,
      multi: true
    },
  ]
})
export class AppModule { }
