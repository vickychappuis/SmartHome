import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { UserDto } from '../models/dtos/userDto';
import { User, UserRole } from '../models/user';
import { environment } from '../../environment/environment';

@Injectable({
  providedIn: 'root',
})
export class UserService {
  constructor(private http: HttpClient) {}

  getUsers(query?: {
    page?: number;
    pageSize?: number;
    name?: string;
    role?: UserRole;
  }) {
    const params: Record<string, string> = {};

    if (query?.page !== undefined && query.pageSize !== undefined) {
      const skip = ((query?.page ?? 1) - 1) * (query?.pageSize ?? 20);
      const take = query?.pageSize ?? 20;

      params['skip'] = skip.toString();
      params['take'] = take.toString();
    }

    if (query?.name !== undefined) {
      params['name'] = query.name;
    }

    if (query?.role !== undefined) {
      params['role'] = query.role.toString();
    }

    return this.http.get<User[]>(`${environment.apiUrl}/users`, {
      params,
      headers: {
        Authorization: `Bearer ${
          JSON.parse(localStorage.getItem('session')!).token
        }`,
      },
    });
  }

  getUser(id: number) {
    return this.http.get<User>(`${environment.apiUrl}/users/${id}`, {
      headers: {
        Authorization: `Bearer ${
          JSON.parse(localStorage.getItem('session')!).token
        }`,
      },
    });
  }

  addUser(user: UserDto) {
    return this.http.post(`${environment.apiUrl}/users`, user, {
      headers: {
        Authorization: `Bearer ${
          JSON.parse(localStorage.getItem('session')!).token
        }`,
      },
    });
  }

  updateUser(id: number, user: UserDto) {
    return this.http.put(`${environment.apiUrl}/users/${id}`, user, {
      headers: {
        Authorization: `Bearer ${
          JSON.parse(localStorage.getItem('session')!).token
        }`,
      },
    });
  }

  deleteUser(id: number) {
    return this.http.delete(`${environment.apiUrl}/users/${id}`, {
      headers: {
        Authorization: `Bearer ${
          JSON.parse(localStorage.getItem('session')!).token
        }`,
      },
    });
  }
}
