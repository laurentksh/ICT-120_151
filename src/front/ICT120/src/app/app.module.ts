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
import { EnvironmentService } from 'src/environments/service/environment.service';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { MatButtonModule } from '@angular/material/button';
import { MatProgressBarModule } from '@angular/material/progress-bar';
import { MAT_SNACK_BAR_DEFAULT_OPTIONS } from '@angular/material/snack-bar';
import { ContentNotFoundComponent } from './main-components/content-not-found/content-not-found.component';


@NgModule({
  declarations: [
    AppComponent,
    LeftSidebarComponent,
    RightSidebarComponent,
    ContentNotFoundComponent,
  ],
  imports: [
    AuthModule,
    UserModule,
    HomeModule,
    PublicationModule,
    HtmlAddonsModule,
    
    CommonModule,
    BrowserModule,
    AppRoutingModule,
    BrowserAnimationsModule,

    //Angular
    BrowserAnimationsModule, //Angular Browser Animations
    MatButtonModule,
    MatProgressBarModule,
  ],
  bootstrap: [AppComponent],
  providers: [
    {
      provide: HTTP_INTERCEPTORS,
      useClass: SessionTokenInterceptor,
      multi: true
    },
    {
      provide: MAT_SNACK_BAR_DEFAULT_OPTIONS,
      useValue: { duration: 3500 }
    }
  ]
})
export class AppModule { }
