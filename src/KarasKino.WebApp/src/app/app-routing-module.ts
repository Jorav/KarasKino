import { Routes } from '@angular/router';
import { AddMovieComponent } from './pages/add-movie/add-movie.component';
import { EditMovieComponent } from './pages/edit-movie/edit-movie.component';
import { MoviesComponent } from './pages/movies/movies.component';
import { LoginComponent } from './pages/login/login.component';
import { authGuard, editorGuard } from './services/auth/auth.guard';

export const routes: Routes = [
  { path: 'add-movie', component: AddMovieComponent, canActivate: [editorGuard] },
  { path: 'movies/:imdbId/edit', component: EditMovieComponent, canActivate: [editorGuard] },
  { path: 'movies', component: MoviesComponent },
  { path: 'login', component: LoginComponent },
  { path: '', redirectTo: 'movies', pathMatch: 'full' },
  { path: '**', redirectTo: 'movies' }
];
