import { Injectable } from '@angular/core';
import { PlayerData } from '../player-data/PlayerData';
import { ObservableArray } from '../../../lib/observable/ObservableArray/ObservableArray';

@Injectable({
  providedIn: null
})
export class CurrentPlayersService 
{
    private currentPlayerData: ObservableArray<PlayerData>;
  
    public constructor() 
    {
      this.currentPlayerData = new ObservableArray<PlayerData>([]);
    }
  
    public get CurrentPlayerData(): ObservableArray<PlayerData>
    {
      return this.currentPlayerData;
    }
}
