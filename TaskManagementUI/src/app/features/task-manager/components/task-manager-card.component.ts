import { Component, input } from '@angular/core';
import { MatCardModule } from '@angular/material/card';
import { Card } from '../models/card.model';

@Component({
  selector: 'app-task-manager-card',
  imports: [MatCardModule],
  templateUrl: './task-manager-card.component.html',
  styleUrls: ['./task-manager-card.component.scss'],
})
export class TaskManagerCardComponent {
  card = input.required<Card>();
}
