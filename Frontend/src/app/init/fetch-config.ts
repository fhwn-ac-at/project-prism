import { inject } from "@angular/core";
import { ConfigService } from "../services/config/config.service";
import { Config } from "../services/config/Config";

export function FetchConfig() : Promise<void | Config>
{
  const config = inject(ConfigService);

  return config.Load().catch((err) => 
  {
    console.log("Error encountered on startup: " + err);

    window.location.href = "/assets/startupError.html";
    throw new Error(err);
  });
}
