import { Component, OnInit } from '@angular/core';
import { TableModule } from 'primeng/table';
import { Company } from '../../../models/company';
import { ButtonModule } from 'primeng/button';
import { CompanyService } from '../../../services/company.service';
import { Observable } from 'rxjs';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { InputTextModule } from 'primeng/inputtext';

@Component({
  selector: 'app-companies-page',
  standalone: true,
  imports: [
    CommonModule,
    TableModule,
    ButtonModule,
    RouterModule,
    FormsModule,
    InputTextModule,
  ],
  templateUrl: './companies-page.component.html',
})
export class CompaniesPageComponent implements OnInit {
  companies$!: Observable<Company[]>;
  filterCompanyName: string = '';
  filterCompanyOwnerName: string = '';
  page: number = 1;
  pageSize: number = 3;

  constructor(private companyService: CompanyService) {}

  ngOnInit(): void {
    this.fetchCompanies();
  }

  resetFilters(): void {
    this.filterCompanyName = '';
    this.filterCompanyOwnerName = '';
    this.fetchCompanies();
  }

  incrementPage(): void {
    this.page++;
    this.fetchCompanies();
  }

  decrementPage(): void {
    this.page--;
    this.fetchCompanies();
  }

  filterCompanies(): void {
    this.page = 1;
    this.fetchCompanies();
  }

  fetchCompanies(): void {
    this.companies$ = this.companyService.getCompanies({
      page: this.page,
      pageSize: this.pageSize,
      companyName: this.filterCompanyName,
      companyOwnerName: this.filterCompanyOwnerName,
    });
  }
}
