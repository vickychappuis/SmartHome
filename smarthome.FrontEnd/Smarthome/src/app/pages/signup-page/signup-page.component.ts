import { Component } from '@angular/core';
import { LoginDto, SignupDto } from '../../models/dtos/auth';
import { FormFields, FormValues } from '../../components/generic-form/types';
import { Validators } from '@angular/forms';
import { ActivatedRoute, RouterModule } from '@angular/router';
import { Router } from '@angular/router';
import { HttpErrorResponse } from '@angular/common/http';
import { AuthService } from '../../services/auth.service';
import { GenericFormComponent } from '../../components/generic-form/generic-form.component';
import { UrlValidator } from '../../models/forms/utils/validators';

@Component({
  selector: 'app-signup-page',
  standalone: true,
  imports: [GenericFormComponent, RouterModule],
  templateUrl: './signup-page.component.html',
})
export class SignupPageComponent {
  returnUrl = '';
  error = '';

  signupFields: FormFields<SignupDto> = {
    email: {
      label: 'Email',
      type: 'email',
      required: true,
      validations: [
        {
          name: 'email',
          validator: Validators.email,
          message: 'Enter a valid email address',
        },
      ],
    },
    password: {
      label: 'Password',
      type: 'password',
      required: true,
    },
    firstName: {
      label: 'First Name',
      type: 'text',
      required: true,
    },
    lastName: {
      label: 'Last Name',
      type: 'text',
      required: true,
    },
    imageUrl: {
      label: 'Profile Image URL',
      type: 'text',
      required: true,
      class: 'col-span-2',
      validations: [
        {
          name: 'imageUrl',
          validator: UrlValidator,
          message: 'Enter a valid URL',
        },
      ],
    },
  };

  constructor(
    private authService: AuthService,
    private route: ActivatedRoute,
    private router: Router,
  ) {}

  ngOnInit() {
    this.returnUrl = this.route.snapshot.queryParams['returnUrl'] || '/';
  }

  onSubmit(signupDto: FormValues<SignupDto>) {
    this.authService.signup(signupDto).subscribe({
      next: () => {
        this.router.navigateByUrl(this.returnUrl);
      },
      error: (err: HttpErrorResponse) => {
        this.error = err.error.errors
          ? Object.values(err.error?.errors).join('\n')
          : err.error;
      },
    });
  }
}
