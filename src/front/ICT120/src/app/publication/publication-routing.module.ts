import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Routes } from '@angular/router';
import { PublicationComponent } from './components/publication/publication.component';

const routes: Routes = [
  {
    path: 'publication/:id',
    component: PublicationComponent
  },
];

@NgModule({
  declarations: [],
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class PublicationRoutingModule { }
