import { inject, Injectable } from '@angular/core';
import { GameApiService } from '../../networking/services/game-api/game-api.service';
import { CanDrawService } from '../can-draw/can-draw.service';
import { StrokesContainer } from './StrokesContainer';
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
import { CanvasOptionsService } from '../canvas-options/canvas-options.service';
import { isLineTo } from '../../networking/dtos/game/drawing/lineTo.guard';
import { StrokeVM } from './StrokeVM';
import { filter, Observable } from 'rxjs';
import { StrokesEvent } from './events/StrokesEvent';
import { isMoveTo } from '../../networking/dtos/game/drawing/moveTo.guard';
import { isSearchedWord } from '../../networking/dtos/game/game-flow/searchedWord.guard';
import { SelectWord } from '../../networking/dtos/game/game-flow/selectWord';
import { isNextRound } from '../../networking/dtos/game/game-flow/nextRound.guard';
import { isGameEnded } from '../../networking/dtos/game/game-flow/gameEnded.guard';
import { NextRound } from '../../networking/dtos/game/game-flow/nextRound';
import { GameEnded } from '../../networking/dtos/game/game-flow/gameEnded';
import { SetDrawer } from '../../networking/dtos/game/game-flow/setDrawer';
import { SetNotDrawer } from '../../networking/dtos/game/game-flow/setNotDrawer';
import { isSetDrawer } from '../../networking/dtos/game/game-flow/setDrawer.guard';
import { isSetNotDrawer } from '../../networking/dtos/game/game-flow/setNotDrawer.guard';

@Injectable({
  providedIn: null,
})
export class StrokesService 
{
  private gameApi: GameApiService = inject(GameApiService);

  private canvasOptions: CanvasOptionsService = inject(CanvasOptionsService);

  private canDraw: CanDrawService = inject(CanDrawService);

  private strokesContainer: StrokesContainer = new StrokesContainer();

  public constructor()
  {
    this.gameApi.ObserveDrawingEvent()
      .subscribe(this.OnDrawingEvent);

    this.gameApi.ObserveGameFlowEvent()
      .pipe(filter((val) => isSetDrawer(val) || isSetNotDrawer(val)))
      .subscribe(this.OnNextRoundOrGameEnded);
  }

  public ObserveStrokesEvent(): Observable<StrokesEvent>
  {
    return this.strokesContainer.ObserveStrokesEvent();
  }

  public get Strokes(): StrokeVM[]
  {
    return this.strokesContainer.Strokes;
  }

  // stroke API
  public async ResetStrokes()
  {
    if (!this.canDraw.IsDrawer())
    {
      return;
    }

    this.strokesContainer.ResetStrokes();

    return this.gameApi.SendClear();
  }

  public async StartStroke(startPos: Position2d, width: number, color: string)
  {
    if (!this.canDraw.IsDrawer())
    {
      return false;
    }

    const started: boolean = this.strokesContainer.StartStroke(startPos, width, color);
 
    if (!started) return;

    return this.gameApi.SendMoveTo(startPos);
  }

  public async MoveStroke(pos: Position2d, color: string)
  {
    if (!this.canDraw.IsDrawer())
    {
      return;
    }

    const moved: boolean = this.strokesContainer.MoveStroke(pos);

    if (!moved) return;

    return this.gameApi.SendLineTo(pos, color);
  }

  public async EndStroke()
  {
    if (!this.canDraw.IsDrawer())
    {
      return false;
    }

    const ended = this.strokesContainer.EndStroke();

    if(!ended) return;

    return this.gameApi.SendClosePath();
  }

  public async Undo()
  {
    if (!this.canDraw.IsDrawer())
    {
      return;
    }

    this.strokesContainer.Undo();

    return this.gameApi.SendUndo();
  }
  
  private OnNextRoundOrGameEnded = (_: SetDrawer | SetNotDrawer) =>
  {
    this.strokesContainer.ResetStrokes();
  }

  private OnDrawingEvent = (value: BackgroundColor | Clear | ClosePath | DrawingSizeChanged | LineTo | MoveTo | Point | Undo) =>
  {
    if (isClear(value))
    {
      this.strokesContainer.ResetStrokes();
    }
    else if (isClosePath(value))
    {
      this.strokesContainer.EndStroke();
    }
    else if (isLineTo(value))
    {
      let v = value as LineTo;

      this.strokesContainer.MoveStroke(v.body.point);
    }
    else if (isUndo(value))
    {
      this.strokesContainer.Undo();
    }
    else if (isMoveTo(value))
    {
      let v = value as MoveTo;

      this.strokesContainer.StartStroke(v.body.point, this.canvasOptions.StrokeWidth.value, this.canvasOptions.StrokeColor.value)
    }
    else if (isDrawingSizeChanged(value))
    {
      let v = value as DrawingSizeChanged;
      this.canvasOptions.StrokeWidth.next(v.body.size);
    }
    else
    {
      console.log("Other data type in drawing event received:" + value);
    }
  }
}
