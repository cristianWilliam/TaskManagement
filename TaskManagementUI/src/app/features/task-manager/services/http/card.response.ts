import { CardStatus } from "../../models/card-status";


export interface CardResponse {
  cardId: string;
  description: string;
  responsible: string;
  creationDateUtc: Date;
  status: CardStatus;
}
