import { ApplicationConfig, provideZoneChangeDetection } from '@angular/core';
import { provideRouter } from '@angular/router';
import { routes } from './app.routes';
import { provideHttpClient } from '@angular/common/http';
import { API_BASE_URL as api_url, TestServiceProxy } from '../shared/service-proxies';

export const appConfig: ApplicationConfig = {
  providers: [
    {
      provide: api_url,
      useValue: 'https://localhost:7045'
    },
    provideZoneChangeDetection({ eventCoalescing: true }), 
    provideHttpClient(),
    provideRouter(routes),
    TestServiceProxy
  ],
};
