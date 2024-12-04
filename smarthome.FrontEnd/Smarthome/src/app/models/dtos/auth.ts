export type SignupDto = {
  email: string;
  password: string;
  firstName: string;
  lastName: string;
  imageUrl?: string;
};

export type LoginDto = {
  email: string;
  password: string;
};
