import { inject, Injectable } from '@angular/core';
import { ConfigService } from '../config/config.service';
import { BehaviorSubject, firstValueFrom, Subject } from 'rxjs';
import { LobbyApiService } from '../../networking/services/lobby-api/lobby-api.service';
import { GameApiService } from '../../networking/services/game-api/game-api.service';
import { GameIdService } from '../gameId/game-id.service';
import { GameStarted } from '../../networking/dtos/lobby/gameStarted';
import { isGameStarted } from '../../networking/dtos/lobby/gameStarted.guard';
import { isRoundAmountChanged } from '../../networking/dtos/lobby/roundAmountChanged.guard';
import { isRoundDurationChanged } from '../../networking/dtos/lobby/roundDurationChanged.guard';
import { RoundAmountChanged } from '../../networking/dtos/lobby/roundAmountChanged';
import { RoundDurationChanged } from '../../networking/dtos/lobby/roundDurationChanged';
import { GameRoundService } from '../game-round/game-round.service';

@Injectable({
  providedIn: null
})
export class LobbyService 
{
  private configService: ConfigService;
  private lobbyApiService: LobbyApiService;
  private gameApiService: GameApiService;
  private gameIdService: GameIdService;
  private roundsService: GameRoundService;

  public constructor
  (
    configService: ConfigService,
    lobbyApi: LobbyApiService,
    gameApiService: GameApiService,
    gameIdService: GameIdService,
    roundsService: GameRoundService
  )
  {
    this.configService = configService;
    this.lobbyApiService = lobbyApi;
    this.gameApiService = gameApiService;
    this.gameIdService = gameIdService;
    this.roundsService = roundsService;
    
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
    if (this.gameIdService.GameId.value == undefined) return Promise.reject(new Error("no game id"));

    await firstValueFrom(this.lobbyApiService.StartGame(this.gameIdService.GameId.value));

    this.roundsService.Initialize(this.RoundAmount.value, this.RoundDuration.value)
    this.GameStarted.next();
  }

  private OnLobbyEvent(data: GameStarted | RoundAmountChanged | RoundDurationChanged) : void
  {
    if (isRoundAmountChanged(data)) 
    {
      this.RoundAmount.next(data.body.rounds);
    }
    else if (isRoundDurationChanged(data)) 
    {
      this.RoundDuration.next(data.body.duration);
    }
    else if (isGameStarted(data))
    {
      this.GameStarted.next();
    }
    else
    {
      throw new Error("Invalid message type");
    }
  }
}
