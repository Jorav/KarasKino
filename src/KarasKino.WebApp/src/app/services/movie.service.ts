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
export interface MovieListItem {
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

export interface MovieSearchResult {
  imdbId: string;
  title: string;
  director: string | null;
  releaseYear: string | null;
  posterUrl: string | null;
}

export interface SearchTmdbMoviesResult {
  results: MovieSearchResult[];
}

export interface PagedMoviesResult {
  items: MovieListItem[];
  totalCount: number;
  page: number;
  pageSize: number;
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
  searchTmdb(query: string): Observable<SearchTmdbMoviesResult> {
    return this.http.get<SearchTmdbMoviesResult>(`${this.apiUrl}/movies/search-tmdb`, {
      params: { query }
    });
  }
  getMovies(page: number, pageSize: number, search?: string): Observable<PagedMoviesResult> {
    let params: any = { page, pageSize };
    if (search) params['search'] = search;
    return this.http.get<PagedMoviesResult>(`${this.apiUrl}/movies`, { params });
  }
  deleteMovie(imdbId: string): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/movies/${imdbId}`);
  }
}
