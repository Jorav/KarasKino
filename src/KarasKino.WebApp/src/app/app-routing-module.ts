import { Routes } from '@angular/router';
import { AddMovieComponent } from './pages/add-movie/add-movie.component';

export const routes: Routes = [
  { path: 'add-movie', component: AddMovieComponent },
  { path: '', redirectTo: 'add-movie', pathMatch: 'full' },
  { path: '**', redirectTo: 'add-movie' }
];
