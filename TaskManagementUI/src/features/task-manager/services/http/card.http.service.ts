import { HttpClient } from "@angular/common/http";
import { inject, Injectable } from "@angular/core";
import { CardResponse } from "./card.response";
import { environment } from "../../../../environments/environment";
import { AddCardRequest } from "./add-card.request";
import { CardStatus } from "../../models/card-status";
import { MoveCardRequest } from "./move-card.request";

@Injectable({
  providedIn: 'root'
})
export class CardHttpService {
  private http = inject(HttpClient);
  private apiUrl = `${environment.apiBaseUrl}/cards`

  getAllCards(){
    return this.http.get<CardResponse[]>(this.apiUrl);
  }

  addTodoCard(cardDescription: string, cardResponsible: string){
    const req: AddCardRequest = {
      description: cardDescription,
      responsible: cardResponsible
    }

    return this.http.post<CardResponse>(this.apiUrl, req);
  }

  updateCardStatus(cardId: string, newStatus: CardStatus){
    const cardRequest: MoveCardRequest = {
      cardId: cardId,
      newStatus: newStatus
    };

    return this.http.patch<CardResponse>(`${this.apiUrl}/${cardId}`, cardRequest);
  }
}


