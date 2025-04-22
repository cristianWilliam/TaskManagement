import { Component, input, output } from "@angular/core";
import { MatCardModule } from "@angular/material/card";
import { FormsModule, NgForm } from "@angular/forms";
import { MatFormFieldModule } from "@angular/material/form-field";
import { MatInputModule } from "@angular/material/input";
import { MatButtonModule } from "@angular/material/button";
import { CardForm } from "../models/card-form";
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';


@Component({
  selector: 'app-task-manager-form',
  templateUrl: './task-manager-form.component.html',
  styleUrls: ['./task-manager-form.component.scss'],
  imports: [
    MatCardModule,
    FormsModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
    MatProgressSpinnerModule,
  ],
})
export class TaskManagerFormComponent {
  isLoading = input<boolean>(false);
  onAddCard = output<CardForm>();

  protected fireAddCard(form: NgForm) {
    if (form.invalid) return;
    this.onAddCard.emit(form.value as CardForm);

    form.resetForm();
  }
}
