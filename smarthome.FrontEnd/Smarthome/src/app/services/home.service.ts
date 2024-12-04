import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Home, HomeDevice, HomeMember, Room } from '../models/home';
import { environment } from '../../environment/environment';
import { HomeDto } from '../models/dtos/homeDto';
import { HomeDeviceDto } from '../models/dtos/homeDeviceDto';
import { RoomDto } from '../models/dtos/roomDto';

@Injectable({
  providedIn: 'root',
})
export class HomeService {
  constructor(private http: HttpClient) {}

  getHomes(query?: { userId?: number }) {
    const params: { userId?: number } = query?.userId
      ? { userId: query.userId }
      : {};
    console.log(params);
    return this.http.get<Home[]>(`${environment.apiUrl}/homes`, {
      params,
      headers: {
        Authorization: `Bearer ${
          JSON.parse(localStorage.getItem('session')!).token
        }`,
      },
    });
  }

  getHomeMembers(homeId: number) {
    return this.http.get<HomeMember[]>(
      `${environment.apiUrl}/homes/${homeId}/members`,
      {
        headers: {
          Authorization: `Bearer ${
            JSON.parse(localStorage.getItem('session')!).token
          }`,
        },
      },
    );
  }

  getHomeDevices(homeId: number, params?: { roomId?: number }) {
    return this.http.get<HomeDevice[]>(
      `${environment.apiUrl}/homes/${homeId}/devices`,
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

  addHome(homeDto: HomeDto) {
    return this.http.post<Home>(`${environment.apiUrl}/homes`, homeDto, {
      headers: {
        Authorization: `Bearer ${
          JSON.parse(localStorage.getItem('session')!).token
        }`,
      },
    });
  }

  addHomeMember(homeId: number, userEmail: string) {
    return this.http.post<HomeMember>(
      `${environment.apiUrl}/homes/${homeId}/members`,
      { userEmail },
      {
        headers: {
          Authorization: `Bearer ${
            JSON.parse(localStorage.getItem('session')!).token
          }`,
        },
      },
    );
  }

  addHomeDevice(homeId: number, addHomeDeviceDto: HomeDeviceDto) {
    return this.http.post(
      `${environment.apiUrl}/homes/${homeId}/devices`,
      addHomeDeviceDto,
      {
        headers: {
          Authorization: `Bearer ${
            JSON.parse(localStorage.getItem('session')!).token
          }`,
        },
      },
    );
  }

  getRooms(homeId: number) {
    return this.http.get<Room[]>(
      `${environment.apiUrl}/homes/${homeId}/rooms`,
      {
        headers: {
          Authorization: `Bearer ${
            JSON.parse(localStorage.getItem('session')!).token
          }`,
        },
      },
    );
  }

  configureNotifications(
    homeId: number,
    userId: number,
    canReceiveNotifications: boolean,
  ) {
    return this.http.patch(
      `${environment.apiUrl}/homes/${homeId}/members/notifications`,
      {
        userId: userId,
        allowed: canReceiveNotifications,
      },
      {
        headers: {
          Authorization: `Bearer ${
            JSON.parse(localStorage.getItem('session')!).token
          }`,
        },
      },
    );
  }

  addRoom(homeId: number, room: RoomDto) {
    return this.http.post(`${environment.apiUrl}/homes/${homeId}/rooms`, room, {
      headers: {
        Authorization: `Bearer ${
          JSON.parse(localStorage.getItem('session')!).token
        }`,
      },
    });
  }
}
