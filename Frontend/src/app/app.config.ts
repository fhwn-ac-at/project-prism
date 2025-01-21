import { ApplicationConfig, provideAppInitializer } from '@angular/core';
import { provideRootAppServices } from './init/provide-root-app-services';
import { provideMiddleware } from './init/provide-middleware';
import { FetchConfig } from './init/fetch-app';

export const appConfig: ApplicationConfig = {
  providers: 
  [   
    provideAppInitializer(FetchConfig),
    provideMiddleware(),
    provideRootAppServices(),
  ]
};