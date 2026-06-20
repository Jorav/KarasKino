import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router, ActivatedRoute, RouterModule } from '@angular/router';
import { MovieService, SavedMovieResult } from '../../services/movie.service';
import { MovieFormComponent, MovieFormValue } from '../../components/movie-form/movie-form.component';

@Component({
  selector: 'app-edit-movie',
  standalone: true,
  imports: [CommonModule, RouterModule, MovieFormComponent],
  templateUrl: './edit-movie.component.html',
  styleUrls: ['./edit-movie.component.scss']
})
export class EditMovieComponent implements OnInit {
  isLoading = true;
  isSaving = false;
  saveError: string | null = null;
  loadError: string | null = null;
  posterPreview: string | null = null;

  imdbId = '';
  formInitialValue: Partial<MovieFormValue> | null = null;

  constructor(
    private movieService: MovieService,
    private router: Router,
    private route: ActivatedRoute
  ) { }

  ngOnInit(): void {
    this.imdbId = this.route.snapshot.paramMap.get('imdbId') ?? '';
    if (!this.imdbId) {
      this.loadError = 'No movie specified.';
      this.isLoading = false;
      return;
    }

    this.movieService.getByImdbId(this.imdbId).subscribe({
      next: (movie: SavedMovieResult) => {
        this.formInitialValue = {
          title: movie.title,
          description: movie.description,
          posterUrl: movie.posterUrl,
          director: movie.director,
          releaseDate: movie.releaseYear,
          runtime: movie.runtime,
          genres: movie.genres,
          watchedByKara: movie.watchedByKara,
          watchedByJohan: movie.watchedByJohan
        };
        this.posterPreview = movie.posterUrl;
        this.isLoading = false;
      },
      error: () => {
        this.loadError = 'Could not load this movie.';
        this.isLoading = false;
      }
    });
  }

  onSave(value: MovieFormValue): void {
    this.isSaving = true;
    this.saveError = null;

    this.movieService.addMovie({
      imdbId: this.imdbId,
      title: value.title,
      description: value.description,
      posterUrl: value.posterUrl,
      director: value.director,
      releaseYear: value.releaseDate,
      runtime: value.runtime,
      genres: value.genres,
      watchedByKara: value.watchedByKara,
      watchedByJohan: value.watchedByJohan
    }).subscribe({
      next: () => this.router.navigate(['/movies']),
      error: () => {
        this.isSaving = false;
        this.saveError = 'Failed to save changes. Please try again.';
      }
    });
  }

  onCancel(): void {
    this.router.navigate(['/movies']);
  }
}