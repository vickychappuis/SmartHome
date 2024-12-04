import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../environment/environment';
import { HomeMemberNotification } from '../models/notification';

@Injectable({
  providedIn: 'root',
})
export class NotificationService {
  constructor(private http: HttpClient) {}

  getHomeMemberNotifications(
    homeId: number,
    query?: { read?: boolean; sinceDate?: Date },
  ) {
    const params: Record<string, any> = {};

    if (query?.read !== undefined) {
      params['read'] = query.read.toString();
    }

    if (query?.sinceDate !== undefined) {
      params['sinceDate'] = query.sinceDate.toISOString();
    }

    return this.http.get<HomeMemberNotification[]>(
      `${environment.apiUrl}/homes/${homeId}/notifications`,
      {
        params,
        headers: {
          Authorization: `Bearer ${
            JSON.parse(localStorage.getItem('session')!).token
          }`,
        },
      },
    );
  }
}
