import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Config } from './Config';
import { firstValueFrom, Observer, Subject } from 'rxjs';

@Injectable({
  providedIn: null
})
export class ConfigService 
{
  private httpClient: HttpClient;

  public constructor(httpClient: HttpClient) 
  {
    this.httpClient = httpClient; 
  }

  public configData!: Config;

  public async Load()
  {
    return firstValueFrom
    (
      this.httpClient
      .get("./assets/config.json")).then((obj) => 
      {
        this.configData = obj as Config;

      }, 
      (err) => {throw new Error(err)}
    );
  }
}
