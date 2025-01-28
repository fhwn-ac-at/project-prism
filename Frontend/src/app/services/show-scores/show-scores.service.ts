import { inject, Injectable } from '@angular/core';
import { GameApiService } from '../../networking/services/game-api/game-api.service';
import { ActivePlayersService } from '../current-players/active-players.service';
import { filter, Observable, ReplaySubject, Subject } from 'rxjs';
import { isNextRound } from '../../networking/dtos/game/game-flow/nextRound.guard';
import { NextRound } from '../../networking/dtos/game/game-flow/nextRound';
import { ShowScoresEvent } from './events/ShowScoresEvent';
import { isGameEnded } from '../../networking/dtos/game/game-flow/gameEnded.guard';
import { GameEnded } from '../../networking/dtos/game/game-flow/gameEnded';

@Injectable({
  providedIn: null
})
export class ShowScoresService 
{
  private gameApiService: GameApiService = inject(GameApiService);
  private activePlayersService: ActivePlayersService = inject(ActivePlayersService);
 
  private showScoresEventSubject: Subject<ShowScoresEvent> = new Subject<ShowScoresEvent>();

  public constructor()
  {
    this.gameApiService.ObserveGameFlowEvent()
      .pipe(filter((val) => isNextRound(val) || isGameEnded(val)))
      .subscribe(this.OnNextRoundOrGameEnded);
  }

  public ObserveShowScoresEvent(): Observable<ShowScoresEvent>
  {
    return this.showScoresEventSubject.asObservable();
  }

  private OnNextRoundOrGameEnded = (value: NextRound | GameEnded): void =>
  {  
    for (let userId of Object.keys(value.body.score))
    {   
      let newScore: number = (value.body.score as any)[userId];

      this.activePlayersService.AddToScore(userId, newScore)
    };

    this.showScoresEventSubject.next
    (
      new ShowScoresEvent
      (
        value.body.word, 
        this.activePlayersService.CurrentPlayers, 
        isGameEnded(value) ? true : false
      )
    );
  }
}
