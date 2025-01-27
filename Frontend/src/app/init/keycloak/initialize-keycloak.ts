import { EnvironmentProviders, inject } from "@angular/core";
import {provideKeycloak, createInterceptorCondition, IncludeBearerTokenCondition, AutoRefreshTokenService, UserActivityService, withAutoRefreshToken, INCLUDE_BEARER_TOKEN_INTERCEPTOR_CONFIG} from "keycloak-angular";
import { ConfigService } from "../../services/config/config.service";
  
export function provideKeycloakAngular(): EnvironmentProviders 
{
  return provideKeycloak
  (
      {
          config: 
          {
            url: "http://localhost:8180",
            realm: "prism",
            clientId: "Frontend"
          },
          initOptions: 
          {
            onLoad: "login-required",
            silentCheckSsoRedirectUri: window.location.origin + '/silent-check-sso.html'
          },
          features: 
          [
            withAutoRefreshToken
            (
              {
                onInactivityTimeout: 'logout',
                sessionTimeout: 300000
              }
            )
          ],
          
      }
  )
};