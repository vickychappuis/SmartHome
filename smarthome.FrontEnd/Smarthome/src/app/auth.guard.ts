import {
  ActivatedRouteSnapshot,
  CanActivate,
  Router,
  RouterStateSnapshot,
} from '@angular/router';
import { AuthService } from './services/auth.service';
import { Injectable } from '@angular/core';
import { UserRole } from './models/user';

@Injectable({
  providedIn: 'root',
})
export class AuthGuard implements CanActivate {
  constructor(private router: Router, private auth: AuthService) {}

  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot) {
    const session = this.auth.getSession();
    if (session == null) {
      this.router.navigate(['/login'], {
        queryParams: { returnUrl: state.url },
      });
      return false;
    }

    const roles = route.data['roles'] as UserRole[] | undefined;

    if (roles && !roles.includes(session.user.role)) {
      this.router.navigate(['/']);
      return false;
    }

    return true;
  }
}
