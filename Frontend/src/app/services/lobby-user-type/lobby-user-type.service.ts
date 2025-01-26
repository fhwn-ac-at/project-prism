import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { LobbyUserType } from './LobbyUserType';

@Injectable({
  providedIn: null
})
export class LobbyUserTypeService 
{
  public LobbyUserType: BehaviorSubject<LobbyUserType> = new BehaviorSubject<LobbyUserType>(LobbyUserType.Unknown);
}
