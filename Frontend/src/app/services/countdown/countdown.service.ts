import { inject, Injectable } from '@angular/core';
import { filter, map, Observable, Observer, Subject, Subscription, takeWhile, timer } from 'rxjs';
import { CountdownEvent } from './CountdownEvent';
import { GameApiService } from '../../networking/services/game-api/game-api.service';
import { isSearchedWord } from '../../networking/dtos/game/game-flow/searchedWord.guard';
import { SearchedWord } from '../../networking/dtos/game/game-flow/searchedWord';
import { GameRoundService } from '../game-round/game-round.service';
import { isNextRound } from '../../networking/dtos/game/game-flow/nextRound.guard';
import { NextRound } from '../../networking/dtos/game/game-flow/nextRound';

@Injectable({
  providedIn: null
})
export class CountdownService 
{
  private gameApiService: GameApiService = inject(GameApiService);
  private roundService: GameRoundService = inject(GameRoundService);

  private timerSubject: Subject<CountdownEvent> = new Subject<CountdownEvent>();

  private timerSubscription: Subscription | null = null;

  public constructor()
  {
    this.gameApiService.ObserveGameFlowEvent()
      .pipe(filter(isSearchedWord))
      .subscribe(this.OnSearchedWordEvent);
    
    this.gameApiService.ObserveGameFlowEvent()
    .pipe(filter(isNextRound))
    .subscribe(this.OnNextRound);
  }

  public ObserveTimerEvent(): Observable<CountdownEvent>
  {
    return this.timerSubject.asObservable();
  }

  public IsRunning() : boolean
  {
    return this.timerSubscription != null;
  }

  public StartTimer(startNumber: number, delayInMs: number): void
  {
    if (!Number.isInteger(startNumber) || startNumber < 0) 
    {
        throw new Error("Invalid startNumber");
    }

    if (delayInMs <= 0) 
    {
        throw new Error("Invalid delay");
    }

    if (this.timerSubscription != null) 
    {
        return;
    }

    this.timerSubscription = timer(0, delayInMs).pipe
    (
      map((num: number) => startNumber - num),
      takeWhile((num) => num >= 0)
    )
    .subscribe
    (
      {
        next: (timeLeft) => 
        {
          // reset if this is last.
          if (timeLeft == 0) 
          {
            this.StopTimer();
            return;
          }
 
          this.timerSubject.next(new CountdownEvent(timeLeft));
        }
      }
    );
  }

  private StopTimer(): void
  {
    if (this.timerSubscription == null) 
    {
        return;
    }

    this.timerSubscription.unsubscribe();
    this.timerSubscription = null;
    this.timerSubject.next(new CountdownEvent(0));
  }

  private OnSearchedWordEvent = (_: SearchedWord) =>
  {
    if (this.roundService.RoundsObject.value == undefined)
    {
      return;
    }

    if (this.IsRunning())
    {
      return;
    }

    this.StartTimer(this.roundService.RoundsObject.value.RoundDuration, 1000);
  }

  private OnNextRound = (_: NextRound) => 
  {
    this.StopTimer();
  }
}