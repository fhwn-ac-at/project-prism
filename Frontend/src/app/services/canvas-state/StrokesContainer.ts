import { inject, Injectable} from '@angular/core';
import { BehaviorSubject, Observable, Observer, Subject, Subscription } from 'rxjs';
import { ConfigService } from "../config/config.service";
import { StrokeVM } from './StrokeVM';
import { Position2d } from '../../../lib/Position2d';
import { StrokesEvent } from './events/StrokesEvent';
import { ClearedEvent } from './events/ClearedEvent';
import { StartedEvent } from './events/StartedEvent';
import { MovedEvent } from './events/MovedEvent';
import { ClosedEvent } from './events/ClosedEvent';
import { RemovedEvent } from './events/RemovedEvent';

export class StrokesContainer 
{
  private strokes: StrokeVM[] = [];
  private openStroke: StrokeVM | null = null;

  private eventSubject: Subject<StrokesEvent> = new Subject<StrokesEvent>();

  public get Strokes(): StrokeVM[]
  {
    const strokes = [...this.strokes];

    if(this.openStroke != null)
    {
      strokes.push(this.openStroke);
    }

    return strokes;
  }

  // event
  public ObserveStrokesEvent() : Observable<StrokesEvent>
  {
    return this.eventSubject.asObservable();
  }

  // stroke API
  public ResetStrokes(): void
  {
    this.strokes = [];

    this.eventSubject.next(new ClearedEvent());
  }

  public StartStroke(startPos: Position2d, width: number, color: string): boolean
  {
    if (this.openStroke != null) return false;

    this.openStroke = {
      StrokeWidth: width,
      PathData: [startPos],
      Color: color,
    }

    this.eventSubject.next(new StartedEvent(this.openStroke));
    return true;
  }

  public MoveStroke(pos: Position2d): boolean
  {
    if (this.openStroke == null) return false;

    this.openStroke.PathData.push(pos);
    
    this.eventSubject.next(new MovedEvent(this.openStroke));

    return true;
  }

  public EndStroke(): boolean
  {
    if (this.openStroke == null) return false;

    this.strokes.push(this.openStroke);

    this.eventSubject.next(new ClosedEvent(this.openStroke));

    this.openStroke = null;

    return true;
  }

  public Undo(): void
  {
    const popped: StrokeVM | undefined = this.strokes.pop();

    if(!popped) return;

    this.eventSubject.next(new RemovedEvent(popped));
  }
}