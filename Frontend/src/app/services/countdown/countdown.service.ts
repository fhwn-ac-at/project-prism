import { Injectable } from '@angular/core';
import { map, Observer, Subject, Subscription, takeWhile, timer } from 'rxjs';
import { CountdownEvent } from './CountdownEvent';

@Injectable({
  providedIn: null
})
export class CountdownService 
{
  private timerSubject: Subject<CountdownEvent> = new Subject<CountdownEvent>();

  private timerSubscription: Subscription | null = null;

  public SubscribeEvent(observer: Partial<Observer<CountdownEvent>>): Subscription
  {
    return this.timerSubject.subscribe(observer);
  }

  public IsRunning() : boolean
  {
    return this.timerSubscription != null;
  }


  public StartTimer(startNumber: number, delayInMs: number): void
  {
    if (!Number.isInteger(startNumber) || startNumber < 0) 
    {
        throw new Error("Invalid startNumber");
    }

    if (delayInMs <= 0) 
    {
        throw new Error("Invalid delay");
    }

    if (this.timerSubscription != null) 
    {
        return;
    }

    this.timerSubscription = timer(0, delayInMs).pipe
    (
      map((num: number) => startNumber - num),
      takeWhile((num) => num >= 0)
    )
    .subscribe
    (
      {
        next: (timeLeft) => 
        {
          // reset if this is last.
          if(timeLeft == 0) this.timerSubscription = null;

          this.timerSubject.next(new CountdownEvent(timeLeft));
        }
      }
    );
  }

  public StopTimer(): void
  {
    if (this.timerSubscription == null) 
    {
        return;
    }

    this.timerSubscription.unsubscribe();
    this.timerSubscription = null;
  }
}