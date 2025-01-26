import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: null
})
export class GameIdService 
{
  public GameId: BehaviorSubject<string | undefined> = new BehaviorSubject<string | undefined>(undefined);
}
