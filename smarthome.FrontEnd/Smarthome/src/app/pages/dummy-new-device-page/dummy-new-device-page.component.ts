import { Component } from '@angular/core';
import { ImportDeviceDialogComponent } from '../../components/import-device-dialog/import-device-dialog.component';

@Component({
  selector: 'app-dummy-new-device-page',
  standalone: true,
  imports: [ImportDeviceDialogComponent],
  templateUrl: './dummy-new-device-page.component.html',
})
export class DummyNewDevicePageComponent {
  addDeviceDialogVisible = false;

  openImportDialog() {
    this.addDeviceDialogVisible = true;
  }

  onImportDevices() {
    this.addDeviceDialogVisible = false;
  }
}
