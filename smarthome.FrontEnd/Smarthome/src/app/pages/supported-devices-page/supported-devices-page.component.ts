import { Component } from '@angular/core';
import {DeviceTypeNames} from '../../models/device';
import { CommonModule } from '@angular/common';
import { FieldsetModule } from 'primeng/fieldset';

@Component({
  selector: 'app-supported-devices-page',
  standalone: true,
  imports: [CommonModule, FieldsetModule],
  templateUrl: './supported-devices-page.component.html',
})
export class SupportedDevicesPageComponent {

  

  deviceTypes = Object.values(DeviceTypeNames);

  trackByIndex(index: number, obj: any): any {
    return index;
  }

}
