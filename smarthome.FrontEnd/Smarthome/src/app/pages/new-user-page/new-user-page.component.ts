import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { InputTextModule } from 'primeng/inputtext';
import { GenericFormComponent } from '../../components/generic-form/generic-form.component';
import { FormFields } from '../../components/generic-form/types';
import { UserDto } from '../../models/dtos/userDto';
import { UserService } from '../../services/user.service';
import { Router } from '@angular/router';
import { newUserFormFields } from '../../models/forms/newUserForm';

@Component({
  selector: 'app-new-user-page',
  standalone: true,
  imports: [FormsModule, InputTextModule, GenericFormComponent],
  templateUrl: './new-user-page.component.html',
})
export class NewUserPageComponent {
  fields: FormFields<UserDto>;

  constructor(private userService: UserService, private router: Router) {
    this.fields = newUserFormFields('HomeOwner');
  }

  onSubmit(userDto: UserDto) {
    this.userService.addUser(userDto).subscribe(() => {
      this.router.navigate(['/users']);
    });
  }
}
