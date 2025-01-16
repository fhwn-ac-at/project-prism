import { Injectable } from '@angular/core';
import { IsEqual, PlayerData } from '../player-data/PlayerData';
import { PartialObserver, Subject, Subscription } from 'rxjs';
import { CurrentPlayersMessage } from './events/CurrentPlayersMessage';
import { PlayerAddedMessage } from './events/PlayerAddedMessage';
import { PlayerRemovedMessage } from './events/PlayerLeftMessage';

@Injectable({
  providedIn: null
})
export class ActivePlayersService 
{
    private currentPlayerData: PlayerData[];
  
    private eventSubject: Subject<CurrentPlayersMessage>;

    public constructor() 
    {
      this.currentPlayerData = [];
      this.eventSubject = new Subject<CurrentPlayersMessage>();
    }
  
    public get CurrentPlayers(): PlayerData[]
    {
      return [...this.currentPlayerData];
    }

    public Subscribe(obs: PartialObserver<CurrentPlayersMessage>) : Subscription
    {
      return this.eventSubject.subscribe(obs);
    }

    public Add(newData: PlayerData): boolean
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

    public Remove(pdToRemove: PlayerData): boolean
    {
      // if nothing to remove 
      let index: number = this.currentPlayerData.findIndex((val: PlayerData) => IsEqual(pdToRemove, val));

      if (index == -1)
      {
        return false;
      }

      this.currentPlayerData.splice(index, 1);
      this.eventSubject.next(new PlayerRemovedMessage(pdToRemove));
      
      return true;
    }
}
