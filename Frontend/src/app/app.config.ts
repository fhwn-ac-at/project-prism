import { ApplicationConfig, provideAppInitializer } from '@angular/core';
import { provideRootAppServices } from './init/provide-root-app-services';
import { provideMiddleware } from './init/provide-middleware';
import { InitializeApplication } from './init/initialize-app';

export const appConfig: ApplicationConfig = {
  providers: 
  [   
    provideAppInitializer(InitializeApplication),
    provideMiddleware(),
    provideRootAppServices(),
  ]
};