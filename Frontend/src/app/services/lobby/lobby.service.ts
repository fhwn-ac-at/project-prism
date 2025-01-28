import { inject, Injectable } from '@angular/core';
import { BehaviorSubject, filter, firstValueFrom, Observable, Subject } from 'rxjs';
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
import { environment } from '../../../environment/environment';

@Injectable({
  providedIn: null
})
export class LobbyService 
{
  private lobbyApiService: LobbyApiService;
  private gameApiService: GameApiService;
  private gameIdService: GameIdService;
  private roundsService: GameRoundService;

  private roundAmount: BehaviorSubject<number>;
  private roundDuration: BehaviorSubject<number>;

  public constructor
  (
    lobbyApi: LobbyApiService,
    gameApiService: GameApiService,
    gameIdService: GameIdService,
    roundsService: GameRoundService
  )
  {
    this.lobbyApiService = lobbyApi;
    this.gameApiService = gameApiService;
    this.gameIdService = gameIdService;
    this.roundsService = roundsService;
    
    this.roundAmount = new BehaviorSubject<number>(environment.lobbyDefaults.roundAmount);
    this.roundDuration = new BehaviorSubject<number>(environment.lobbyDefaults.roundDuration);

    this.gameApiService.ObserveLobbyEvent()
      .subscribe(this.OnLobbyEvent)
  }
  
  public ObserveRoundAmount(): Observable<number>
  {
    return this.roundAmount.asObservable();
  }

  public ObserveRoundDuration(): Observable<number>
  {
    return this.roundDuration.asObservable();
  }

  public async SendRoundAmount(roundAmount: number): Promise<void>
  {
    this.roundAmount.next(roundAmount);
    return this.gameApiService.SendRoundAmount(roundAmount);
  }

  public async SendRoundDuration(duration: number): Promise<void>
  {
    this.roundDuration.next(duration);
    return this.gameApiService.SendRoundDuration(duration);
  }

  public async StartGame(): Promise<void>
  {
    if (this.gameIdService.GameId.value == undefined) return Promise.reject(new Error("no game id"));

    return firstValueFrom(this.lobbyApiService.StartGame(this.gameIdService.GameId.value));
  }

  private OnLobbyEvent = (data: RoundAmountChanged | RoundDurationChanged | GameStarted) : void =>
  {
    if (isRoundAmountChanged(data)) 
    {
      this.roundAmount.next(data.body.rounds);
    }
    else if (isRoundDurationChanged(data)) 
    {
      this.roundDuration.next(data.body.duration);
    }
    else if (isGameStarted(data))
    {
      this.roundsService.Initialize(this.roundAmount.value, this.roundDuration.value);
    }
    else
    {
      throw new Error("Invalid message type");
    }
  }
}
