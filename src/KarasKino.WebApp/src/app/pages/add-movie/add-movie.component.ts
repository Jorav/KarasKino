import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { Router, ActivatedRoute, RouterModule } from '@angular/router';
import { Subject } from 'rxjs';
import { debounceTime, distinctUntilChanged, switchMap, filter } from 'rxjs/operators';
import { MovieService, TmdbMovieResult, SavedMovieResult, MovieSearchResult } from '../../services/movie.service';
import { HostListener, ElementRef } from '@angular/core';

@Component({
  selector: 'app-add-movie',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, RouterModule],
  templateUrl: './add-movie.component.html',
  styleUrls: ['./add-movie.component.scss']
})
export class AddMovieComponent implements OnInit {
  form: FormGroup;
  isLoadingTmdb = false;
  isLoadingDb = false;
  isSaving = false;
  saveError: string | null = null;
  tmdbError: string | null = null;
  dbResult: string | null = null;
  posterPreview: string | null = null;

  searchQuery = '';
  searchResults: MovieSearchResult[] = [];
  isSearching = false;
  showDropdown = false;

  private searchSubject = new Subject<string>();

  constructor(
    private fb: FormBuilder,
    private movieService: MovieService,
    private router: Router,
    private route: ActivatedRoute,
    private elementRef: ElementRef
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

  ngOnInit(): void {
    this.route.queryParams.subscribe(params => {
      const imdbId = params['imdbId'];
      if (imdbId) {
        this.loadMovieFromDb(imdbId, false);
      }
    });

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

  onSearchInput(value: string): void {
    this.searchQuery = value;
    this.tmdbError = null;
    this.dbResult = null;

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

  extractImdbId(link: string): string | null {
    const match = link.match(/tt\d+/);
    return match ? match[0] : null;
  }

  lookupByImdbId(imdbId: string): void {
    this.posterPreview = null;
    this.tmdbError = null;
    this.dbResult = null;
    this.isLoadingTmdb = true;

    this.movieService.findByImdbId(imdbId).subscribe({
      next: (movie: TmdbMovieResult) => {
        this.form.patchValue({
          imdbLink: `https://www.imdb.com/title/${imdbId}/`,
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

    const link = this.form.get('imdbLink')?.value ?? '';
    const imdbId = this.extractImdbId(link);

    if (!imdbId) {
      this.dbResult = 'Could not extract a valid IMDB ID from that link.';
      return;
    }

    this.loadMovieFromDb(imdbId, true);
  }

  loadMovieFromDb(imdbId: string, isManual: boolean): void {
    this.isLoadingDb = true;
    this.dbResult = null;
    this.tmdbError = null;

    this.movieService.getByImdbId(imdbId).subscribe({
      next: (movie: SavedMovieResult) => {
        this.form.patchValue({
          imdbLink: `https://www.imdb.com/title/${movie.imdbId}/`,
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
        this.searchQuery = movie.title;
        this.isLoadingDb = false;
        if (isManual) {
          this.dbResult = 'Loaded from database.';
        }
      },
      error: (err) => {
        this.isLoadingDb = false;
        if (isManual) {
          if (err.status === 404) {
            this.dbResult = 'No saved entry found in database.';
          } else {
            this.dbResult = 'Something went wrong fetching from database.';
          }
        } else {
          this.dbResult = 'Failed to load movie details.';
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

  cancel(): void {
    this.router.navigate(['/movies']);
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
}