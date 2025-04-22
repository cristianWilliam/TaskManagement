import { TestBed } from '@angular/core/testing';
import { CardsStore } from './cards.store.service';
import { Card } from '../models/card.model';

describe('CardsStore', () => {
  let service: CardsStore;

  // Sample test data
  const mockCards: Card[] = [
    {
      cardId: '1',
      description: 'Task 1',
      responsible: 'User 1',
      creationDateUtc: new Date('2025-04-20T10:00:00Z'),
      status: 'Todo'
    },
    {
      cardId: '2',
      description: 'Task 2',
      responsible: 'User 2',
      creationDateUtc: new Date('2025-04-20T11:00:00Z'),
      status: 'InProgress'
    },
    {
      cardId: '3',
      description: 'Task 3',
      responsible: 'User 3',
      creationDateUtc: new Date('2025-04-20T12:00:00Z'),
      status: 'Done'
    }
  ];

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [CardsStore]
    });
    service = TestBed.inject(CardsStore);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  it('should initialize with empty tasks', () => {
    // Assert
    expect(service.getTasks()()).toEqual([]);
  });

  it('should initialize tasks with provided cards', () => {
    // Act
    service.initializeTasks(mockCards);

    // Assert
    expect(service.getTasks()()).toEqual(mockCards);
  });

  it('should add a new card', () => {
    // Arrange
    service.initializeTasks([]);
    const newCard: Card = {
      cardId: '4',
      description: 'New Task',
      responsible: 'New User',
      creationDateUtc: new Date('2025-04-20T13:00:00Z'),
      status: 'Todo'
    };

    // Act
    service.addCard(newCard);

    // Assert
    expect(service.getTasks()()).toContain(newCard);
    expect(service.getTasks()().length).toBe(1);
  });

  it('should not add a card if it already exists', () => {
    // Arrange
    service.initializeTasks(mockCards);
    const existingCard = { ...mockCards[0] };

    // Act
    service.addCard(existingCard);

    // Assert
    expect(service.getTasks()().length).toBe(mockCards.length);
  });

  it('should update card status', () => {
    // Arrange
    service.initializeTasks(mockCards);
    const cardIdToUpdate = '1';
    const newStatus = 'InProgress';

    // Act
    service.updateCardStatus(cardIdToUpdate, newStatus);

    // Assert
    const updatedCard = service.getTasks()().find(card => card.cardId === cardIdToUpdate);
    expect(updatedCard?.status).toBe(newStatus);
  });

  it('should not modify other cards when updating status', () => {
    // Arrange
    service.initializeTasks(mockCards);
    const cardIdToUpdate = '1';
    const newStatus = 'InProgress';
    const otherCards = mockCards.filter(card => card.cardId !== cardIdToUpdate);

    // Act
    service.updateCardStatus(cardIdToUpdate, newStatus);

    // Assert
    const currentOtherCards = service.getTasks()().filter(card => card.cardId !== cardIdToUpdate);
    expect(currentOtherCards).toEqual(otherCards);
  });
});
