import { Injectable } from '@angular/core';
import { Maybe, none } from '@sweet-monads/maybe';
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: null
})
export class GameIdService 
{
  public GameId: BehaviorSubject<Maybe<string>> = new BehaviorSubject<Maybe<string>>(none());
}
