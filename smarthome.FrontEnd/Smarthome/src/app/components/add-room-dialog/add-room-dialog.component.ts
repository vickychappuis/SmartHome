import { Component, EventEmitter, Input, Output } from '@angular/core';
import { HomeService } from '../../services/home.service';
import { FormFields } from '../generic-form/types';
import { HttpErrorResponse } from '@angular/common/http';
import { DialogModule } from 'primeng/dialog';
import { GenericFormComponent } from '../generic-form/generic-form.component';

@Component({
  selector: 'app-add-room-dialog',
  standalone: true,
  imports: [DialogModule, GenericFormComponent],
  templateUrl: './add-room-dialog.component.html',
})
export class AddRoomDialogComponent {
  @Input() visible = false;
  @Input() homeId!: number;
  @Output() onClose = new EventEmitter<void>();
  @Output() onAddRoom = new EventEmitter<void>();
  error = '';

  fields: FormFields<{ roomName: string }> = {
    roomName: {
      label: 'Room Name',
      type: 'text',
    },
  };

  constructor(private homeService: HomeService) {}

  onSubmit({ roomName }: { roomName: string }) {
    this.homeService
      .addRoom(this.homeId, { roomName, homeId: this.homeId })
      .subscribe({
        next: () => {
          this.onAddRoom.emit();
          this.close();
        },
        error: (error: HttpErrorResponse) => {
          this.error = error.error;
        },
      });
  }

  close() {
    this.onClose.emit();
  }
}
