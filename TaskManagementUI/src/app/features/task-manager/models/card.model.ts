import { CardStatus } from './card-status';

export interface Card {
  cardId: string;
  description: string;
  responsible: string;
  creationDateUtc: Date;
  status: CardStatus;
}
