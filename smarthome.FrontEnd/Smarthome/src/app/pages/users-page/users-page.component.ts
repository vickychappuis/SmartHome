import { Component, OnInit } from '@angular/core';
import { TableModule } from 'primeng/table';
import { User, UserRole, UserRoleLabels } from '../../models/user';
import { ButtonModule } from 'primeng/button';
import { UserService } from '../../services/user.service';
import { Observable } from 'rxjs';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { DropdownModule } from 'primeng/dropdown';
import { InputTextModule } from 'primeng/inputtext';

type RoleOption = {
  name: string;
  value: UserRole | null;
};

@Component({
  selector: 'app-users-page',
  standalone: true,
  imports: [
    CommonModule,
    TableModule,
    ButtonModule,
    RouterModule,
    FormsModule,
    DropdownModule,
    InputTextModule,
  ],
  templateUrl: './users-page.component.html',
})
export class UsersPageComponent implements OnInit {
  users$!: Observable<User[]>;
  page = 1;
  pageSize = 6;
  nameQuery: string | undefined;
  roleQuery: RoleOption = { name: 'All', value: null };

  roles: RoleOption[] = [
    { name: 'All', value: null },
    { name: 'Administrator', value: UserRole.Administrator },
    { name: 'Company Owner', value: UserRole.CompanyOwner },
    { name: 'Home Owner', value: UserRole.HomeOwner },
  ];

  constructor(private userService: UserService) {}

  ngOnInit(): void {
    this.fetchUsers();
  }

  decrementPage() {
    this.page--;
    this.fetchUsers();
  }

  incrementPage() {
    this.page++;
    this.fetchUsers();
  }

  resetFilters() {
    this.nameQuery = undefined;
    this.roleQuery = { name: 'All', value: null };
    this.fetchUsers();
  }

  fetchUsers() {
    this.users$ = this.userService.getUsers({
      page: this.page,
      pageSize: this.pageSize,
      name: this.nameQuery,
      role: this.roleQuery.value ?? undefined,
    });
  }

  deleteUser(id: number) {
    this.userService.deleteUser(id).subscribe(() => {
      this.fetchUsers();
    });
  }

  getUserRoleLabel(role: UserRole): string {
    return UserRoleLabels[role];
  }

  isAdmin(user: User): boolean {
    return user.role === UserRole.Administrator;
  }
  
}
