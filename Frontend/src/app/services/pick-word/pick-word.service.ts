import { Injectable } from '@angular/core';
import { PartialObserver, Subject, Subscription } from 'rxjs';
import { WordPickedEvent } from './events/WordPicked';
import { WordsToPickEvent } from './events/WordsToPick';

@Injectable({
  providedIn: null,
})
export class PickWordService 
{
  private wordPickedSubject: Subject<WordPickedEvent> = new Subject<WordPickedEvent>();
  private wordsToPickSubject: Subject<WordsToPickEvent> = new Subject<WordsToPickEvent>();

  public SubscribeWordPicked(obs: PartialObserver<WordPickedEvent>): Subscription
  {
    return this.wordPickedSubject.subscribe(obs);
  }

  public SubscribeOnWordToPick(obs: PartialObserver<WordsToPickEvent>): Subscription
  {
    return this.wordsToPickSubject.subscribe(obs);
  }

  public TriggerWordPicked(word: string)
  {
    this.wordPickedSubject.next(new WordPickedEvent(word));
  }

  public LetUserPickWord(words: string[]): void
  {
    this.wordsToPickSubject.next(new WordsToPickEvent(words));
  }
}
