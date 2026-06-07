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
}
