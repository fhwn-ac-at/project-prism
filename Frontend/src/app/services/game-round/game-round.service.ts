import { inject, Injectable } from '@angular/core';
import { BehaviorSubject, filter, Observable } from 'rxjs';
import { RoundData } from './RoundData';
import { GameApiService } from '../../networking/services/game-api/game-api.service';
import { isNextRound } from '../../networking/dtos/game/game-flow/nextRound.guard';
import { NextRound } from '../../networking/dtos/game/game-flow/nextRound';
import { isGameStarted } from '../../networking/dtos/lobby/gameStarted.guard';

@Injectable({
  providedIn: null
})
export class GameRoundService 
{
  private gameApiSerive: GameApiService = inject(GameApiService);
  
  public constructor()
  {
    this.gameApiSerive.ObserveGameFlowEvent()
      .pipe(filter(isNextRound))
      .subscribe(() => this.OnNextRoundReceived);
  }

  public RoundsObject: BehaviorSubject<RoundData | undefined> = new BehaviorSubject<RoundData | undefined>(undefined);

  public Initialize(totalRounds: number, roundDuration: number): void
  {
    this.RoundsObject.next({CurrentRound: 1, TotalRounds: totalRounds, RoundDuration: roundDuration});
  }

  private OnNextRoundReceived = (val: NextRound): void =>
  {
    if (this.RoundsObject.value == undefined) return;

    if (this.RoundsObject.value.CurrentRound >= this.RoundsObject.value.TotalRounds) return;

    this.RoundsObject.next
    (
      {
        CurrentRound: val.body.round,
        TotalRounds: this.RoundsObject.value.TotalRounds,
        RoundDuration: this.RoundsObject.value.RoundDuration
      }
    );
  }
}
