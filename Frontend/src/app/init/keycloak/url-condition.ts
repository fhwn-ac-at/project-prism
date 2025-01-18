import { createInterceptorCondition, IncludeBearerTokenCondition } from "keycloak-angular";

export const urlCondition = createInterceptorCondition<IncludeBearerTokenCondition>
(
    {
        urlPattern: /^(http:\/\/localhost:8180)(\/.*)?$/i,
        bearerPrefix: 'Bearer'
    }
);