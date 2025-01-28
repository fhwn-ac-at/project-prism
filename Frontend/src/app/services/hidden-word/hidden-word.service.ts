import { inject, Injectable } from '@angular/core';
import { WordPart } from './WordPart';
import { StringToWordPartsConverter } from './StringToWordPartsConverter';
import { filter, Observable, ReplaySubject, Subject } from 'rxjs';
import { HiddenWordEvent } from './HiddenWordEvent';
import { GameApiService } from '../../networking/services/game-api/game-api.service';
import { isSearchedWord } from '../../networking/dtos/game/game-flow/searchedWord.guard';
import { SearchedWord } from '../../networking/dtos/game/game-flow/searchedWord';

@Injectable({
  providedIn: null
})
export class HiddenWordService 
{
  private gameApi: GameApiService = inject(GameApiService);
  private eventSubject: Subject<HiddenWordEvent> = new Subject<HiddenWordEvent>();

  public constructor() 
  {
    this.gameApi.ObserveGameFlowEvent()
      .pipe<SearchedWord>(filter((val) => isSearchedWord(val)))
      .subscribe(this.OnSearchedWordEvent);
  }

  public ObserveWordEvent(): Observable<HiddenWordEvent>
  {
    return this.eventSubject.asObservable();
  } 

  public WordParts: WordPart[] | undefined;

  private SetWord(word: string): void
  {
    this.WordParts = StringToWordPartsConverter.ConvertToWordPart(word);

    this.eventSubject.next
    (
      new HiddenWordEvent(this.WordParts)
    );
  }

  private OnSearchedWordEvent = (value: SearchedWord) =>
  {
    this.SetWord(value.body.word);
  }
}

