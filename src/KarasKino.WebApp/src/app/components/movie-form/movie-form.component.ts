import { Component, Input, Output, EventEmitter, OnChanges, SimpleChanges } from '@angular/core';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';

export interface MovieFormValue {
  title: string;
  description: string | null;
  posterUrl: string | null;
  director: string | null;
  releaseDate: string | null;
  runtime: number | null;
  genres: string[];
  watchedByKara: boolean;
  watchedByJohan: boolean;
}

@Component({
  selector: 'app-movie-form',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './movie-form.component.html',
  styleUrls: ['./movie-form.component.scss']
})
export class MovieFormComponent implements OnChanges {
  @Input() initialValue: Partial<MovieFormValue> | null = null;
  @Input() posterPreview: string | null = null;
  @Input() isSaving = false;
  @Input() saveError: string | null = null;
  @Input() saveButtonLabel = 'Save Movie';

  @Output() save = new EventEmitter<MovieFormValue>();
  @Output() cancel = new EventEmitter<void>();
  @Output() formReady = new EventEmitter<FormGroup>();

  form: FormGroup;

  constructor(private fb: FormBuilder) {
    this.form = this.fb.group({
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
    this.formReady.emit(this.form);
  }

  ngOnChanges(changes: SimpleChanges): void {
    if (changes['initialValue'] && this.initialValue) {
      this.form.patchValue(this.initialValue);
    }
  }

  get genres(): string[] {
    return this.form.get('genres')?.value ?? [];
  }

  submit(): void {
    if (this.form.invalid || this.isSaving) return;
    this.save.emit(this.form.value);
  }

  onCancel(): void {
    this.cancel.emit();
  }
}