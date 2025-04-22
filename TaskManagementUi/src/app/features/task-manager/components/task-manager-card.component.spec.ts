import { ComponentFixture, TestBed } from '@angular/core/testing';
import { TaskManagerCardComponent } from './task-manager-card.component';
import { Component, signal } from '@angular/core';
import { MatCardModule } from '@angular/material/card';
import { Card } from '../models/card.model';

// Host component
@Component({
  template: `
    <app-task-manager-card [card]="card()" data-testid="task-card">
    </app-task-manager-card>
  `,
  imports: [TaskManagerCardComponent],
})
class TestHostComponent {
  card = signal<Card>({
    cardId: '1',
    description: 'Test Task',
    responsible: 'Test User',
    creationDateUtc: new Date('2025-04-20T10:00:00Z'),
    status: 'Todo',
  });
}

describe('TaskManagerCardComponent', () => {
  let component: TestHostComponent;
  let fixture: ComponentFixture<TestHostComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [
        TestHostComponent,
        TaskManagerCardComponent,
        MatCardModule
      ]
    }).compileComponents();

    fixture = TestBed.createComponent(TestHostComponent);
    component = fixture.componentInstance;
  });

  it('should create', () => {
    fixture.detectChanges();
    expect(component).toBeTruthy();
  });

  it('should display card description', () => {
    // Act
    fixture.detectChanges();

    // Assert
    const cardElement = fixture.nativeElement.querySelector('[data-testid="task-card"]');
    expect(cardElement.textContent).toContain('Test Task');
  });

  it('should display card responsible', () => {
    // Act
    fixture.detectChanges();

    // Assert
    const cardElement = fixture.nativeElement.querySelector('[data-testid="task-card"]');
    expect(cardElement.textContent).toContain('Test User');
  });

  it('should update when card data changes', () => {
    // Arrange
    fixture.detectChanges();

    // Act - Update card data
    component.card.update(card => ({
      ...card,
      description: 'Updated Task',
      responsible: 'Updated User'
    }));
    fixture.detectChanges();

    // Assert
    const cardElement = fixture.nativeElement.querySelector('[data-testid="task-card"]');
    expect(cardElement.textContent).toContain('Updated Task');
    expect(cardElement.textContent).toContain('Updated User');
  });
});
