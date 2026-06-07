import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';

export interface TmdbMovieResult {
  title: string;
  description: string | null;
  posterUrl: string | null;
  director: string | null;
  releaseDate: string | null;
  runtime: number | null;
  imdbId: string | null;
  genres: string[];
}

export interface AddMovieRequest {
  imdbId: string;
  title: string;
  description: string | null;
  posterUrl: string | null;
  director: string | null;
  releaseYear: string | null;
  runtime: number | null;
  genres: string[];
  watchedByKara: boolean;
  watchedByJohan: boolean;
}

export interface SavedMovieResult {
  id: string;
  title: string;
  imdbId: string;
  description: string | null;
  posterUrl: string | null;
  director: string | null;
  releaseYear: string | null;
  runtime: number | null;
  genres: string[];
  watchedByKara: boolean;
  watchedByJohan: boolean;
}

@Injectable({
  providedIn: 'root'
})
export class MovieService {
  private readonly apiUrl = environment.apiBaseUrl;

  constructor(private http: HttpClient) { }

  findByImdbId(imdbId: string): Observable<TmdbMovieResult> {
    return this.http.get<TmdbMovieResult>(`${this.apiUrl}/movies/search`, {
      params: { imdbId }
    });
  }
  addMovie(request: AddMovieRequest): Observable<string> {
    return this.http.post<string>(`${this.apiUrl}/movies`, request);
  }
  getByImdbId(imdbId: string): Observable<SavedMovieResult> {
    return this.http.get<SavedMovieResult>(`${this.apiUrl}/movies/${imdbId}`);
  }
}
