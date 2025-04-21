import { Component, computed, inject } from '@angular/core';
import { TaskManagerColumnComponent } from './components/task-manager-column.component';
import { TaskService } from './services/task.service';

@Component({
  selector: 'app-task-manager-page',
  imports: [TaskManagerColumnComponent],
  templateUrl: './task-manager.page.component.html',
  styleUrls: ['./task-manager.page.component.scss'],
})
export class TaskManagerPageComponent {
  private taskService = inject(TaskService);
  private tasks = this.taskService.getTasks();

  protected todoTasks = computed(() => this.tasks().filter((task) => task.status === 'Todo'));
  protected inProgressTasks = computed(() => this.tasks().filter((task) => task.status === 'InProgress'));
  protected doneTasks = computed(() => this.tasks().filter((task) => task.status === 'Done'));

  constructor() {
    console.log(this.inProgressTasks());
  }

}

