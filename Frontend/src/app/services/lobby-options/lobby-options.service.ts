import { inject, Injectable } from '@angular/core';
import { ConfigService } from '../config/config.service';
import { BehaviorSubject, firstValueFrom, Subject } from 'rxjs';
import { LobbyApiService } from '../lobby-api/lobby-api.service';
import { GameApiService } from '../gameApi/game-api.service';
import { GameIdService } from '../gameId/game-id.service';
import { GameStarted } from '../../networking/dtos/lobby/gameStarted';
import { RoundAmountChanged } from '../gameApi/messages/lobby/RoundAmountChanged';
import { RoundDurationChanged } from '../gameApi/messages/lobby/RoundDurationChanged';
import { isGameStarted } from '../../networking/dtos/lobby/gameStarted.guard';
import { isRoundAmountChanged } from '../../networking/dtos/lobby/roundAmountChanged.guard';
import { isRoundDurationChanged } from '../../networking/dtos/lobby/roundDurationChanged.guard';

@Injectable({
  providedIn: null
})
export class LobbyOptionsService 
{
  private configService: ConfigService;
  private lobbyApiService: LobbyApiService;
  private gameApiService: GameApiService;
  private gameIdService: GameIdService;

  public constructor
  (
    configService: ConfigService,
    lobbyApi: LobbyApiService,
    gameApiService: GameApiService,
    gameIdService: GameIdService,
  )
  {
    this.configService = configService;
    this.lobbyApiService = lobbyApi;
    this.gameApiService = gameApiService;
    this.gameIdService = gameIdService;
    
    this.RoundAmount = new BehaviorSubject<number>(this.configService.configData.lobbyDefaults.roundAmount);
    this.RoundDuration = new BehaviorSubject<number>(this.configService.configData.lobbyDefaults.roundDuration);

    this.RoundAmount.subscribe((val => this.gameApiService.SendRoundAmount(val)));
    this.RoundDuration.subscribe((val => this.gameApiService.SendRoundDuration(val)));

    this.gameApiService.ObserveLobbyEvent().subscribe(this.OnLobbyEvent)
  }

  public RoundAmount: BehaviorSubject<number>;
  public RoundDuration: BehaviorSubject<number>;
  public GameStarted: Subject<void> = new Subject();
  
  public async StartGame(): Promise<void>
  {
    if (this.gameIdService.GameId.value.isNone()) return Promise.reject(new Error("no game id"));

    await firstValueFrom(this.lobbyApiService.StartGame(this.gameIdService.GameId.value.value));

    this.GameStarted.next();
  }

  private OnLobbyEvent(data: GameStarted | RoundAmountChanged | RoundDurationChanged) 
  {
    if (isGameStarted(data)) 
    {
      this.GameStarted.next();
    }
    else if (isRoundAmountChanged(data)) 
    {
      this.RoundAmount.next(data.body.rounds);
    }
    else if (isRoundDurationChanged(data)) 
    {
      this.RoundDuration.next(data.body.duration);
    }
    else
    {
      throw new Error("Invalid message type");
    }
  }
}
