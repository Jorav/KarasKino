import { Component, OnInit, HostListener, ElementRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router, RouterModule } from '@angular/router';
import { Subject } from 'rxjs';
import { debounceTime, distinctUntilChanged, switchMap, filter } from 'rxjs/operators';
import { MovieService, TmdbMovieResult, MovieSearchResult } from '../../services/movie.service';
import { MovieFormComponent, MovieFormValue } from '../../components/movie-form/movie-form.component';

@Component({
  selector: 'app-add-movie',
  standalone: true,
  imports: [CommonModule, RouterModule, MovieFormComponent],
  templateUrl: './add-movie.component.html',
  styleUrls: ['./add-movie.component.scss']
})
export class AddMovieComponent implements OnInit {
  isLoadingTmdb = false;
  isSaving = false;
  saveError: string | null = null;
  tmdbError: string | null = null;
  posterPreview: string | null = null;

  searchQuery = '';
  searchResults: MovieSearchResult[] = [];
  isSearching = false;
  showDropdown = false;

  selectedImdbId: string | null = null;
  formInitialValue: Partial<MovieFormValue> | null = null;

  private searchSubject = new Subject<string>();

  constructor(
    private movieService: MovieService,
    private router: Router,
    private elementRef: ElementRef
  ) { }

  ngOnInit(): void {
    this.searchSubject.pipe(
      debounceTime(300),
      distinctUntilChanged(),
      filter(q => q.trim().length > 2),
      switchMap(q => {
        this.isSearching = true;
        return this.movieService.searchTmdb(q);
      })
    ).subscribe({
      next: (res) => {
        this.searchResults = res.results;
        this.showDropdown = this.searchResults.length > 0;
        this.isSearching = false;
      },
      error: () => {
        this.isSearching = false;
        this.searchResults = [];
        this.showDropdown = false;
      }
    });
  }

  @HostListener('document:click', ['$event'])
  onDocumentClick(event: MouseEvent): void {
    if (!this.elementRef.nativeElement.contains(event.target)) {
      this.showDropdown = false;
    }
  }

  @HostListener('document:keydown.escape')
  onEscapeKey(): void {
    this.showDropdown = false;
  }

  extractImdbId(link: string): string | null {
    const match = link.match(/tt\d+/);
    return match ? match[0] : null;
  }

  onSearchInput(value: string): void {
    this.searchQuery = value;
    this.tmdbError = null;

    const imdbId = this.extractImdbId(value);
    if (imdbId) {
      this.showDropdown = false;
      this.searchResults = [];
      this.lookupByImdbId(imdbId);
      return;
    }

    if (value.trim().length <= 2) {
      this.showDropdown = false;
      this.searchResults = [];
      return;
    }

    this.searchSubject.next(value);
  }

  selectSearchResult(result: MovieSearchResult): void {
    this.showDropdown = false;
    this.searchQuery = result.title;
    this.lookupByImdbId(result.imdbId);
  }

  lookupByImdbId(imdbId: string): void {
    this.posterPreview = null;
    this.tmdbError = null;
    this.isLoadingTmdb = true;

    this.movieService.findByImdbId(imdbId).subscribe({
      next: (movie: TmdbMovieResult) => {
        this.selectedImdbId = imdbId;
        this.formInitialValue = {
          title: movie.title,
          description: movie.description,
          posterUrl: movie.posterUrl,
          director: movie.director,
          releaseDate: movie.releaseDate,
          runtime: movie.runtime,
          genres: movie.genres
        };
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

  onSave(value: MovieFormValue): void {
    if (!this.selectedImdbId) {
      this.saveError = 'Please find a movie first.';
      return;
    }

    this.isSaving = true;
    this.saveError = null;

    this.movieService.addMovie({
      imdbId: this.selectedImdbId,
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
        this.saveError = 'Failed to save movie. Please try again.';
      }
    });
  }

  onCancel(): void {
    this.router.navigate(['/movies']);
  }
}
