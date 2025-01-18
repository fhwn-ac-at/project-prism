import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { RoundData } from './RoundData';

@Injectable({
  providedIn: 'root'
})
export class GameRoundService 
{
  public Initialize(totalRounds: bigint): void
  {
    this.RoundsObject.next({CurrentRound: 1n, TotalRounds: totalRounds});
  }

  public IncrementRound(): void
  {
    if (this.RoundsObject.value == undefined) return;

    if (this.RoundsObject.value.CurrentRound >= this.RoundsObject.value.TotalRounds) return;

    this.RoundsObject.next
    (
      {
        CurrentRound: this.RoundsObject.value.CurrentRound + 1n,
        TotalRounds: this.RoundsObject.value.TotalRounds
      }
    );
  }

  public RoundsObject: BehaviorSubject<RoundData | undefined> = new BehaviorSubject<RoundData | undefined>(undefined);
}
