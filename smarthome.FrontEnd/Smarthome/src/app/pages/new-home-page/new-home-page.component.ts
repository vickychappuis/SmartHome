import { Component } from '@angular/core';
import { FormFields } from '../../components/generic-form/types';
import { HomeDto } from '../../models/dtos/homeDto';
import { HomeService } from '../../services/home.service';
import { Router } from '@angular/router';
import { GenericFormComponent } from '../../components/generic-form/generic-form.component';

@Component({
  selector: 'app-new-home-page',
  standalone: true,
  imports: [GenericFormComponent],
  templateUrl: './new-home-page.component.html',
})
export class NewHomePageComponent {
  fields: FormFields<HomeDto> = {
    homeName: {
      type: 'text',
      label: 'Home Name',
      required: false,
    },
    address: {
      type: 'text',
      label: 'Address',
      required: true,
    },
    latitude: {
      type: 'number',
      label: 'Latitude',
      required: true,
    },
    longitude: {
      type: 'number',
      label: 'Longitude',
      required: true,
    },
    maxMembers: {
      type: 'number',
      label: 'Maximum Members',
      required: true,
    },
  };

  constructor(private homeService: HomeService, private router: Router) {}

  onSubmit(homeDto: HomeDto) {
    this.homeService.addHome(homeDto).subscribe(() => {
      this.router.navigate(['/homes']);
    });
  }
}
