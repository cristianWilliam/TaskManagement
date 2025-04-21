import { Component, HostBinding, input } from '@angular/core';
import { CardStatus } from '../models/card-status';

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
  columnType = input<CardStatus>('Todo');

  // Getters
  @HostBinding('class')
  get hostClass(){
    return this.columnType();
  }
}
