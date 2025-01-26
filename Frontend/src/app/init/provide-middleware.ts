import { provideAppInitializer, provideZoneChangeDetection } from "@angular/core"
import { provideKeycloakAngular } from "./keycloak/initialize-keycloak"
import { provideHttpClient, withInterceptors } from "@angular/common/http"
import { provideAnimationsAsync } from "@angular/platform-browser/animations/async"
import { provideRouter, withComponentInputBinding } from "@angular/router"
import { AutoRefreshTokenService, UserActivityService, includeBearerTokenInterceptor, INCLUDE_BEARER_TOKEN_INTERCEPTOR_CONFIG, KeycloakService } from "keycloak-angular"
import { routes } from "./app.routes"
import { urlCondition } from "./keycloak/url-condition"

export const provideMiddleware = () =>
{
    return [
        provideKeycloakAngular(),
        provideHttpClient
        (
            withInterceptors([includeBearerTokenInterceptor])            
        ),
        {
            provide: INCLUDE_BEARER_TOKEN_INTERCEPTOR_CONFIG,
            useValue: [urlCondition] // <-- Note that multiple conditions might be added.
        },
        AutoRefreshTokenService,
        UserActivityService, 
        provideZoneChangeDetection({ eventCoalescing: true }),
        provideRouter(routes, withComponentInputBinding()),
        provideAnimationsAsync(),
    ]
}