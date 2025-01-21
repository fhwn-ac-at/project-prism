import { createInterceptorCondition, IncludeBearerTokenCondition } from "keycloak-angular";

export const urlCondition = createInterceptorCondition<IncludeBearerTokenCondition>
(
    {
        urlPattern: /^(http:\/\/localhost:5164)(\/.*)?$/i,
        bearerPrefix: 'Bearer'
    }
);