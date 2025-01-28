import { ApplicationConfig, provideAppInitializer } from '@angular/core';
import { provideRootAppServices } from './provide-root-app-services';
import { provideMiddleware } from './provide-middleware';

export const appConfig: ApplicationConfig = {
  providers: 
  [   
    provideMiddleware(),
    provideRootAppServices(),
  ]
};