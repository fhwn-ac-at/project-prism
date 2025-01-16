import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Config } from './Config';
import { Observer, Subject } from 'rxjs';

@Injectable({
  providedIn: null
})
export class ConfigService 
{
  private httpClient: HttpClient;

  private configChangedEvent: Subject<Config> = new Subject<Config>();

  public constructor(httpClient: HttpClient) 
  {
    this.httpClient = httpClient; 
  }

  public configData!: Config;

  public Subscribe(obs: Partial<Observer<Config>>)
  {
    this.configChangedEvent.subscribe(obs);
  }

  public async Load()
  {
    return new Promise<Config>(resolve => 
    {
      this.httpClient
      .get("config.json")
      .subscribe(
        {
          next: (obj) => 
          {
            this.configData = obj as Config;
            resolve(this.configData);
          },
          error: (err) => 
          {
            throw err;
          }
        }
      )
    })
  }
}
