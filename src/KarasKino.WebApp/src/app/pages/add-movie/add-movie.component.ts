import { Component } from '@angular/core';
import { FormArray, FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { MovieService, TmdbMovieResult } from '../../services/movie.service';

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
  tmdbError: string | null = null;
  posterPreview: string | null = null;

  constructor(private fb: FormBuilder, private movieService: MovieService) {
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
      watchedByJohan: [false],
      achievements: this.fb.array([])
    });
  }

  get achievements(): FormArray {
    return this.form.get('achievements') as FormArray;
  }

  extractImdbId(link: string): string | null {
    const match = link.match(/tt\d+/);
    return match ? match[0] : null;
  }

  lookupMovie(): void {
    this.posterPreview = null;
    this.isLoadingTmdb = true;
    this.tmdbError = null;

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

  get genres(): string[] {
    return this.form.get('genres')?.value ?? [];
  }

  addAchievement(): void {
    this.achievements.push(this.fb.group({
      description: ['', Validators.required]
    }));
  }

  removeAchievement(index: number): void {
    this.achievements.removeAt(index);
  }

  submit(): void {
    if (this.form.invalid) return;
    console.log(this.form.value);
  }
}
