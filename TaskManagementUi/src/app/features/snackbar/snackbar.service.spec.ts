import { TestBed } from '@angular/core/testing';
import { SnackbarService } from './snackbar.service';
import { MatSnackBar } from '@angular/material/snack-bar';

describe('SnackbarService', () => {
  let service: SnackbarService;
  let snackBarSpy: jasmine.SpyObj<MatSnackBar>;

  beforeEach(() => {
    // Create spy for MatSnackBar
    const spy = jasmine.createSpyObj('MatSnackBar', ['open']);

    TestBed.configureTestingModule({
      providers: [
        SnackbarService,
        { provide: MatSnackBar, useValue: spy }
      ]
    });

    service = TestBed.inject(SnackbarService);
    snackBarSpy = TestBed.inject(MatSnackBar) as jasmine.SpyObj<MatSnackBar>;
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  it('should show success message', () => {
    // Arrange
    const message = 'Success message';

    // Act
    service.showSuccess(message);

    // Assert
    expect(snackBarSpy.open).toHaveBeenCalledWith(message, 'Close', {
      duration: 3000,
      horizontalPosition: 'right',
      verticalPosition: 'bottom'
    });
  });

  it('should show error message', () => {
    // Arrange
    const message = 'Error message';

    // Act
    service.showError(message);

    // Assert
    expect(snackBarSpy.open).toHaveBeenCalledWith(message, 'Close', {
      duration: 3000,
      horizontalPosition: 'right',
      verticalPosition: 'bottom'
    });
  });
});
