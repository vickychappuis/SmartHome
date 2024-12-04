import { Component, Input, OnInit, Output, EventEmitter } from '@angular/core';

import { FormControl, ReactiveFormsModule, ValidatorFn } from '@angular/forms';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { FormFields, FormValues } from './types';
import { InputTextModule } from 'primeng/inputtext';
import { DropdownModule } from 'primeng/dropdown';
import { ButtonModule } from 'primeng/button';
import { CheckboxModule } from 'primeng/checkbox';
import { SimpleChanges } from '@angular/core';
import { OnChanges } from '@angular/core';
import { InputTextareaModule } from 'primeng/inputtextarea';

@Component({
  selector: 'app-generic-form',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    InputTextModule,
    DropdownModule,
    ButtonModule,
    CheckboxModule,
    InputTextareaModule,
  ],
  templateUrl: './generic-form.component.html',
})
export class GenericFormComponent<TFormSchema extends { [key: string]: any }>
  implements OnInit, OnChanges
{
  @Input() submitButtonLabel = 'Submit';
  @Input() fieldContainerClass = '';
  @Input() fields!: FormFields<TFormSchema>;
  @Output() formSubmit = new EventEmitter<FormValues<TFormSchema>>();
  formGroup!: FormGroup;
  formFields!: ({
    key: string;
  } & FormFields<TFormSchema>[keyof TFormSchema])[];

  constructor(private fb: FormBuilder) {}

  ngOnChanges(changes: SimpleChanges): void {
    if (changes['fields']) {
      this.createForm();
      this.formFields = Object.keys(this.fields).map(key => ({
        key,
        ...this.fields[key as keyof TFormSchema],
      }));
    }
  }

  ngOnInit(): void {
    this.createForm();
    this.formFields = Object.keys(this.fields).map(key => ({
      key,
      ...this.fields[key as keyof TFormSchema],
    }));
  }

  createForm() {
    let group = {} as Record<keyof TFormSchema, FormControl>; // Object to hold the form controls

    (Object.keys(this.fields) as (keyof TFormSchema)[]).forEach(field => {
      const validators: ValidatorFn[] = [];
      const fieldConfig = this.fields[field];

      if (fieldConfig.required) {
        validators.push(Validators.required);
      }

      if (fieldConfig.validations) {
        fieldConfig.validations.forEach(validation => {
          validators.push(validation.validator);
        });
      }

      group[field] = this.fb.control(
        fieldConfig.defaultValue || '',
        Validators.compose(validators),
      );
    });

    this.formGroup = this.fb.group(group);
  }

  onSubmit() {
    if (this.formGroup.valid) {
      this.formSubmit.emit(this.getValues());
    } else {
      console.log('Form is invalid');
    }
  }

  private getValues() {
    let formValues = {} as TFormSchema;
    (Object.keys(this.formGroup.controls) as (keyof TFormSchema)[]).forEach(
      key => {
        formValues[key] = this.formGroup.controls[key as string].value;
      },
    );
    return formValues;
  }
}
