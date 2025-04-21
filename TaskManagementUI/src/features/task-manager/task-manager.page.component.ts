import { Component, computed, inject, signal } from '@angular/core';
import { TaskManagerColumnComponent } from './components/task-manager-column.component';
import { TaskService } from './services/task.service';
import { CdkDropListGroup } from '@angular/cdk/drag-drop';
import { TaskManagerFormComponent } from './components/task-manager-form.component';
import { MatDividerModule } from '@angular/material/divider';
import { CardForm } from './models/card-form';
import { finalize, take } from 'rxjs';

@Component({
  selector: 'app-task-manager-page',
  imports: [TaskManagerColumnComponent, CdkDropListGroup, TaskManagerFormComponent, MatDividerModule],
  templateUrl: './task-manager.page.component.html',
  styleUrls: ['./task-manager.page.component.scss'],
})
export class TaskManagerPageComponent {
  private taskService = inject(TaskService);
  private tasks = this.taskService.getTasks();

  protected todoTasks = computed(() => this.tasks().filter((task) => task.status === 'Todo'));
  protected inProgressTasks = computed(() => this.tasks().filter((task) => task.status === 'InProgress'));
  protected doneTasks = computed(() => this.tasks().filter((task) => task.status === 'Done'));

  protected isLoading = signal(false);

  protected addCard(card: CardForm){
    this.isLoading.set(true);
    this.taskService.addTodoCard(card.taskDescription, card.taskResponsible)
      .pipe(
        take(1),
        finalize(() => this.isLoading.set(false))
      )
      .subscribe();
  }
}

