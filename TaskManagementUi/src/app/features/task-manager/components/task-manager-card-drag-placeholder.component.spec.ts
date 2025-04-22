import { ComponentFixture, TestBed } from '@angular/core/testing';
import { TaskManagerCardDragPlaceholderComponent } from './task-manager-card-drag-placeholder.component';
import { Component } from '@angular/core';

// Host component
@Component({
  template: `
    <app-task-manager-card-drag-placeholder data-testid="placeholder">
      Drag content here
    </app-task-manager-card-drag-placeholder>
  `,
  standalone: true,
  imports: [TaskManagerCardDragPlaceholderComponent]
})
class TestHostComponent {}

describe('TaskManagerCardDragPlaceholderComponent', () => {
  let component: TestHostComponent;
  let fixture: ComponentFixture<TestHostComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [
        TestHostComponent,
        TaskManagerCardDragPlaceholderComponent
      ]
    }).compileComponents();

    fixture = TestBed.createComponent(TestHostComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should project content', () => {
    // Assert
    const placeholderElement = fixture.nativeElement.querySelector('[data-testid="placeholder"]');
    expect(placeholderElement.textContent.trim()).toBe('Drag content here');
  });
});
