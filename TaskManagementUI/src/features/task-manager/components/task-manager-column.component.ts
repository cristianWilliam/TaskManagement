import { Component, input } from '@angular/core';
import { Card } from '../models/card.model';
import { TaskManagerHeaderComponent } from './task-manager-header.component';
import { ColumnType } from '../models/column-type';
import { TaskManagerCardComponent } from './task-manager-card.component';

@Component({
  selector: 'app-task-manager-column',
  imports: [TaskManagerHeaderComponent, TaskManagerCardComponent],
  templateUrl: './task-manager-column.component.html',
  styleUrls: ['./task-manager-column.component.scss'],
})
export class TaskManagerColumnComponent {
  columnTitle = input.required<string>();
  columnData = input<Card[]>([]);
  columnType = input<ColumnType>('todo');
}
