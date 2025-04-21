import { Injectable, signal } from "@angular/core";
import { Card } from "../models/card.model";
import { CardStatus } from "../models/card-status";
import { delay, of, tap } from "rxjs";

@Injectable({
  providedIn: 'root',
})
export class TaskService {
  private tasks = signal<Card[]>([]);

  constructor() {
    this.tasks.set(this.getMockTasks());
  }

  getTasks() {
    return this.tasks.asReadonly();
  }

  private getMockTasks(): Card[] {
    return [
      {
        responsible: 'John 1',
        creationDateUtc: new Date(),
        status: 'Todo',
        cardId: '1',
        description: 'Task 1',
      },
      {
        responsible: 'John 4',
        creationDateUtc: new Date(),
        status: 'Todo',
        cardId: '4',
        description: 'Task 4',
      },
      {
        responsible: 'John 5',
        creationDateUtc: new Date(),
        status: 'Todo',
        cardId: '5',
        description: 'Task 5',
      },
      {
        responsible: 'John 6',
        creationDateUtc: new Date(),
        status: 'Todo',
        cardId: '6',
        description: 'Task 6',
      },
      {
        responsible: 'John 2',
        creationDateUtc: new Date(),
        status: 'InProgress',
        cardId: '2',
        description: 'Task 2',
      },
      {
        responsible: 'John 7',
        creationDateUtc: new Date(),
        status: 'InProgress',
        cardId: '7',
        description: 'Task 7',
      },
      {
        responsible: 'John 8',
        creationDateUtc: new Date(),
        status: 'InProgress',
        cardId: '8',
        description: 'Task 8',
      },
      {
        responsible: 'John 9',
        creationDateUtc: new Date(),
        status: 'InProgress',
        cardId: '9',
        description: 'Task 9',
      },
      {
        responsible: 'John 3',
        creationDateUtc: new Date(),
        status: 'Done',
        cardId: '3',
        description: 'Task 3',
      },
      {
        responsible: 'John 10',
        creationDateUtc: new Date(),
        status: 'Done',
        cardId: '10',
        description: 'Task 10',
      },
      {
        responsible: 'John 11',
        creationDateUtc: new Date(),
        status: 'Done',
        cardId: '11',
        description: 'Task 11',
      },
      {
        responsible: 'John 12',
        creationDateUtc: new Date(),
        status: 'Done',
        cardId: '12',
        description: 'Task 12',
      },
    ];
  }

  public updateTaskStatus(cardId: string, newStatus: CardStatus) {
    this.tasks.update((tasks) => tasks.map((task) => task.cardId === cardId ? { ...task, status: newStatus } : task));
  }

  addTodoCard(cardDescription: string, cardResponsible: string) {
    const card: Card = {
      cardId: (new Date()).getTime().toString(),
      description: cardDescription,
      responsible: cardResponsible,
      creationDateUtc: new Date(),
      status: 'InProgress',
    };

    return of(card).pipe(
      delay(3000),
      tap(() => {
        this.tasks.update((tasks) => [card, ...tasks]);
      })
    )
  }

}
