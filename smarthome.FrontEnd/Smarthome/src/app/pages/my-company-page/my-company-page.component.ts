import { Component, OnInit } from '@angular/core';
import { CompanyService } from '../../services/company.service';
import { Observable } from 'rxjs';
import { Company } from '../../models/company';
import { AsyncPipe, CommonModule } from '@angular/common';

@Component({
  selector: 'app-my-company-page',
  standalone: true,
  imports: [CommonModule, AsyncPipe],
  templateUrl: './my-company-page.component.html',
})
export class MyCompanyPageComponent implements OnInit {
  company$!: Observable<Company>;
  constructor(private companyService: CompanyService) {}

  ngOnInit() {
    this.company$ = this.companyService.myCompany();
  }
}
