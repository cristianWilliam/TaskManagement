import { inject, Injectable, OnDestroy } from "@angular/core";
import * as signalR from '@microsoft/signalr';
import { environment } from "../../../../../environments/environment";
import { CardsStore } from "../cards.store.service";
import { CardStatusUpdatedEvent } from "./models/card-status-updated.event";
import { Card } from "../../models/card.model";
import { CardCreatedEvent } from "./models/card-created.event";

@Injectable()
export class CardHubService implements OnDestroy {
  private hubConnection: signalR.HubConnection;
  private isHubConnectionStarted = false;
  private cardStore = inject(CardsStore);

  constructor() {
    this.hubConnection = new signalR.HubConnectionBuilder()
      .withUrl(`${environment.apiBaseUrl}/cards-hub`, {
        skipNegotiation: true,
        transport: signalR.HttpTransportType.WebSockets
      })
      .withAutomaticReconnect()
      .build();
  }

  ngOnDestroy(): void {
    this.closeHubConnection();
  }

  public startConnection(){
    if (this.isHubConnectionStarted) return;

    this.hubConnection.start()
      .then(() => {
        console.log('Connection started');
        this.isHubConnectionStarted = true;
        this.registerHubEvents();
      })
      .catch((err) => {
        console.error('Error while starting connection: ', err);
        this.isHubConnectionStarted = false;
      });

  }

  private registerHubEvents(){
    if (!this.isHubConnectionStarted) return;

    this.hubConnection.on('CardStatusUpdated', (data: CardStatusUpdatedEvent) => {
      this.cardStore.updateCardStatus(data.cardId, data.newStatus);
    });

    this.hubConnection.on('CardCreated', (data: CardCreatedEvent) => {
      var newCard: Card = {
        cardId: data.cardId,
        creationDateUtc: data.creationDateUtc,
        description: data.description,
        responsible: data.responsible,
        status: data.status
      }

      this.cardStore.addCard(newCard);
    });
  }

  public closeHubConnection(){
    if (!this.isHubConnectionStarted) return;

    this.hubConnection.stop().then(() => {
      console.log('HUB STOPPED');
      this.isHubConnectionStarted = false;
    });
  }

}
