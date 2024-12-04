import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { DeviceService } from '../../services/device.service';
import { CompanyService } from '../../services/company.service';
import { Device, DeviceType, DeviceTypeNames } from '../../models/device';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { CommonModule } from '@angular/common';
import { Company } from '../../models/company';

@Component({
  selector: 'app-device-details-page',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './device-details-page.component.html',
})
export class DeviceDetailsPageComponent implements OnInit {
  device$!: Observable<Device | undefined>;
  companyName: string = '';
  constructor(
    private route: ActivatedRoute,
    private deviceService: DeviceService,
    private companyService: CompanyService,
  ) {}

  ngOnInit(): void {
    const deviceId = this.route.snapshot.paramMap.get('deviceId');
    if (deviceId) {
      this.device$ = this.getDevice(deviceId);
      this.device$.subscribe((device: Device | undefined) => {
        if (device) {
          this.getCompany(device.companyId);
        }
      });
    }

  }
  
  getDevice(deviceId: string): Observable<Device | undefined> {
    return this.deviceService.getDevices().pipe(
      map((devices: Device[]) => devices.find((device: Device) => device.deviceId === Number(deviceId)))
    );    
  }

  getCompany(companyId: number) {
    this.companyService.getCompanies({companyId: companyId}).subscribe((companies: Company[]) => {
      this.companyName = companies.find(c => c.companyId === companyId)?.name || '';
    });
  }

  


  getDeviceType(type: DeviceType): string {
    return DeviceTypeNames[type];
  }
}

