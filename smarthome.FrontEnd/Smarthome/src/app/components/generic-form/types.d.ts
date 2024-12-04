import { ValidationRule } from './validation-rule';

export type FormSchema = Record<string, unknown>;

export type FormField<TFormSchema> = {
  label: string;
  type:
    | 'text'
    | 'email'
    | 'select'
    | 'checkbox'
    | 'number'
    | 'password'
    | 'textarea';
  required?: boolean;
  options?: { label: string; value: TFormSchema }[];
  placeholder?: string;
  helpText?: string;
  validations?: ValidationRule[];
  defaultValue?: TFormSchema;
  class?: string;
};

export type FormFields<TFormSchema> = {
  [FormKey in keyof TFormSchema]: FormField<TFormSchema[FormKey]>;
};

export type FormValues<TFormSchema> = {
  [FormKey in keyof TFormSchema]: TFormSchema[FormKey];
};