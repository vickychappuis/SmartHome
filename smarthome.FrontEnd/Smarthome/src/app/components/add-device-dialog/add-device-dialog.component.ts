import { Component, EventEmitter, Input, Output } from '@angular/core';
import { DialogModule } from 'primeng/dialog';
import { FormFields } from '../generic-form/types';
import { HomeService } from '../../services/home.service';
import { GenericFormComponent } from '../generic-form/generic-form.component';
import { AsyncPipe } from '@angular/common';
import { map, Observable, switchMap, zip } from 'rxjs';
import { HttpErrorResponse } from '@angular/common/http';
import { DeviceService } from '../../services/device.service';
import { HomeDeviceDto } from '../../models/dtos/homeDeviceDto';

@Component({
  selector: 'app-add-device-dialog',
  standalone: true,
  imports: [DialogModule, GenericFormComponent, AsyncPipe],
  templateUrl: './add-device-dialog.component.html',
})
export class AddDeviceDialogComponent {
  @Input() visible = false;
  @Input() homeId!: number;
  @Output() onClose = new EventEmitter<void>();
  @Output() onAddDevice = new EventEmitter<void>();
  error = '';

  fields$!: Observable<FormFields<HomeDeviceDto>>;

  constructor(
    private homeService: HomeService,
    private deviceService: DeviceService,
  ) {}

  ngOnInit() {
    this.fields$ = zip(
      this.deviceService.getDevices(),
      this.homeService.getRooms(this.homeId),
    ).pipe(
      map(([devices, rooms]) => ({
        deviceId: {
          label: 'Device',
          type: 'select',
          required: true,
          options: devices.map(device => ({
            label: device.name,
            value: device.deviceId,
          })),
        },
        roomId: {
          label: 'Room',
          type: 'select',
          required: false,
          options: rooms.map(room => ({
            label: room.roomName,
            value: room.roomId,
          })),
        },
      })),
    );
  }

  onSubmit(addDeviceDto: HomeDeviceDto) {
    this.homeService.addHomeDevice(this.homeId, addDeviceDto).subscribe({
      next: () => {
        this.onAddDevice.emit();
        this.close();
      },
      error: (error: HttpErrorResponse) => {
        this.error = error.error;
      },
    });
  }

  close() {
    this.onClose.emit();
  }
}
