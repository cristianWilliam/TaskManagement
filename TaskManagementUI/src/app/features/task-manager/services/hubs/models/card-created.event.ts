import { CardStatus } from "../../../models/card-status";

export interface CardCreatedEvent {
  cardId: string;
  creationDateUtc: Date,
  description: string,
  responsible: string,
  status: CardStatus
}
