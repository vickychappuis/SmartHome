import { Component, OnInit } from '@angular/core';
import { AuthService } from '../../services/auth.service';
import { Session } from '../../models/session';
import { UserRole, UserRoleLabels } from '../../models/user';
import { Company } from '../../models/company';
import { CompanyService } from '../../services/company.service';

@Component({
  selector: 'app-home-page',
  standalone: true,
  imports: [],
  templateUrl: './home-page.component.html',
})
export class HomePageComponent implements OnInit {
  user!: Session['user'];
  company: Company | null = null;

  constructor(
    private authService: AuthService,
    private companyService: CompanyService,
  ) {}

  ngOnInit(): void {
    this.user = this.authService.getSession()!.user;

    if (this.user.role == UserRole.CompanyOwner) {
      this.companyService.myCompany().subscribe(company => {
        this.company = company;
      });
    }
  }

  getRole() {
    return UserRoleLabels[this.user.role];
  }
}
