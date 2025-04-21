import { Component, HostBinding, input } from '@angular/core';
import { ColumnType } from '../models/column-type';

@Component({
  selector: 'app-task-manager-header',
  template: `
    <h3>
      <ng-content />
    </h3>
  `,
  styleUrls: ['./task-manager-header.component.scss'],
})
export class TaskManagerHeaderComponent {
  // Inputs
  columnType = input<ColumnType>('todo');

  // Getters
  @HostBinding('class')
  get hostClass(){
    return this.columnType();
  }
}
