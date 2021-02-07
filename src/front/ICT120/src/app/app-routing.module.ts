import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { ContentNotFoundComponent } from './main-components/content-not-found/content-not-found.component';

const routes: Routes = [
  {
    path: '',
    redirectTo: '/home',
    pathMatch: 'full'
  },
  {
    path: 'content-not-found',
    component: ContentNotFoundComponent
  },
  {
    path: '**',
    redirectTo: "/content-not-found"
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes, { relativeLinkResolution: 'legacy' })],
  exports: [RouterModule]
})
export class AppRoutingModule { }
