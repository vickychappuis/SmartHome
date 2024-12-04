import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { Router, RouterModule } from '@angular/router';
import { MenuItem } from 'primeng/api';
import { MenubarModule } from 'primeng/menubar';
import { AuthService } from '../../services/auth.service';
import { UserRole } from '../../models/user';

const roleItems: Record<UserRole, MenuItem[]> = {
  [UserRole.Administrator]: [
    {
      label: 'Users',
      icon: 'pi pi-users',
      routerLink: 'users',
    },
    {
      label: 'Companies',
      icon: 'pi pi-building',
      routerLink: 'companies',
    },
    {
      label: 'Devices',
      icon: 'pi pi-lightbulb',
      routerLink: 'devices',
    },
  ],
  [UserRole.CompanyOwner]: [
    {
      label: 'My Company',
      icon: 'pi pi-building',
      routerLink: 'companies/my',
    },
    {
      label: 'Devices',
      icon: 'pi pi-lightbulb',
      routerLink: 'devices',
    },
  ],
  [UserRole.HomeOwner]: [
    {
      label: 'Homes',
      icon: 'pi pi-home',
      routerLink: 'homes',
    },
    {
      label: 'Devices',
      icon: 'pi pi-lightbulb',
      routerLink: 'devices',
    },
  ],
};

const notLoggedInItems: MenuItem[] = [];

@Component({
  selector: 'app-navbar',
  standalone: true,
  imports: [CommonModule, RouterModule, MenubarModule],
  templateUrl: './navbar.component.html',
})
export class NavbarComponent {
  items: MenuItem[];

  constructor(private authService: AuthService, private router: Router) {
    const role = this.authService.getSession()?.user.role ?? null;

    this.items = role !== null ? roleItems[role] : notLoggedInItems;
  }

  logout() {
    this.authService.logout().subscribe({
      complete: () => {
        this.router.navigate(['/login']);
      },
    });
  }
}
