import { inject, Injectable } from '@angular/core';
import { BehaviorSubject, filter } from 'rxjs';
import { PlayerData } from './PlayerData';
import { just, Maybe, none } from '@sweet-monads/maybe';

@Injectable({
  providedIn: null
})
export class PlayerDataService 
{
  public PlayerData: BehaviorSubject<PlayerData | undefined> = new BehaviorSubject<PlayerData | undefined>(undefined);
}
