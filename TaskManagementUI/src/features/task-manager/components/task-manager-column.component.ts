import { Component, inject, input } from '@angular/core';
import { Card } from '../models/card.model';
import { TaskManagerHeaderComponent } from './task-manager-header.component';
import { TaskManagerCardComponent } from './task-manager-card.component';
import { CdkDrag, CdkDropList, CdkDragPlaceholder, CdkDragDrop, moveItemInArray, transferArrayItem } from '@angular/cdk/drag-drop';
import { TaskManagerCardDragPlaceholderComponent } from './task-manager-card-drag-placeholder.component';
import { CardStatus } from '../models/card-status';
import { TaskService } from '../services/task.service';
@Component({
  selector: 'app-task-manager-column',
  imports: [TaskManagerHeaderComponent, TaskManagerCardComponent, CdkDropList, CdkDrag, TaskManagerCardDragPlaceholderComponent, CdkDragPlaceholder],
  templateUrl: './task-manager-column.component.html',
  styleUrls: ['./task-manager-column.component.scss'],
})
export class TaskManagerColumnComponent {
  columnTitle = input.required<string>();
  columnData = input<Card[]>([]);
  columnType = input<CardStatus>('Todo');

  private taskService = inject(TaskService);

  protected onDrop(event: CdkDragDrop<Card[]>) {
    if (event.previousContainer === event.container) {
      moveItemInArray(event.container.data, event.previousIndex, event.currentIndex);
    } else {
      transferArrayItem(
        event.previousContainer.data,
        event.container.data,
        event.previousIndex,
        0, // Top of the list
      );

      this.taskService.updateTaskStatus(event.item.data.cardId, event.container.id as CardStatus);
    }
  }

  protected noDonePredicate(item: CdkDrag<Card>) {
    return item.data.status !== 'Done';
  }
}
