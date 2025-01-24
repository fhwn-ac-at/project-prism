import { createInterceptorCondition, IncludeBearerTokenCondition } from "keycloak-angular";

export const urlCondition = createInterceptorCondition<IncludeBearerTokenCondition>
(
    {
        urlPattern: /^((http|ws):\/\/localhost:5164)(\/.*)?$/i,
        bearerPrefix: 'Bearer'
    }
);