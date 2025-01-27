import { inject, Injectable } from '@angular/core';
import { ConfigService } from '../config/config.service';
import { BehaviorSubject, filter } from 'rxjs';
import { GameApiService } from '../../networking/services/game-api/game-api.service';
import { isDrawingSizeChanged } from '../../networking/dtos/game/drawing/drawingSizeChanged.guard';
import { isBackgroundColor } from '../../networking/dtos/game/drawing/backgroundColor.guard';

@Injectable({
  providedIn: null
})
export class CanvasOptionsService 
{
  private configService: ConfigService = inject(ConfigService);
  private gameApi: GameApiService = inject(GameApiService);

  public constructor()
  {
    this.StrokeWidth.asObservable().
    subscribe
    (
      (val) => this.gameApi.SendDrawingSize(val)
    );
    this.gameApi.ObserveDrawingEvent().pipe(filter(isDrawingSizeChanged)).subscribe((val) => this.StrokeWidth.next(val.body.size))

    this.StrokeColor.asObservable()
    .subscribe
    (
      (val) => this.gameApi.SendBackgroundColor(val)
    );
    this.gameApi.ObserveDrawingEvent().pipe(filter(isBackgroundColor)).subscribe((val) => this.StrokeColor.next(val.body.color.hexString))
  }

  public StrokeWidth: BehaviorSubject<number> = new BehaviorSubject<number>(this.configService.configData.canvasOptions.strokeWidth);
  public StrokeColor: BehaviorSubject<string> = new BehaviorSubject<string>(this.configService.configData.canvasOptions.strokeColor);
}
