import { Component, EventEmitter, Input, Output } from '@angular/core';
import { Observable, of } from 'rxjs';
import { FormFields } from '../generic-form/types';
import { DeviceService } from '../../services/device.service';
import { GenericFormComponent } from '../generic-form/generic-form.component';
import { DialogModule } from 'primeng/dialog';
import { AsyncPipe } from '@angular/common';
import { ProgressBarModule } from 'primeng/progressbar';

type ImportDevicesDto = {
  data: string;
  importStrategy: string;
};

@Component({
  selector: 'app-import-device-dialog',
  standalone: true,
  imports: [GenericFormComponent, DialogModule, AsyncPipe, ProgressBarModule],
  templateUrl: './import-device-dialog.component.html',
})
export class ImportDeviceDialogComponent {
  @Input() visible: boolean = false;
  @Output() onClose = new EventEmitter<void>();
  @Output() onImportDevices = new EventEmitter<void>();
  error = '';
  loading = false;

  fields$!: Observable<FormFields<ImportDevicesDto>>;

  constructor(private deviceService: DeviceService) {}

  ngOnInit() {
    this.deviceService.getImportStrategies().subscribe(strategies => {
      this.fields$ = of({
        data: { label: 'Data', type: 'textarea' },
        importStrategy: {
          label: 'Import Strategy',
          type: 'select',
          options: strategies.map(strategy => ({
            label: strategy,
            value: strategy,
          })),
        },
      });
    });
  }

  onSubmit({ data, importStrategy }: ImportDevicesDto) {
    this.loading = true;
    this.deviceService.importDevices(data, importStrategy, 3).subscribe({
      next: () => {
        this.onImportDevices.emit();
        this.loading = false;
      },
      error: error => {
        this.error = error.error;
        this.loading = false;
      },
    });
  }

  close() {
    this.onClose.emit();
  }
}
