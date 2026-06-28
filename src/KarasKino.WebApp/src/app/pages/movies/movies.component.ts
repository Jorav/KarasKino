import { Component, OnInit, OnDestroy, HostListener } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router, RouterModule } from '@angular/router';
import { Subject } from 'rxjs';
import { debounceTime, distinctUntilChanged, takeUntil } from 'rxjs/operators';
import { MovieService, MovieListItem } from '../../services/movie.service';
import { inject } from '@angular/core';
import { AuthService } from '../../services/auth/auth.service';

@Component({
  selector: 'app-movies',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterModule],
  templateUrl: './movies.component.html',
  styleUrls: ['./movies.component.scss']
})
export class MoviesComponent implements OnInit, OnDestroy {
  auth = inject(AuthService);
  movies: MovieListItem[] = [];
  search = '';
  page = 1;
  pageSize = 24;
  totalCount = 0;
  isLoading = false;
  hasMore = true;

  activeCardImdbId: string | null = null;
  confirmingDeleteImdbId: string | null = null;
  isDeleting = false;

  private searchSubject = new Subject<string>();
  private destroy$ = new Subject<void>();

  constructor(
    private movieService: MovieService,
    private router: Router
  ) { }

  ngOnInit(): void {
    this.searchSubject.pipe(
      debounceTime(300),
      distinctUntilChanged(),
      takeUntil(this.destroy$)
    ).subscribe(() => this.resetAndLoad());

    this.loadMovies();
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }

  onSearchChange(): void {
    this.searchSubject.next(this.search);
  }

  resetAndLoad(): void {
    this.movies = [];
    this.page = 1;
    this.hasMore = true;
    this.loadMovies();
  }

  loadMovies(): void {
    if (this.isLoading || !this.hasMore) return;

    this.isLoading = true;
    this.movieService.getMovies(this.page, this.pageSize, this.search || undefined).subscribe({
      next: (result) => {
        this.movies = [...this.movies, ...result.items];
        this.totalCount = result.totalCount;
        this.hasMore = this.movies.length < result.totalCount;
        this.page++;
        this.isLoading = false;
      },
      error: () => {
        this.isLoading = false;
      }
    });
  }

  @HostListener('window:scroll')
  onScroll(): void {
    const threshold = 300;
    const position = window.innerHeight + window.scrollY;
    const height = document.documentElement.scrollHeight;
    if (position >= height - threshold) {
      this.loadMovies();
    }
  }

  formatRuntime(minutes: number | null): string {
    if (!minutes) return '';
    const h = Math.floor(minutes / 60);
    const m = minutes % 60;
    return h > 0 ? `${h}h ${m}m` : `${m}m`;
  }

  toggleCardActions(imdbId: string, event: Event): void {
    event.stopPropagation();
    this.activeCardImdbId = this.activeCardImdbId === imdbId ? null : imdbId;
    this.confirmingDeleteImdbId = null;
  }

  editMovie(imdbId: string, event: Event): void {
    event.stopPropagation();
    this.router.navigate(['/edit-movie', imdbId]);  }

  requestDelete(imdbId: string, event: Event): void {
    event.stopPropagation();
    this.confirmingDeleteImdbId = imdbId;
  }

  cancelDelete(event: Event): void {
    event.stopPropagation();
    this.confirmingDeleteImdbId = null;
  }

  confirmDelete(imdbId: string, event: Event): void {
    event.stopPropagation();
    this.isDeleting = true;
    this.movieService.deleteMovie(imdbId).subscribe({
      next: () => {
        this.movies = this.movies.filter(m => m.imdbId !== imdbId);
        this.totalCount--;
        this.activeCardImdbId = null;
        this.confirmingDeleteImdbId = null;
        this.isDeleting = false;
      },
      error: () => {
        this.isDeleting = false;
      }
    });
  }

  @HostListener('document:click')
  onDocumentClick(): void {
    this.activeCardImdbId = null;
    this.confirmingDeleteImdbId = null;
  }
}
