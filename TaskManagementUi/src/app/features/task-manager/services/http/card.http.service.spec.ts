import { TestBed } from '@angular/core/testing';
import { CardHttpService } from './card.http.service';
import { provideHttpClient } from '@angular/common/http';
import { provideHttpClientTesting, HttpTestingController } from '@angular/common/http/testing';
import { environment } from '../../../../../environments/environment';
import { CardResponse } from './card.response';
import { AddCardRequest } from './add-card.request';
import { MoveCardRequest } from './move-card.request';

describe('CardHttpService', () => {
  let service: CardHttpService;
  let httpTestingController: HttpTestingController;
  const apiUrl = `${environment.apiBaseUrl}/cards`;

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [
        CardHttpService,
        provideHttpClient(),
        provideHttpClientTesting()
      ]
    });

    service = TestBed.inject(CardHttpService);
    httpTestingController = TestBed.inject(HttpTestingController);
  });

  afterEach(() => {
    httpTestingController.verify();
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  it('should get all cards', () => {
    // Arrange
    const mockCards: CardResponse[] = [
      {
        cardId: '1',
        description: 'Task 1',
        responsible: 'User 1',
        creationDateUtc: new Date(),
        status: 'Todo'
      },
      {
        cardId: '2',
        description: 'Task 2',
        responsible: 'User 2',
        creationDateUtc: new Date(),
        status: 'InProgress'
      }
    ];

    // Act
    service.getAllCards().subscribe(cards => {
      expect(cards).toEqual(mockCards);
    });

    // Assert
    const req = httpTestingController.expectOne(apiUrl);
    expect(req.request.method).toEqual('GET');
    req.flush(mockCards);
  });

  it('should add a todo card', () => {
    // Arrange
    const description = 'New Task';
    const responsible = 'New User';
    const mockResponse: CardResponse = {
      cardId: '3',
      description,
      responsible,
      creationDateUtc: new Date(),
      status: 'Todo'
    };
    const expectedRequest: AddCardRequest = {
      description,
      responsible
    };

    // Act
    service.addTodoCard(description, responsible).subscribe(card => {
      expect(card).toEqual(mockResponse);
    });

    // Assert
    const req = httpTestingController.expectOne(apiUrl);
    expect(req.request.method).toEqual('POST');
    expect(req.request.body).toEqual(expectedRequest);
    req.flush(mockResponse);
  });

  it('should update card status', () => {
    // Arrange
    const cardId = '1';
    const newStatus = 'InProgress';
    const mockResponse: CardResponse = {
      cardId,
      description: 'Task 1',
      responsible: 'User 1',
      creationDateUtc: new Date(),
      status: newStatus
    };
    const expectedRequest: MoveCardRequest = {
      cardId,
      newStatus
    };

    // Act
    service.updateCardStatus(cardId, newStatus).subscribe(card => {
      expect(card).toEqual(mockResponse);
    });

    // Assert
    const req = httpTestingController.expectOne(`${apiUrl}/${cardId}`);
    expect(req.request.method).toEqual('PATCH');
    expect(req.request.body).toEqual(expectedRequest);
    req.flush(mockResponse);
  });
});
