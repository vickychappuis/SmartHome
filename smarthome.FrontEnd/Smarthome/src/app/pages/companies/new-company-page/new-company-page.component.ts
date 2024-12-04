import { Component } from '@angular/core';
import { CompanyService } from '../../../services/company.service';
import { CompanyDto } from '../../../models/dtos/companyDto';
import { FormFields } from '../../../components/generic-form/types';
import { GenericFormComponent } from '../../../components/generic-form/generic-form.component';
import { map, Observable } from 'rxjs';
import { DeviceService } from '../../../services/device.service';
import { AsyncPipe } from '@angular/common';
import { Router } from '@angular/router';

@Component({
  selector: 'app-new-company-page',
  standalone: true,
  imports: [GenericFormComponent, AsyncPipe],
  templateUrl: './new-company-page.component.html',
})
export class NewCompanyPageComponent {
  fields$!: Observable<FormFields<CompanyDto>>;

  constructor(
    private companyService: CompanyService,
    private deviceService: DeviceService,
    private router: Router,
  ) {}

  ngOnInit() {
    this.fields$ = this.deviceService.getValidators().pipe(
      map(validators => ({
        name: {
          type: 'text',
          label: 'Company Name',
          required: true,
          placeholder: 'Enter the company name',
          helpText: 'The full legal name of the company',
        },
        rut: {
          type: 'number',
          label: 'RUT',
          required: true,
          placeholder: 'Enter the RUT number',
          helpText: 'Unique tax identification number for the company',
        },
        logotypeUrl: {
          type: 'text',
          label: 'Logotype URL',
          required: false,
          placeholder: 'Enter the URL of the company logo',
          helpText: 'Optional field to provide a logo URL',
        },
        deviceValidator: {
          type: 'select',
          label: 'Device Validator',
          placeholder: 'Select a validator',
          helpText: 'The validator to use for the devices created',
          options: validators.map(v => ({
            label: v,
            value: v,
          })),
        },
      })),
    );
  }

  onSubmit(companyDto: CompanyDto) {
    this.companyService.addCompany(companyDto).subscribe(() => {
      this.router.navigate(['/companies/my']);
    });
  }
}
