import { Component } from '@angular/core';
import { TaskManagerPageComponent } from '../features/task-manager/task-manager.page.component';

@Component({
  selector: 'app-root',
  imports: [TaskManagerPageComponent],
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss',
})
export class AppComponent {
  title = 'TaskManagementUI';
}
