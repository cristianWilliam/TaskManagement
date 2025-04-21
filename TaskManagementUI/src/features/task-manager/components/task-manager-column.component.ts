import { Component, inject, input } from '@angular/core';
import { Card } from '../models/card.model';
import { TaskManagerHeaderComponent } from './task-manager-header.component';
import { TaskManagerCardComponent } from './task-manager-card.component';
import { CdkDrag, CdkDropList, CdkDragPlaceholder, CdkDragDrop, moveItemInArray, transferArrayItem } from '@angular/cdk/drag-drop';
import { TaskManagerCardDragPlaceholderComponent } from './task-manager-card-drag-placeholder.component';
import { CardStatus } from '../models/card-status';
import { CardsStore } from '../services/cards.store.service';
import { CardHttpService } from '../services/http/card.http.service';
import { catchError, take, throwError } from 'rxjs';
import { MatSnackBar } from '@angular/material/snack-bar';
@Component({
  selector: 'app-task-manager-column',
  imports: [
    TaskManagerHeaderComponent,
    TaskManagerCardComponent,
    CdkDropList,
    CdkDrag,
    TaskManagerCardDragPlaceholderComponent,
    CdkDragPlaceholder,
  ],
  templateUrl: './task-manager-column.component.html',
  styleUrls: ['./task-manager-column.component.scss'],
})
export class TaskManagerColumnComponent {
  // Dependencies
  private cardsStore = inject(CardsStore);
  private cardHttpService = inject(CardHttpService);
  private matSnackbar = inject(MatSnackBar);

  // Inputs
  columnTitle = input.required<string>();
  columnData = input<Card[]>([]);
  columnType = input<CardStatus>('Todo');

  // Helper methods
  protected onDrop(event: CdkDragDrop<Card[]>) {
    if (event.previousContainer === event.container) {
      moveItemInArray(
        event.container.data,
        event.previousIndex,
        event.currentIndex
      );
    } else {
      transferArrayItem(
        event.previousContainer.data,
        event.container.data,
        event.previousIndex,
        0 // Top of the list
      );

      const cardId: string = (event.item.data as Card).cardId;
      const newStatus: CardStatus = event.container.id as CardStatus;

      this.cardHttpService
        .updateCardStatus(cardId, newStatus)
        .pipe(
          take(1),
          catchError((err) => {
            this.showError('Error on Saving');

            // Undo Move card
            transferArrayItem(
              event.container.data,
              event.previousContainer.data,
              0, // Top of the list
              event.previousIndex
            );

            return throwError(() => err);
          })
        )
        .subscribe();
    }
  }

  protected noDonePredicate(item: CdkDrag<Card>) {
    return item.data.status !== 'Done';
  }

  private showError(errorMessage: string) {
    this.matSnackbar.open(errorMessage, 'Close', {
      horizontalPosition: 'end',
      verticalPosition: 'bottom',
    });
  }
}
