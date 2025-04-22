import { ComponentFixture, TestBed } from '@angular/core/testing';
import { TaskManagerFormComponent } from './task-manager-form.component';
import { Component, signal } from '@angular/core';
import { MatCardModule } from '@angular/material/card';
import { FormsModule } from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { NoopAnimationsModule } from '@angular/platform-browser/animations';
import { CardForm } from '../models/card-form';

// Host component
@Component({
  template: `
    <app-task-manager-form
      [isLoading]="isLoading()"
      (onAddCard)="handleAddCard($event)"
      data-testid="task-form"
    >
    </app-task-manager-form>
  `,
  imports: [TaskManagerFormComponent],
})
class TestHostComponent {
  isLoading = signal<boolean>(false);
  lastAddedCard: CardForm | null = null;

  handleAddCard(card: CardForm) {
    this.lastAddedCard = card;
  }

  setLoading(loading: boolean) {
    this.isLoading.set(loading);
  }
}

describe('TaskManagerFormComponent', () => {
  let component: TestHostComponent;
  let fixture: ComponentFixture<TestHostComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [
        TestHostComponent,
        TaskManagerFormComponent,
        MatCardModule,
        FormsModule,
        MatFormFieldModule,
        MatInputModule,
        MatButtonModule,
        MatProgressSpinnerModule,
        NoopAnimationsModule
      ]
    }).compileComponents();

    fixture = TestBed.createComponent(TestHostComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should display form fields', () => {
    // Assert
    const descriptionField = fixture.nativeElement.querySelector('[data-testid="task-form"] input[name="taskDescription"]');
    const responsibleField = fixture.nativeElement.querySelector('[data-testid="task-form"] input[name="taskResponsible"]');
    const submitButton = fixture.nativeElement.querySelector('[data-testid="task-form"] button[type="submit"]');

    expect(descriptionField).toBeTruthy();
    expect(responsibleField).toBeTruthy();
    expect(submitButton).toBeTruthy();
  });

  it('should emit onAddCard event when form is submitted with valid data', () => {
    // Arrange
    const descriptionField = fixture.nativeElement.querySelector('[data-testid="task-form"] input[name="taskDescription"]');
    const responsibleField = fixture.nativeElement.querySelector('[data-testid="task-form"] input[name="taskResponsible"]');
    const form = fixture.nativeElement.querySelector('[data-testid="task-form"] form');

    // Act - Fill form
    descriptionField.value = 'New Task';
    descriptionField.dispatchEvent(new Event('input'));

    responsibleField.value = 'New User';
    responsibleField.dispatchEvent(new Event('input'));

    // Submit form
    form.dispatchEvent(new Event('submit'));
    fixture.detectChanges();

    // Assert
    expect(component.lastAddedCard).toEqual({
      taskDescription: 'New Task',
      taskResponsible: 'New User'
    });
  });

  it('should not emit onAddCard event when form is invalid', () => {
    // Arrange
    const form = fixture.nativeElement.querySelector('[data-testid="task-form"] form');
    component.lastAddedCard = null;

    // Act - Submit form without filling required fields
    form.dispatchEvent(new Event('submit'));
    fixture.detectChanges();

    // Assert
    expect(component.lastAddedCard).toBeNull();
  });

  it('should show spinner when isLoading is true', () => {
    // Arrange
    component.setLoading(false);
    fixture.detectChanges();

    // Assert - No spinner initially
    let spinner = fixture.nativeElement.querySelector('[data-testid="task-form"] mat-progress-spinner');
    expect(spinner).toBeFalsy();

    // Act - Set loading to true
    component.setLoading(true);
    fixture.detectChanges();

    // Assert - Spinner should be visible
    spinner = fixture.nativeElement.querySelector('[data-testid="task-form"] mat-progress-spinner');
    expect(spinner).toBeTruthy();
  });
});
