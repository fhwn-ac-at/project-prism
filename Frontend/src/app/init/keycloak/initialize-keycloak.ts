import { EnvironmentProviders } from "@angular/core";
import {provideKeycloak, withAutoRefreshToken, } from "keycloak-angular";
import { environment } from "../../../environment/environment";

export function provideKeycloakAngular(): EnvironmentProviders 
{
  return provideKeycloak
  (
      {
          config: 
          {
            url: environment.keycloak.url,
            realm: environment.keycloak.realm,
            clientId: environment.keycloak.clientId
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