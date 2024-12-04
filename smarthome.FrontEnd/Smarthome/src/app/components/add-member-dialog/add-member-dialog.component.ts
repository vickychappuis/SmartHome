import { Component, EventEmitter, Input, Output } from '@angular/core';
import { DialogModule } from 'primeng/dialog';
import { FormFields } from '../generic-form/types';
import { HomeService } from '../../services/home.service';
import { GenericFormComponent } from '../generic-form/generic-form.component';
import { UserService } from '../../services/user.service';
import { AsyncPipe } from '@angular/common';
import { Validators } from '@angular/forms';

type AddMemberDto = { userEmail: string };

@Component({
  selector: 'app-add-member-dialog',
  standalone: true,
  imports: [DialogModule, GenericFormComponent],
  templateUrl: './add-member-dialog.component.html',
})
export class AddMemberDialogComponent {
  @Input() visible = false;
  @Input() homeId!: number;
  @Output() onClose = new EventEmitter<void>();
  @Output() onAddMember = new EventEmitter<void>();
  error = '';

  fields: FormFields<AddMemberDto> = {
    userEmail: {
      label: 'Member Email',
      type: 'text',
      validations: [
        {
          name: 'email',
          validator: Validators.email,
          message: 'Invalid email',
        },
      ],
    },
  };

  constructor(private homeService: HomeService) {}

  onSubmit({ userEmail }: AddMemberDto) {
    this.homeService.addHomeMember(this.homeId, userEmail).subscribe({
      next: () => {
        this.onAddMember.emit();
        this.close();
      },
      error: error => {
        this.error = error.error;
      },
    });
  }

  close() {
    this.onClose.emit();
  }
}
