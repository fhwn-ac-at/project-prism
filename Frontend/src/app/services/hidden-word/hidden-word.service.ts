import { inject, Injectable } from '@angular/core';
import { WordPart } from './WordPart';
import { StringToWordPartsConverter } from './StringToWordPartsConverter';
import { filter, Observer, ReplaySubject, Subject } from 'rxjs';
import  { Maybe } from '@sweet-monads/maybe';
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

  private wordParts: WordPart[] | undefined;

  private eventSubject: ReplaySubject<HiddenWordEvent> = new ReplaySubject<HiddenWordEvent>(1);

  public constructor() 
  {
    this.gameApi.ObserveGameFlowEvent()
      .pipe<SearchedWord>(filter((val) => isSearchedWord(val)))
      .subscribe(this.OnSearchedWordEvent);
  }

  public SubscribeWordEvent(sub: Partial<Observer<HiddenWordEvent>>)
  {
    this.eventSubject.subscribe(sub);
  } 

  public GetWord(): Maybe<string>[] | undefined
  {
    if (this.wordParts == undefined) return undefined;

    return StringToWordPartsConverter.ConvertToPublicRepresentation(this.wordParts);
  }

  private SetWord(word: string): void
  {
    this.wordParts = StringToWordPartsConverter.ConvertToWordPart(word);

    this.eventSubject.next
    (
      new HiddenWordEvent(StringToWordPartsConverter.ConvertToPublicRepresentation(this.wordParts))
    );
  }
  
  private Reveal(index: number)
  {
    if (this.wordParts == undefined) return;
    if (index - Math.trunc(index) != 0) throw new Error("Index must be integer");
    if (index < 0  || index >= this.wordParts.length) throw new Error("Must be in range!");

    this.wordParts[index] = new WordPart(this.wordParts[index].Char, true);

    this.eventSubject.next
    (
      new HiddenWordEvent(StringToWordPartsConverter.ConvertToPublicRepresentation(this.wordParts))
    );
  }

  private OnSearchedWordEvent = (value: SearchedWord) =>
  {
    if (this.wordParts === undefined)
    {
      this.SetWord(value.body.word);
    }
    else
    {
      this.RevealBasedOnWordString(value.body.word);
    }
  }
  private RevealBasedOnWordString(word: string) 
  {
    for(let i = 0; i < word.length; i++)
    {
      if(word[i] == "_")
      {
        continue;
      }

      this.Reveal(i);
    }
  }
}

