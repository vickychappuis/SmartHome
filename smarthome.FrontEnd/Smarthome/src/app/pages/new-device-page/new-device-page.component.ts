import { Component } from '@angular/core';
import { FormsModule, Validators } from '@angular/forms';
import { DropdownModule } from 'primeng/dropdown';
import { DeviceService } from '../../services/device.service';
import { DeviceType, DeviceTypeNames } from '../../models/device';
import { GenericFormComponent } from '../../components/generic-form/generic-form.component';
import { DeviceDto } from '../../models/dtos/deviceDto';
import { FormFields } from '../../components/generic-form/types';
import { CommonModule } from '@angular/common';
import { ChangeDetectorRef } from '@angular/core';
import { CompanyService } from '../../services/company.service';
import { AuthService } from '../../services/auth.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-add-device-page',
  standalone: true,
  imports: [DropdownModule, FormsModule, GenericFormComponent, CommonModule],
  templateUrl: './new-device-page.component.html',
  animations: [],
})
export class AddDevicePageComponent {
  selectedDeviceType: DeviceType = DeviceType.SecurityCamera;
  fields!: FormFields<DeviceDto>;
  error = '';

  ngOnInit() {
    this.updateFields();
  }

  updateFields() {
    this.fields = {
      Name: {
        label: 'Name',
        type: 'text',
        required: true,
        validations: [
          {
            name: 'required',
            validator: Validators.required,
            message: 'Name is required',
          },
        ],
      },
      Model: {
        label: 'Model',
        type: 'text',
        required: true,
        validations: [
          {
            name: 'required',
            validator: Validators.required,
            message: 'Model is required',
          },
        ],
      },
      Description: {
        label: 'Description',
        type: 'text',
        required: true,
        validations: [
          {
            name: 'required',
            validator: Validators.required,
            message: 'Description is required',
          },
        ],
      },
      ImageUrl: {
        label: 'Image URL',
        type: 'text',
        required: true,
        validations: [
          {
            name: 'required',
            validator: Validators.required,
            message: 'Image URL is required',
          },
        ],
      },
      ...this.getSpecificDeviceFields(this.selectedDeviceType),
    };
    this.cdr.detectChanges();
  }

  getSpecificDeviceFields(deviceType: DeviceType): FormFields<any> {
    switch (deviceType) {
      case DeviceType.SecurityCamera:
        return {
          IsInterior: {
            label: 'Is interior',
            type: 'checkbox',
            defaultValue: false,
          },
          IsExterior: {
            label: 'Is exterior',
            type: 'checkbox',
            defaultValue: false,
          },
          CanDetectMotion: {
            label: 'Can detect motion',
            type: 'checkbox',
            defaultValue: false,
          },
          CanDetectPerson: {
            label: 'Can detect person',
            type: 'checkbox',
            defaultValue: false,
          },
        };
      case DeviceType.WindowSensor:
        return {};
      case DeviceType.SmartLamp:
        return {};
      case DeviceType.MotionSensor:
        return {};
      default:
        return {};
    }
  }

  deviceTypes = Object.entries(DeviceTypeNames).map(([key, value]) => ({
    key: Number(key) as DeviceType,
    value,
  }));

  constructor(
    private deviceService: DeviceService,
    private cdr: ChangeDetectorRef,
    private companyService: CompanyService,
    private authService: AuthService,
    private router: Router,
  ) {}

  onSubmit(device: DeviceDto) {
    const session = this.authService.getSession();
    if (!session) {
      return;
    }

    this.companyService.myCompany().subscribe({
      next: company => {
        device.CompanyId = company.companyId;

        if (this.selectedDeviceType == DeviceType.SecurityCamera) {
          device.IsInterior = !!device.IsInterior;
          device.IsExterior = !!device.IsExterior;
          device.CanDetectMotion = !!device.CanDetectMotion;
          device.CanDetectPerson = !!device.CanDetectPerson;
        }

        this.deviceService
          .postDevice(device, this.selectedDeviceType)
          .subscribe({
            next: () => {
              this.router.navigate(['/devices']);
            },
            error: error => {
              this.error = error.error;
            },
          });
      },
      error: () => {
        this.router.navigate(['/companies/new']);
      },
    });
  }

  trackByIndex(index: number, obj: any): any {
    return index;
  }
}
