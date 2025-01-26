import { ApplicationConfig, provideAppInitializer } from '@angular/core';
import { provideRootAppServices } from './provide-root-app-services';
import { provideMiddleware } from './provide-middleware';
import { FetchConfig } from './fetch-config';

export const appConfig: ApplicationConfig = {
  providers: 
  [   
    provideAppInitializer(FetchConfig),
    provideMiddleware(),
    provideRootAppServices(),
  ]
};