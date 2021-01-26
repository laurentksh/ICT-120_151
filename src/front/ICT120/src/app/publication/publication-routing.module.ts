import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Routes } from '@angular/router';
import { NewPublicationComponent } from './components/new-publication/new-publication.component';
import { AuthenticationGuard } from '../guards/authentication.guard';
import { ViewPublicationComponent } from './components/view-publication/view-publication.component';

const routes: Routes = [
  {
    path: 'publication/id/:id',
    component: ViewPublicationComponent
  },
  {
    path: 'publication/new',
    component: NewPublicationComponent,
    canActivate: [AuthenticationGuard]
  }
];

@NgModule({
  declarations: [],
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class PublicationRoutingModule { }
