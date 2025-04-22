import { ComponentFixture, TestBed } from '@angular/core/testing';
import { TaskManagerHeaderComponent } from './task-manager-header.component';
import { Component } from '@angular/core';
import { CardStatus } from '../models/card-status';

// Host component
@Component({
  template: `
    <app-task-manager-header [columnType]="columnType" data-testid="header">
      {{ headerTitle }}
    </app-task-manager-header>
  `,
  imports: [TaskManagerHeaderComponent],
})
class TestHostComponent {
  columnType: CardStatus = 'Todo';
  headerTitle = 'Test Header';
}

describe('TaskManagerHeaderComponent', () => {
  let component: TestHostComponent;
  let fixture: ComponentFixture<TestHostComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [
        TestHostComponent,
        TaskManagerHeaderComponent
      ]
    }).compileComponents();

    fixture = TestBed.createComponent(TestHostComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should display the header title', () => {
    // Assert
    const headerElement = fixture.nativeElement.querySelector('[data-testid="header"] h3');
    expect(headerElement.textContent.trim()).toBe('Test Header');
  });

  it('should apply the correct class based on columnType', () => {
    // Assert - Default is Todo
    const headerElement = fixture.nativeElement.querySelector('[data-testid="header"]');
    expect(headerElement.classList.contains('Todo')).toBe(true);

    // Act - Change to InProgress
    component.columnType = 'InProgress';
    fixture.detectChanges();

    // Assert
    expect(headerElement.classList.contains('InProgress')).toBe(true);
    expect(headerElement.classList.contains('Todo')).toBe(false);

    // Act - Change to Done
    component.columnType = 'Done';
    fixture.detectChanges();

    // Assert
    expect(headerElement.classList.contains('Done')).toBe(true);
    expect(headerElement.classList.contains('InProgress')).toBe(false);
  });
});
