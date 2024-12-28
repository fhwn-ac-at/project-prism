import { Injectable} from '@angular/core';
import { BehaviorSubject, Observer, Subscription } from 'rxjs';
import { ObservableArray } from '../../../lib/observable/ObservableArray/ObservableArray';
import { ConfigService } from "../config/config.service";
import { StrokeVM } from './StrokeVM';
import { Event } from '../../../lib/observable/ObservableArray/events/Event';
import { CountdownService } from '../countdown/countdown.service';
import { PlayerDataService } from '../player-data/player-data.service';
import { PlayerType } from '../player-data/PlayerType';

@Injectable(
{
  providedIn: null
})
export class CanvasStateService 
{
  private configService: ConfigService;
  private countdownService: CountdownService;
  private playerDataService: PlayerDataService;

  private strokes: ObservableArray<StrokeVM>;

  public constructor
  (
    configService: ConfigService, 
    countdownService: CountdownService,
    playerDataService: PlayerDataService
  )
  {
    this.configService = configService;
    this.countdownService = countdownService;
    this.playerDataService = playerDataService;

    this.strokes = new ObservableArray<StrokeVM>([]);

    this.StrokeWidth = new BehaviorSubject<number>(this.configService.configData.canvasOptions.strokeWidth);
    this.StrokeColor = new BehaviorSubject<string>(this.configService.configData.canvasOptions.strokeColor);
  }

  public StrokeWidth: BehaviorSubject<number>;
  public StrokeColor: BehaviorSubject<string>;

  public GetStrokes(): StrokeVM[]
  {
    return this.strokes.GetItems();
  }

  public SubscribeToStrokes(obs : Partial<Observer<Event>>) : Subscription
  {
    return this.strokes.SubscribeEvent(obs);
  }

  public PushStroke(stroke: StrokeVM): void
  {
    if (!this.CanDraw()) return;

    this.strokes.Push(stroke);
  }

  public ResetStrokes(): void
  {
    if (!this.CanDraw()) return;

    this.strokes.Clear();
  }

  private CanDraw(): boolean
  {
    if (this.playerDataService.PlayerData.value.isNone()) return false;

    return this.playerDataService.PlayerData.value.value.Role == PlayerType.Drawer && this.countdownService.IsRunning();
  }
}
