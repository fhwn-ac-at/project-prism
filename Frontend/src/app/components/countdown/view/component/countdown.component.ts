import { Component, OnDestroy } from '@angular/core';
import { CountdownService } from '../../../../services/countdown/countdown.service';
import { MatCard } from '@angular/material/card';
import { NgIf } from '@angular/common';
import { CountdownState } from '../CountdownState/CountdownState';
import { CountdownNotStartedState } from '../CountdownState/CountdownNotStartedState';
import { CountdownRunningState } from '../CountdownState/CountdownRunningState';
import { CountdownEvent } from '../../../../services/countdown/CountdownEvent';
import { CountdownFinishedState } from '../CountdownState/CountdownFinishedState';
import { InstanceofPipe } from "../../../../pipes/instanceof.pipe";
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-countdown',
  imports: [MatCard, NgIf, InstanceofPipe],
  templateUrl: './countdown.component.html',
  styleUrl: './countdown.component.css'
})
export class CountdownComponent implements OnDestroy
{
  private countDownService: CountdownService;
  private sub1: Subscription;

  public constructor(countdownService: CountdownService)
  {
    this.countDownService = countdownService;

    this.sub1 = this.countDownService.ObserveTimerEvent().
    subscribe
    (
      {
        next: (event) => 
        {
          this.OnCountdownStateUpdated(event);
        },
      }
    );
  }

  public CountdownState: CountdownState = new CountdownNotStartedState();

  public readonly CdNotStarted = CountdownNotStartedState;
  public readonly CdRunning = CountdownRunningState;
  public readonly CdFinished = CountdownFinishedState;

  ngOnDestroy(): void 
  {
    this.sub1.unsubscribe();
  }

  private OnCountdownStateUpdated(event: CountdownEvent)
  {
    if (event.TimeLeft == 0) 
    {
        this.CountdownState = new CountdownFinishedState();
    }
    else
    {
      this.CountdownState = new CountdownRunningState(event.TimeLeft);
    }
  }
}
