import { inject, Injectable } from '@angular/core';
import { IsEqual, PlayerData } from '../player-data/PlayerData';
import { filter, Observable, PartialObserver, ReplaySubject, Subject, Subscription } from 'rxjs';
import { CurrentPlayersMessage } from './events/CurrentPlayersMessage';
import { PlayerAddedMessage } from './events/PlayerAddedMessage';
import { PlayerRemovedMessage } from './events/PlayerRemovedMessage';
import { SetScoreMessage } from './events/SetScoreMessage';
import { GameApiService } from '../../networking/services/game-api/game-api.service';
import { UserJoined } from '../../networking/dtos/shared/userJoined';
import { UserDisconnected } from '../../networking/dtos/shared/userDisconnected';
import { isUserDisconnected } from '../../networking/dtos/shared/userDisconnected.guard';
import { isUserJoined } from '../../networking/dtos/shared/userJoined.guard';
import { PlayerType } from '../player-data/PlayerType';
import { isUserScore } from '../../networking/dtos/game/game-flow/userScore.guard';
import { UserScore } from '../../networking/dtos/game/game-flow/userScore';
import { Closed } from '../../networking/services/signal-r/events/Closed';
import { Connected } from '../../networking/services/signal-r/events/Connected';
import { Reconnecting } from '../../networking/services/signal-r/events/Reconnecting';
import { PlayerDataService } from '../player-data/player-data.service';

@Injectable({
  providedIn: null
})
export class ActivePlayersService 
{
    private currentPlayerData: PlayerData[] = [];

    private playerDataService: PlayerDataService = inject(PlayerDataService);

    private eventSubject: Subject<CurrentPlayersMessage> = new Subject<CurrentPlayersMessage>();

    private gameApiService: GameApiService = inject(GameApiService);

    public constructor()
    {
      this.gameApiService.ObserveUserConnectionEvent().subscribe
      (
        {
          next: this.OnUserConnectionEvent
        }
      );

      this.gameApiService.ObserveConnectionStatusEvent().subscribe
      (
        {
          next: this.OnConnectionStatusEvent
        }
      )

      this.gameApiService.ObserveGameFlowEvent()
        .pipe((filter((val) => isUserScore(val))))
        .subscribe(this.OnScoreEvent);
      
      if (this.gameApiService.ConnectionStatus == "Connected" && this.playerDataService.PlayerData.value != undefined)
      {
        this.Add(this.playerDataService.PlayerData.value);
      }
    }

    public get CurrentPlayers(): PlayerData[]
    {
      return [...this.currentPlayerData];
    }

    public ObserveCurrentPlayersEvent() : Observable<CurrentPlayersMessage>
    {
      return this.eventSubject.asObservable();
    }

    private OnScoreEvent = (value: UserScore): void =>
    {
      this.AddToScore(value.body.user.id, value.body.score);
    }

    private OnUserConnectionEvent = (value: UserDisconnected | UserJoined): void =>
    {
      if (isUserJoined(value))
      {
        let joined = value as UserJoined;

        this.Add({Username: joined.body.user.name, Id: joined.body.user.id, Score: 0});     
      }
      else if (isUserDisconnected(value))
      {
        let discon = value as UserDisconnected;

        this.Remove(discon.body.user.id);
      }
    }

    private OnConnectionStatusEvent = (value: Closed | Connected | Reconnecting) =>
    {
      if (this.playerDataService.PlayerData.value == undefined)
      {
        console.log("Received change of connection status event, but player data is not set!");
        return;
      }

      if (value instanceof Reconnecting)
      {
        return;
      }

      if (value instanceof Closed)
      {
        this.Remove(this.playerDataService.PlayerData.value.Id);
      }
      else if (value instanceof Connected)
      {
       this.Add(this.playerDataService.PlayerData.value);
      }
    }

    public AddToScore(playerId: string, newScore: number): boolean
    {
      let found: boolean = false;

      for(let i = 0; i < this.currentPlayerData.length; i++)
      {
        if (this.currentPlayerData[i].Id == playerId)
        {
          let oldScore = this.currentPlayerData[i].Score;

          this.currentPlayerData[i].Score += newScore;

          found = true;

          this.eventSubject.next(new SetScoreMessage(playerId, oldScore, newScore));
        }
      }

      return found;
    }

    private Add(newData: PlayerData): boolean
    { 
      // if data already exists  
      if (this.currentPlayerData.find((val: PlayerData) => IsEqual(newData, val)))
      {
        return false;
      }

      this.currentPlayerData.push(newData);
      this.eventSubject.next(new PlayerAddedMessage(newData));

      return true;
    }

    private Remove(userId: string): boolean
    {
      // if nothing to remove 
      let index: number = this.currentPlayerData.findIndex((val: PlayerData) => val.Id == userId);

      if (index == -1)
      {
        return false;
      }

      let pdToRemove = this.currentPlayerData[index];

      this.currentPlayerData.splice(index, 1);
      this.eventSubject.next(new PlayerRemovedMessage(pdToRemove));
      
      return true;
    }
}