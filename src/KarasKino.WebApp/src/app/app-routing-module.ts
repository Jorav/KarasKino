import { Routes } from '@angular/router';
import { AddMovieComponent } from './pages/add-movie/add-movie.component';
import { MoviesComponent } from './pages/movies/movies.component';

export const routes: Routes = [
  { path: 'add-movie', component: AddMovieComponent },
  { path: 'movies', component: MoviesComponent },
  { path: '', redirectTo: 'movies', pathMatch: 'full' },
  { path: '**', redirectTo: 'movies' }
];
