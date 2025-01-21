import { inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { ConfigService } from '../services/config/config.service';

@Injectable({
  providedIn: null
})
export class ApiService {

  private httpClient: HttpClient = inject(HttpClient);
  private configService: ConfigService = inject(ConfigService);


}
