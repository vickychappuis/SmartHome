import { Component, OnInit } from '@angular/core';
import { GenericFormComponent } from '../../components/generic-form/generic-form.component';
import { UserDto } from '../../models/dtos/userDto';
import { FormFields } from '../../components/generic-form/types';
import { UserService } from '../../services/user.service';
import { ActivatedRoute } from '@angular/router';
import { map, Subject } from 'rxjs';
import { UserRoleValues } from '../../models/user';
import { AsyncPipe } from '@angular/common';
import { newUserFormFields } from '../../models/forms/newUserForm';

@Component({
  selector: 'app-edit-user-page',
  standalone: true,
  imports: [GenericFormComponent, AsyncPipe],
  templateUrl: './edit-user-page.component.html',
})
export class EditUserPageComponent implements OnInit {
  userId!: number;
  fields$: Subject<FormFields<UserDto>> = new Subject();

  constructor(
    private userService: UserService,
    private route: ActivatedRoute,
  ) {}

  ngOnInit(): void {
    this.userId = Number(this.route.snapshot.params['id']);
    this.userService
      .getUser(this.userId)
      .pipe(
        map(user => ({
          ...user,
          role: UserRoleValues[user.role],
        })),
      )
      .subscribe(v => {
        const fields = newUserFormFields();

        for (const key in fields) {
          fields[key as keyof UserDto]!.defaultValue = v[key as keyof UserDto];
        }
        this.fields$.next(fields);
      });
  }

  onSubmit(values: UserDto) {
    this.userService.updateUser(this.userId, values).subscribe();
  }
}
