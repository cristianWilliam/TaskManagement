import { CardStatus } from "../../../models/card-status";

export interface CardStatusUpdatedEvent {
  cardId: string;
  oldStatus: CardStatus;
  newStatus: CardStatus;
}
