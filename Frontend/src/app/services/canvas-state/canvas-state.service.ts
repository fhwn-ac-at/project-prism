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
import { GameApiService } from '../../networking/services/game-api/game-api.service';
import { CanDrawService } from '../can-draw/can-draw.service';
import { BackgroundColor } from '../../networking/dtos/game/drawing/backgroundColor';
import { Clear } from '../../networking/dtos/game/drawing/clear';
import { ClosePath } from '../../networking/dtos/game/drawing/closePath';
import { DrawingSizeChanged } from '../../networking/dtos/game/drawing/drawingSizeChanged';
import { LineTo } from '../../networking/dtos/game/drawing/lineTo';
import { MoveTo } from '../../networking/dtos/game/drawing/moveTo';
import { Point } from '../../networking/dtos/game/drawing/point';
import { Undo } from '../../networking/dtos/game/drawing/undo';
import { isClear } from '../../networking/dtos/game/drawing/clear.guard';
import { isClosePath } from '../../networking/dtos/game/drawing/closePath.guard';
import { isDrawingSizeChanged } from '../../networking/dtos/game/drawing/drawingSizeChanged.guard';
import { isUndo } from '../../networking/dtos/game/drawing/undo.guard';

@Injectable(
{
  providedIn: null
})
export class CanvasStateService 
{
  // services
  private configService: ConfigService = inject(ConfigService);
  private gameApi: GameApiService = inject(GameApiService);
  private canDraw: CanDrawService = inject(CanDrawService);

  // stroke data
  private strokes: StrokeVM[] = [];
  private openStroke: StrokeVM | null = null;

  // event subject
  private eventSubject: Subject<StrokesEvent> = new Subject<StrokesEvent>();

  public constructor()
  {
   this.gameApi.ObserveDrawingEvent().subscribe(this.OnDrawingEvent); 
  }

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

    if (this.canDraw.IsDrawer())
    {
      this.gameApi.SendClear();
    }
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

    if (this.canDraw.IsDrawer())
    {
      this.gameApi.SendMoveTo(startPos);
    }

    return true;
  }

  public MoveStroke(pos: Position2d): boolean
  {
    if (this.openStroke == null) return false;

    this.openStroke.PathData.push(pos);
    
    this.eventSubject.next(new MovedEvent(this.openStroke));

    if (this.canDraw.IsDrawer())
    {
      this.gameApi.SendLineTo(pos, this.StrokeColor.value);
    }

    return true;
  }

  public EndStroke(): boolean
  {
    if (this.openStroke == null) return false;

    this.strokes.push(this.openStroke);

    this.eventSubject.next(new ClosedEvent(this.openStroke));

    this.openStroke = null;

    if (this.canDraw.IsDrawer())
    {
      this.gameApi.SendClosePath();
    }

    return true;
  }

  public Undo(): void
  {
    const popped: StrokeVM | undefined = this.strokes.pop();

    if(!popped) return;

    this.eventSubject.next(new RemovedEvent(popped));

    if (this.canDraw.IsDrawer())
    {
      this.gameApi.SendUndo();
    }
  }

  private OnDrawingEvent(value: BackgroundColor | Clear | ClosePath | DrawingSizeChanged | LineTo | MoveTo | Point | Undo) 
  {
    if (isClear(value))
    {
      this.ResetStrokes();
    }
    else if (isClosePath(value))
    {
      this.EndStroke();
    }
    else if (isDrawingSizeChanged(value))
    {
      let v = value as DrawingSizeChanged;
      this.StrokeWidth.next(v.body.size);
    }
    else if (isUndo(value))
    {
      this.Undo();
    }
    else{
      console.log("Other data type in drawing event received:" + value);
    }
  }
}
