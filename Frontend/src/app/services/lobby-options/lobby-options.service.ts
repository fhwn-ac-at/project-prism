import { inject, Injectable } from '@angular/core';
import { ConfigService } from '../config/config.service';
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: null
})
export class LobbyOptionsService 
{
  private configService: ConfigService;

  public constructor(configService: ConfigService)
  {
    this.configService = configService;

    this.RoundAmount = new BehaviorSubject<number>(this.configService.configData.lobbyDefaults.roundAmount);
    this.RoundDuration = new BehaviorSubject<number>(this.configService.configData.lobbyDefaults.roundDuration);
  }

  public RoundAmount: BehaviorSubject<number>;
  public RoundDuration: BehaviorSubject<number>;

  public StartGame(): void
  {
    
  }
}
