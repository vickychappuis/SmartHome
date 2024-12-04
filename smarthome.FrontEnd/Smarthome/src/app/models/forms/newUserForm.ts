import { FormFields } from '../../components/generic-form/types';
import { UserDto } from '../dtos/userDto';
import { Validators } from '@angular/forms';
import { UrlValidator } from './utils/validators';

export const newUserFormFields = (excludedRole?: UserDto['role']): FormFields<UserDto> => {
  const roles: { label: string; value: UserDto['role'] }[] = [
    { label: 'Administrator', value: 'Administrator' },
    { label: 'Company Owner', value: 'CompanyOwner' },
    { label: 'Home Owner', value: 'HomeOwner' },
  ];

  return {
    firstName: {
      label: 'First Name',
      type: 'text',
      required: true,
    },
    lastName: {
      label: 'Last Name',
      type: 'text',
      required: true,
    },
    email: {
      label: 'Email',
      type: 'email',
      required: true,
      validations: [
        {
          name: 'email',
          validator: Validators.email,
          message: 'Invalid email address',
        },
      ],
    },
    role: {
      label: 'Role',
      type: 'select',
      options: roles.filter(role => role.value !== excludedRole),
      placeholder: 'Select Role',
      required: true,
    },
    password: {
      label: 'Password',
      type: 'password',
      required: true,
      validations: [
        {
          name: 'minLength',
          validator: Validators.minLength(8),
          message: 'Password must be at least 8 characters',
        },
      ],
    },
    imageUrl: {
      label: 'Profile Image URL',
      type: 'text',
      required: true,
      validations: [
        {
          name: 'url',
          validator: UrlValidator,
          message: 'Please enter a valid URL',
        },
      ],
    },
  };
};
