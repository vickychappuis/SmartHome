import { ValidatorFn } from "@angular/forms";

export interface ValidationRule {
    name: string;      
    validator: ValidatorFn;
    message: string;       
}