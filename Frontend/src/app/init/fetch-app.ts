import { inject } from "@angular/core";
import { ConfigService } from "../services/config/config.service";

export async function FetchConfig() : Promise<void>
{
  const config = inject(ConfigService);

  await config.Load(); 
}