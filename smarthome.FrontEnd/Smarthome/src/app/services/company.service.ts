import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { CompanyDto } from '../models/dtos/companyDto';
import { Company } from '../models/company';
import { environment } from '../../environment/environment';

@Injectable({
  providedIn: 'root',
})
export class CompanyService {
  constructor(private http: HttpClient) {}

  myCompany() {
    return this.http.get<Company>(`${environment.apiUrl}/companies/my`, {
      headers: {
        Authorization: `Bearer ${
          JSON.parse(localStorage.getItem('session')!).token
        }`,
      },
    });
  }

  getCompanies(query?: {
    companyName?: string;
    companyOwnerName?: string;
    page?: number;
    pageSize?: number;
    companyId?: number;
  }) {
    const params: Record<string, string> = {};

    if (query?.companyName) {
      params['companyName'] = query.companyName;
    }

    if (query?.companyOwnerName) {
      params['companyOwnerName'] = query.companyOwnerName;
    }

    if (query?.page && query?.pageSize) {
      const skip = (query.page - 1) * query.pageSize;
      const take = query.pageSize;

      params['skip'] = skip.toString();
      params['take'] = take.toString();
    }

    if (query?.companyId) {
      params['companyId'] = query.companyId.toString();
    }

    return this.http.get<Company[]>(`${environment.apiUrl}/companies`, {
      params,
      headers: {
        Authorization: `Bearer ${
          JSON.parse(localStorage.getItem('session')!).token
        }`,
      },
    });
  }

  getCompanyByName(companyName: string) {
    const params = new HttpParams().set('company_name', companyName);

    return this.http.get<Company>(`${environment.apiUrl}/companies`, {
      params,
    });
  }

  getUserCompany(userId: number) {
    this.http
      .get<Company>(`${environment.apiUrl}/companies/user/${userId}`)
      .subscribe(company => {
        return company;
      });
  }

  addCompany(company: CompanyDto) {
    return this.http.post(`${environment.apiUrl}/companies`, company, {
      headers: {
        Authorization: `Bearer ${
          JSON.parse(localStorage.getItem('session')!).token
        }`,
      },
    });
  }
}
