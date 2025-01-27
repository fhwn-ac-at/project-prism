import { inject, Injectable } from '@angular/core';
import { GameApiService } from '../../networking/services/game-api/game-api.service';
import { CanDrawService } from '../can-draw/can-draw.service';
import { CanvasStateService } from './canvas-state.service';
import { Position2d } from '../../../lib/Position2d';
import { BackgroundColor } from '../../networking/dtos/game/drawing/backgroundColor';
import { Clear } from '../../networking/dtos/game/drawing/clear';
import { isClear } from '../../networking/dtos/game/drawing/clear.guard';
import { ClosePath } from '../../networking/dtos/game/drawing/closePath';
import { isClosePath } from '../../networking/dtos/game/drawing/closePath.guard';
import { DrawingSizeChanged } from '../../networking/dtos/game/drawing/drawingSizeChanged';
import { isDrawingSizeChanged } from '../../networking/dtos/game/drawing/drawingSizeChanged.guard';
import { LineTo } from '../../networking/dtos/game/drawing/lineTo';
import { MoveTo } from '../../networking/dtos/game/drawing/moveTo';
import { Point } from '../../networking/dtos/game/drawing/point';
import { Undo } from '../../networking/dtos/game/drawing/undo';
import { isUndo } from '../../networking/dtos/game/drawing/undo.guard';
import { StrokeVM } from './StrokeVM';
import { BehaviorSubject, Observer, Subscription } from 'rxjs';
import { StrokesEvent } from './events/StrokesEvent';

@Injectable({
  providedIn: null,
})
export class CanvasWrapperService 
{
  private gameApi: GameApiService = inject(GameApiService);
  private canDraw: CanDrawService = inject(CanDrawService);
  private canvasService: CanvasStateService = inject(CanvasStateService);
  
  public constructor()
  {
    this.gameApi.ObserveDrawingEvent().subscribe(this.OnDrawingEvent);
  }

  // stroke API
  public async ResetStrokes()
  {
    if (!this.canDraw.IsDrawer())
    {
      return;
    }

    this.canvasService.ResetStrokes();

    return this.gameApi.SendClear();
  }

  public async StartStroke(startPos: Position2d)
  {
    if (!this.canDraw.IsDrawer())
    {
      return false;
    }

    this.canvasService.StartStroke(startPos);

    return this.gameApi.SendMoveTo(startPos);
  }

  public async MoveStroke(pos: Position2d)
  {
    if (!this.canDraw.IsDrawer())
    {
      return;
    }

    this.canvasService.MoveStroke(pos);

    return this.gameApi.SendLineTo(pos, this.canvasService.StrokeColor.value);
  }

  public async EndStroke()
  {
    if (!this.canDraw.IsDrawer())
      {
        return false;
      }

    this.canvasService.EndStroke();

    return this.gameApi.SendClosePath();
  }

  public async Undo()
  {
    if (!this.canDraw.IsDrawer())
    {
      return;
    }

    this.canvasService.Undo();

    return this.gameApi.SendUndo();
  }
  
  private OnDrawingEvent = (value: BackgroundColor | Clear | ClosePath | DrawingSizeChanged | LineTo | MoveTo | Point | Undo) =>
  {
    if (isClear(value))
    {
      this.canvasService.ResetStrokes();
    }
    else if (isClosePath(value))
    {
      this.canvasService.EndStroke();
    }
    else if (isDrawingSizeChanged(value))
    {
      let v = value as DrawingSizeChanged;
      this.canvasService.StrokeWidth.next(v.body.size);
    }
    else if (isUndo(value))
    {
      this.canvasService.Undo();
    }
    else{
      console.log("Other data type in drawing event received:" + value);
    }
  }
}
