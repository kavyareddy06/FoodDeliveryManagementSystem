import { ApplicationConfig } from '@angular/core';
import { provideRouter } from '@angular/router';

import { routes } from './app.routes';
import { provideHttpClient } from '@angular/common/http';
import { provideToastr } from 'ngx-toastr';
import { provideAnimations } from '@angular/platform-browser/animations';
import { provideAnimationsAsync } from '@angular/platform-browser/animations/async';
import { MatTooltipModule } from '@angular/material/tooltip';
export const appConfig: ApplicationConfig = {
  providers: [provideHttpClient(),provideRouter(routes), provideAnimations(), // required animations providers
    provideToastr(), provideAnimationsAsync(),]
};
