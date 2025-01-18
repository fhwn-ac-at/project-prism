import { inject, Injectable} from '@angular/core';
import { BehaviorSubject, Observer, Subject, Subscription } from 'rxjs';
import { ConfigService } from "../config/config.service";
import { StrokeVM } from './StrokeVM';
import { Position2d } from '../../../lib/Position2d';
import { StrokesEvent } from './events/StrokesEvent';
import { ClearedEvent } from './events/ClearedEvent';
import { StartedEvent } from './events/StartedEvent';
import { MovedEvent } from './events/MovedEvent';
import { ClosedEvent } from './events/ClosedEvent';
import { RemovedEvent } from './events/RemovedEvent';

@Injectable(
{
  providedIn: null
})
export class CanvasStateService 
{
  // services
  private configService: ConfigService = inject(ConfigService);

  // stroke data
  private strokes: StrokeVM[] = [];
  private openStroke: StrokeVM | null = null;

  // event subject
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

  // event stuff
  public StrokeWidth: BehaviorSubject<number> = new BehaviorSubject<number>(this.configService.configData.canvasOptions.strokeWidth);
  public StrokeColor: BehaviorSubject<string> = new BehaviorSubject<string>(this.configService.configData.canvasOptions.strokeColor);
  public SubscribeStrokesEvent(obs : Partial<Observer<StrokesEvent>>) : Subscription
  {
    return this.eventSubject.subscribe(obs);
  }

  // stroke API

  public ResetStrokes(): void
  {
    this.strokes = [];

    this.eventSubject.next(new ClearedEvent());
  }

  public StartStroke(startPos: Position2d): boolean
  {
    if (this.openStroke != null) return false;

    this.openStroke = {
      StrokeWidth: this.StrokeWidth.value,
      PathData: [startPos],
      Color: this.StrokeColor.value,
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
