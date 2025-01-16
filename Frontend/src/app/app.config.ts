import { ApplicationConfig, inject, provideAppInitializer, provideZoneChangeDetection } from '@angular/core';
import { provideRouter, withComponentInputBinding } from '@angular/router';
import { routes } from './app.routes';
import { provideHttpClient } from '@angular/common/http';
import { provideAnimationsAsync } from '@angular/platform-browser/animations/async';
import { ConfigService } from './services/config/config.service';
import { Config } from './services/config/Config';
 
async function initializeApp() : Promise<Config>
{
  const config = inject(ConfigService);

  return await config.Load(); 
}

export const appConfig: ApplicationConfig = {
  providers: 
  [
    provideZoneChangeDetection({ eventCoalescing: true }),
    provideRouter(routes, withComponentInputBinding()),
    provideHttpClient(),
    {provide: ConfigService, useClass: ConfigService},
    provideAnimationsAsync(),
    provideAppInitializer(initializeApp),
  ]
};
