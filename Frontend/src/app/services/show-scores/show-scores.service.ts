import { inject, Injectable } from '@angular/core';
import { GameApiService } from '../../networking/services/game-api/game-api.service';
import { ActivePlayersService } from '../current-players/active-players.service';
import { filter, Observable, ReplaySubject } from 'rxjs';
import { isNextRound } from '../../networking/dtos/game/game-flow/nextRound.guard';
import { NextRound } from '../../networking/dtos/game/game-flow/nextRound';
import { ShowScoresEvent } from './events/ShowScoresEvent';
import { PlayerData } from '../player-data/PlayerData';
import { isGameEnded } from '../../networking/dtos/game/game-flow/gameEnded.guard';
import { GameEnded } from '../../networking/dtos/game/game-flow/gameEnded';

@Injectable({
  providedIn: null
})
export class ShowScoresService 
{
  private gameApiService: GameApiService = inject(GameApiService);
  private activePlayersService: ActivePlayersService = inject(ActivePlayersService);
 
  private showScoresEventSubject: ReplaySubject<ShowScoresEvent> = new ReplaySubject<ShowScoresEvent>(5);

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
    // this is really stupid: I should just be able to take the player data from the service,
    // but there is no way to know that their scores have already been updated at this point.

    // therefore, update all the data to be sure
    let currentPlayers: PlayerData[] = this.activePlayersService.CurrentPlayers;

    let playersToInclude: PlayerData[] = [];
    
    for(let key of value.body.score as any)
    {   
      let val = (value.body.score as any)["key"];

      let found = currentPlayers.find((v) => v.Id == key);

      if  (found == undefined)
      {
        return;
      }

      playersToInclude.push({Username: found.Username, Id: found.Id, Score: val});
    };

    this.showScoresEventSubject.next(new ShowScoresEvent(value.body.word, playersToInclude));
  }
}
