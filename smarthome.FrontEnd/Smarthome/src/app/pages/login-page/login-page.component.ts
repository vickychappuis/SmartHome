import { Component, OnInit } from '@angular/core';
import { AuthService } from '../../services/auth.service';
import { FormFields, FormValues } from '../../components/generic-form/types';
import { LoginDto } from '../../models/dtos/auth';
import { Validators } from '@angular/forms';
import { GenericFormComponent } from '../../components/generic-form/generic-form.component';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { HttpErrorResponse } from '@angular/common/http';

@Component({
  selector: 'app-login-page',
  standalone: true,
  imports: [GenericFormComponent, RouterModule],
  templateUrl: './login-page.component.html',
})
export class LoginPageComponent implements OnInit {
  returnUrl = '';
  error = '';

  loginFields: FormFields<LoginDto> = {
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
  };

  constructor(
    private authService: AuthService,
    private route: ActivatedRoute,
    private router: Router,
  ) {}

  ngOnInit() {
    this.returnUrl = this.route.snapshot.queryParams['returnUrl'] || '/';
  }

  onSubmit({ email, password }: FormValues<LoginDto>) {
    this.authService.login(email, password).subscribe({
      next: () => {
        this.router.navigateByUrl(this.returnUrl);
      },
      error: (err: HttpErrorResponse) => {
        this.error = err.error;
      },
    });
  }
}
