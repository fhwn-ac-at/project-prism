import { inject, Injectable } from '@angular/core';
import { filter, Observable, Subject } from 'rxjs';
import { WordsToPickEvent } from './events/WordsToPick';
import { GameApiService } from '../../networking/services/game-api/game-api.service';
import { isSetDrawer } from '../../networking/dtos/game/game-flow/setDrawer.guard';
import { SetDrawer } from '../../networking/dtos/game/game-flow/setDrawer';

@Injectable({
  providedIn: null,
})
export class PickWordService 
{
  private gameApiService: GameApiService = inject(GameApiService);

  private wordsToPickSubject: Subject<WordsToPickEvent> = new Subject<WordsToPickEvent>();
  
  public constructor()
  {
    this.gameApiService.ObserveGameFlowEvent().
      pipe(filter((val) => isSetDrawer(val)))
      .subscribe(this.OnSetDrawer);
  }

  public ObserveWordsToPickEvent(): Observable<WordsToPickEvent>
  {
    return this.wordsToPickSubject.asObservable();
  }

  public SendWordPicked(word: string): Promise<void>
  {
    return this.gameApiService.SendSelectedWord(word);
  }

  private OnSetDrawer = (val: SetDrawer): void =>
  {
    this.wordsToPickSubject.next({Words: val.body.words});
  }
}