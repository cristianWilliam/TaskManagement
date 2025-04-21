import { CardStatus } from "../../models/card-status";

export interface MoveCardRequest {
  cardId: string,
  newStatus: CardStatus,
}
