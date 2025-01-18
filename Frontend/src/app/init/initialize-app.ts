import { inject } from "@angular/core";
import { ConfigService } from "../services/config/config.service";
import { KeycloakService } from "keycloak-angular";

export async function InitializeApplication() : Promise<void>
{
  const config = inject(ConfigService);

  await config.Load(); 
}