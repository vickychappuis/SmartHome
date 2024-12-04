import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { DeviceDto } from '../models/dtos/deviceDto';
import { environment } from '../../environment/environment';
import { Device, DeviceType } from '../models/device';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class DeviceService {
  constructor(private http: HttpClient) {}

  getDevices(query?: {
    page?: number;
    pageSize?: number;
    name?: string;
    deviceType?: DeviceType;
  }): Observable<Device[]> {
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

    if (query?.deviceType !== undefined) {
      params['deviceType'] = query.deviceType.toString();
    }

    return this.http.get<Device[]>(`${environment.apiUrl}/devices`, {
      params,
      headers: {
        Authorization: `Bearer ${
          JSON.parse(localStorage.getItem('session')!).token
        }`,
      },
    });
  }

  getDevice(deviceId: number) {
    return this.http.get<Device>(`${environment.apiUrl}/devices/${deviceId}`, {
      headers: {
        Authorization: `Bearer ${
          JSON.parse(localStorage.getItem('session')!).token
        }`,
      },
    });
  }

  getDeviceTypes() {
    return this.http.get<DeviceType[]>(`${environment.apiUrl}/devices/types`);
  }

  postDevice(device: DeviceDto, type: number) {
    return this.http.post(`${environment.apiUrl}/devices`, device, {
      params: {
        type,
      },
      headers: {
        Authorization: `Bearer ${
          JSON.parse(localStorage.getItem('session')!).token
        }`,
      },
    });
  }

  getImportStrategies() {
    return this.http.get<string[]>(
      `${environment.apiUrl}/devices/import/strategies`,
      {
        headers: {
          Authorization: `Bearer ${
            JSON.parse(localStorage.getItem('session')!).token
          }`,
        },
      },
    );
  }

  importDevices(data: string, importStrategy: string, companyId: number) {
    return this.http.post(
      `${environment.apiUrl}/devices/import`,
      {
        data,
        importStrategy,
        companyId,
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

  getValidators() {
    return this.http.get<string[]>(`${environment.apiUrl}/devices/validators`, {
      headers: {
        Authorization: `Bearer ${
          JSON.parse(localStorage.getItem('session')!).token
        }`,
      },
    });
  }
}
