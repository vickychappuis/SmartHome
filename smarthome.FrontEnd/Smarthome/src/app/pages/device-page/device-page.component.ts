import { Component } from '@angular/core';
import { DeviceService } from '../../services/device.service';
import { Device, DeviceType, DeviceTypeNames } from '../../models/device';
import { Observable } from 'rxjs';
import { TableModule } from 'primeng/table';
import { ButtonModule } from 'primeng/button';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { RouterModule } from '@angular/router';
import { DropdownModule } from 'primeng/dropdown';
import { FormsModule } from '@angular/forms';
import { InputTextModule } from 'primeng/inputtext';
import { AuthService } from '../../services/auth.service';
import { UserRole } from '../../models/user';

type TypeOption = { name: string; value: DeviceType | null };

@Component({
  selector: 'app-device-page',
  standalone: true,
  imports: [
    TableModule,
    ButtonModule,
    CommonModule,
    RouterModule,
    DropdownModule,
    FormsModule,
    InputTextModule,
  ],
  templateUrl: './device-page.component.html',
})
export class DevicePageComponent {
  page = 1;
  pageSize = 6;
  nameQuery: string | undefined;
  typeQuery: TypeOption = { name: 'All', value: null };

  ngOnInit(): void {
    this.fetchDevices();
  }

  incrementPage() {
    this.page++;
    this.fetchDevices();
  }

  decrementPage() {
    this.page--;
    this.fetchDevices();
  }

  devices$!: Observable<Device[]>;
  constructor(
    private deviceService: DeviceService,
    private authService: AuthService,
  ) {}

  deviceTypes: TypeOption[] = [
    { name: 'All', value: null },
    { name: 'Security Camera', value: DeviceType.SecurityCamera },
    { name: 'Window Sensor', value: DeviceType.WindowSensor },
    { name: 'Smart Lamp', value: DeviceType.SmartLamp },
    { name: 'Motion Sensor', value: DeviceType.MotionSensor },
  ];

  resetFilters() {
    this.nameQuery = undefined;
    this.typeQuery = { name: 'All', value: null };
    this.fetchDevices();
  }

  fetchDevices() {
    this.devices$ = this.deviceService.getDevices({
      page: this.page,
      pageSize: this.pageSize,
      name: this.nameQuery,
      deviceType: this.typeQuery.value ?? undefined,
    });
  }

  getDeviceType(type: DeviceType): string {
    return DeviceTypeNames[type];
  }

  canAddDevice(): boolean {
    return this.authService.getSession()?.user.role === UserRole.CompanyOwner;
  }
}
