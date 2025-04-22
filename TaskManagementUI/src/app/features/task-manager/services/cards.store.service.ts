import { Injectable, signal } from "@angular/core";
import { Card } from "../models/card.model";
import { CardStatus } from "../models/card-status";

@Injectable({
  providedIn: 'root',
})
export class CardsStore {
  private tasks = signal<Card[]>([]);

  public getTasks() {
    return this.tasks.asReadonly();
  }

  public initializeTasks(allCards: Card[]){
    this.tasks.set(allCards);
  }

  public updateCardStatus(cardId: string, newStatus: CardStatus) {
    this.tasks.update((tasks) =>
      tasks.map((task) =>
        task.cardId === cardId ? { ...task, status: newStatus } : task
      )
    );
  }

  public addCard(newCard: Card){
    // Check if card already exists
    const existingCard = this.tasks().find(card => card.cardId === newCard.cardId);
    if (existingCard) {
      return;
    }

    this.tasks.update(cards => [newCard, ...cards]);
  }
}
