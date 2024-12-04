import { Validators } from '@angular/forms';

export const UrlValidator = Validators.pattern(
  /^(https?:\/\/)?([\da-z\.-]+)\.([a-z\.]{2,6})([\/\w \.-]*)*\/?$/,
);
