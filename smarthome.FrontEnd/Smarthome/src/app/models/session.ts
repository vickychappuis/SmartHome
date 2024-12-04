import { UserRole } from './user';

export interface Session {
  token: string;
  userId: number;
  user: {
    userId: number;
    firstName: string;
    lastName: string;
    email: string;
    imageUrl: string;
    password: string;
    role: UserRole;
    registerDate: string;
  };
  expires: string;
}
