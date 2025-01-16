import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { PlayerData } from './PlayerData';
import { Maybe, none } from '@sweet-monads/maybe';

@Injectable({
  providedIn: null
})
export class PlayerDataService 
{
  public PlayerData: BehaviorSubject<Maybe<PlayerData>> = new BehaviorSubject<Maybe<PlayerData>>(none());
}
