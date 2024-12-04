import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Session } from '../models/session';
import { environment } from '../../environment/environment';
import { EMPTY, iif, tap } from 'rxjs';
import { SignupDto } from '../models/dtos/auth';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  constructor(private http: HttpClient) {}

  login(email: string, password: string) {
    return this.http
      .post<Session>(`${environment.apiUrl}/auth/login`, {
        email,
        password,
      })
      .pipe(
        tap({
          next: response => {
            localStorage.setItem('session', JSON.stringify(response));
          },
        }),
      );
  }

  signup(signupDto: SignupDto) {
    return this.http
      .post<Session>(`${environment.apiUrl}/auth/signup`, signupDto)
      .pipe(
        tap({
          next: response => {
            localStorage.setItem('session', JSON.stringify(response));
          },
        }),
      );
  }

  logout() {
    const token = (localStorage.getItem('session') as Session | null)?.token;

    localStorage.removeItem('session');

    return token
      ? this.http.post(`${environment.apiUrl}/auth/logout`, null, {
          headers: {
            Authorization: `Bearer ${token}`,
          },
        })
      : EMPTY;
  }

  isLoggedIn() {
    const session_ = localStorage.getItem('session');

    if (!session_) {
      return false;
    }

    const session = JSON.parse(session_) as Session;

    if (new Date(session.expires) < new Date()) {
      this.logout();
      return false;
    }

    return true;
  }

  getSession() {
    if (!this.isLoggedIn()) {
      return null;
    }

    return JSON.parse(localStorage.getItem('session')!) as Session;
  }
}
