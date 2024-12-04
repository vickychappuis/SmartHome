export interface UserDto {
  firstName: string;
  lastName: string;
  email: string;
  password: string;
  role?: 'Administrator' | 'CompanyOwner' | 'HomeOwner';
  imageUrl?: string;
}
