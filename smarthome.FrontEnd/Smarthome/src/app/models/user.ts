import { Company } from './company';

export enum UserRole {
  Administrator = 0,
  CompanyOwner = 1,
  HomeOwner = 2,
}

export const UserRoleLabels: Record<UserRole, string> = {
  [UserRole.Administrator]: 'Administrator',
  [UserRole.CompanyOwner]: 'Company Owner',
  [UserRole.HomeOwner]: 'Home Owner',
};

export const UserRoleValues = {
  [UserRole.Administrator]: 'Administrator',
  [UserRole.CompanyOwner]: 'CompanyOwner',
  [UserRole.HomeOwner]: 'HomeOwner',
} as const satisfies Record<UserRole, string>;

export type User = {
  userId: number;
  firstName: string;
  lastName: string;
  email: string;
  imageUrl: string;
  password: string;
  role: UserRole;
  registerDate: Date;
  companyId?: number;
};
