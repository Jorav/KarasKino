import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { MovieService, TmdbMovieResult, SavedMovieResult } from '../../services/movie.service';

@Component({
  selector: 'app-add-movie',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './add-movie.component.html',
  styleUrls: ['./add-movie.component.scss']
})
export class AddMovieComponent {
  form: FormGroup;
  isLoadingTmdb = false;
  isLoadingDb = false;
  isSaving = false;
  saveError: string | null = null;
  tmdbError: string | null = null;
  dbResult: string | null = null;
  posterPreview: string | null = null;

  constructor(
    private fb: FormBuilder,
    private movieService: MovieService,
    private router: Router
  ) {
    this.form = this.fb.group({
      imdbLink: ['', Validators.required],
      title: ['', Validators.required],
      description: [''],
      posterUrl: [''],
      director: [''],
      releaseDate: [null],
      runtime: [null],
      genres: [[]],
      watchedByKara: [false],
      watchedByJohan: [false]
    });
  }

  extractImdbId(link: string): string | null {
    const match = link.match(/tt\d+/);
    return match ? match[0] : null;
  }

  lookupMovie(): void {
    this.posterPreview = null;
    this.tmdbError = null;
    this.dbResult = null;
    this.isLoadingTmdb = true;

    const link = this.form.get('imdbLink')?.value;
    const imdbId = this.extractImdbId(link);

    if (!imdbId) {
      this.tmdbError = 'Could not extract a valid IMDB ID from that link.';
      this.isLoadingTmdb = false;
      return;
    }

    this.movieService.findByImdbId(imdbId).subscribe({
      next: (movie: TmdbMovieResult) => {
        this.form.patchValue({
          title: movie.title,
          description: movie.description,
          posterUrl: movie.posterUrl,
          director: movie.director,
          releaseDate: movie.releaseDate,
          runtime: movie.runtime,
          genres: movie.genres
        });
        this.posterPreview = movie.posterUrl;
        this.isLoadingTmdb = false;
      },
      error: (err) => {
        this.isLoadingTmdb = false;
        if (err.status === 404) {
          this.tmdbError = 'Movie not found. Please check the IMDB link and try again.';
        } else if (err.status === 500) {
          this.tmdbError = 'Something went wrong on our end. Please try again later.';
        } else {
          this.tmdbError = 'Something went wrong. Please try again.';
        }
      }
    });
  }

  getFromDb(): void {
    this.dbResult = null;
    this.tmdbError = null;
    this.isLoadingDb = true;

    const link = this.form.get('imdbLink')?.value ?? '';
    const imdbId = this.extractImdbId(link);

    if (!imdbId) {
      this.dbResult = 'Could not extract a valid IMDB ID from that link.';
      this.isLoadingDb = false;
      return;
    }

    this.movieService.getByImdbId(imdbId).subscribe({
      next: (movie: SavedMovieResult) => {
        this.form.patchValue({
          title: movie.title,
          description: movie.description,
          posterUrl: movie.posterUrl,
          director: movie.director,
          releaseDate: movie.releaseYear,
          runtime: movie.runtime,
          genres: movie.genres,
          watchedByKara: movie.watchedByKara,
          watchedByJohan: movie.watchedByJohan
        });
        this.posterPreview = movie.posterUrl;
        this.dbResult = 'Loaded from database.';
        this.isLoadingDb = false;
      },
      error: (err) => {
        this.isLoadingDb = false;
        if (err.status === 404) {
          this.dbResult = 'No saved entry found in database.';
        } else {
          this.dbResult = 'Something went wrong fetching from database.';
        }
      }
    });
  }

  get genres(): string[] {
    return this.form.get('genres')?.value ?? [];
  }

  submit(): void {
    if (this.form.invalid || this.isSaving) return;

    const link = this.form.get('imdbLink')?.value ?? '';
    const imdbId = this.extractImdbId(link) ?? '';

    this.isSaving = true;
    this.saveError = null;

    this.movieService.addMovie({
      imdbId,
      title: this.form.get('title')?.value,
      description: this.form.get('description')?.value || null,
      posterUrl: this.form.get('posterUrl')?.value || null,
      director: this.form.get('director')?.value || null,
      releaseYear: this.form.get('releaseDate')?.value || null,
      runtime: this.form.get('runtime')?.value || null,
      genres: this.form.get('genres')?.value ?? [],
      watchedByKara: this.form.get('watchedByKara')?.value ?? false,
      watchedByJohan: this.form.get('watchedByJohan')?.value ?? false
    }).subscribe({
      next: () => this.router.navigate(['/movies']),
      error: () => {
        this.isSaving = false;
        this.saveError = 'Failed to save movie. Please try again.';
      }
    });
  }
}
