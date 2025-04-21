import { Component, computed, inject, OnInit, signal } from '@angular/core';
import { TaskManagerColumnComponent } from './components/task-manager-column.component';
import { CardsStore } from './services/cards.store.service';
import { CdkDropListGroup } from '@angular/cdk/drag-drop';
import { TaskManagerFormComponent } from './components/task-manager-form.component';
import { MatDividerModule } from '@angular/material/divider';
import { catchError, finalize, of, take, throwError } from 'rxjs';
import { CardHttpService } from './services/http/card.http.service';
import { MatSnackBar } from '@angular/material/snack-bar';
import { CardForm } from './models/card-form';

@Component({
  selector: 'app-task-manager-page',
  imports: [
    TaskManagerColumnComponent,
    CdkDropListGroup,
    TaskManagerFormComponent,
    MatDividerModule,
  ],
  templateUrl: './task-manager.page.component.html',
  styleUrls: ['./task-manager.page.component.scss'],
})
export class TaskManagerPageComponent implements OnInit {
  // Dependencies
  private cardsStore = inject(CardsStore);
  private cardHttpService = inject(CardHttpService);
  private snackBar = inject(MatSnackBar);

  // Internal Members
  private tasks = this.cardsStore.getTasks();

  protected todoTasks = computed(() =>
    this.tasks().filter((task) => task.status === 'Todo')
  );
  protected inProgressTasks = computed(() =>
    this.tasks().filter((task) => task.status === 'InProgress')
  );
  protected doneTasks = computed(() =>
    this.tasks().filter((task) => task.status === 'Done')
  );

  protected isLoading = signal(false);

  // Hooks
  ngOnInit(): void {
    this.getAllCardsApi();
  }

  // Helper Methods
  private getAllCardsApi() {
    this.cardHttpService
      .getAllCards()
      .pipe(
        take(1),
        catchError((error) => {
          this.showError('Failed to Get Cards');

          return of([]);
        })
      )
      .subscribe((cards) => {
        this.cardsStore.initializeTasks(cards);
      });
  }

  protected addCard(card: CardForm) {
    this.isLoading.set(true);
    this.cardHttpService
      .addTodoCard(card.taskDescription, card.taskResponsible)
      .pipe(
        take(1),
        catchError((err) => {
          this.showError('Failed to add Card');
          return throwError(() => err);
        }),
        finalize(() => this.isLoading.set(false))
      )
      .subscribe((newCard) => {
        this.cardsStore.addCard(newCard);
      });
  }

  private showError(errorMessage: string) {
    this.snackBar.open(errorMessage, 'Close', {
      horizontalPosition: 'end',
      verticalPosition: 'bottom',
    });
  }
}
