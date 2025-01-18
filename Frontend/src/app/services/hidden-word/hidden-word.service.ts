import { Injectable } from '@angular/core';
import { WordPart } from './WordPart';
import { StringToWordPartsConverter } from './StringToWordPartsConverter';
import { Observer, ReplaySubject, Subject } from 'rxjs';
import  { Maybe } from '@sweet-monads/maybe';
import { HiddenWordEvent } from './HiddenWordEvent';

@Injectable({
  providedIn: null
})
export class HiddenWordService 
{
  private wordParts: WordPart[] | undefined;

  private eventSubject: ReplaySubject<HiddenWordEvent> = new ReplaySubject<HiddenWordEvent>(1);

  public constructor() 
  { }

  public SubscribeWordEvent(sub: Partial<Observer<HiddenWordEvent>>)
  {
    this.eventSubject.subscribe(sub);
  } 

  public GetWord(): Maybe<string>[] | undefined
  {
    if (this.wordParts == undefined) return undefined;

    return StringToWordPartsConverter.ConvertToPublicRepresentation(this.wordParts);
  }

  public SetWord(word: string): void
  {
    this.wordParts = StringToWordPartsConverter.ConvertToWordPart(word);

    this.eventSubject.next(
      new HiddenWordEvent(StringToWordPartsConverter.ConvertToPublicRepresentation(this.wordParts))
    );
  }
  
  public Reveal(index: number)
  {
    if (this.wordParts == undefined) return;

    if (index - Math.trunc(index) != 0) throw new Error("Index must be integer");

    if (index < 0  || index >= this.wordParts.length) throw new Error("Must be in range!");

    this.wordParts[index] = new WordPart(this.wordParts[index].Char, true);

    this.eventSubject.next(
      new HiddenWordEvent(StringToWordPartsConverter.ConvertToPublicRepresentation(this.wordParts))
    );
  }
}

