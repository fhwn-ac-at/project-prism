import { Component, inject, OnDestroy } from '@angular/core';
import { StrokesContainer } from "../../../services/canvas-state/StrokesContainer";
import {ColorPickerModule} from "ngx-color-picker";
import {MatSliderModule} from "@angular/material/slider";
import {MatButtonModule} from "@angular/material/button";
import { MatCardModule } from '@angular/material/card';
import { MatDividerModule } from '@angular/material/divider';
import { CanDrawService } from '../../../services/can-draw/can-draw.service';
import { PlayerTypeService } from '../../../services/player-type/player-type.service';
import { PlayerType } from '../../../services/player-data/PlayerType';
import { CanvasOptionsService } from '../../../services/canvas-options/canvas-options.service';
import { StrokesService } from '../../../services/canvas-state/strokes.service';
import { Subscription, timer } from 'rxjs';

@Component({
  selector: 'app-canvas-options',
  imports: [ColorPickerModule, MatSliderModule, MatButtonModule, MatCardModule, MatDividerModule],
  templateUrl: './canvas-options.component.html',
  styleUrl: './canvas-options.component.css',
})
export class CanvasOptionsComponent implements OnDestroy
{
  private strokes: StrokesService = inject(StrokesService);
  public canvasOptions: CanvasOptionsService = inject(CanvasOptionsService);

  private sub1: Subscription;
  private sub2: Subscription;

  public constructor()
  {
    this.Color = this.canvasOptions.StrokeColor.value;
    this.StrokeSize = this.canvasOptions.StrokeWidth.value;

    this.sub1 = this.canvasOptions.StrokeColor.subscribe((val) => {this.Color = val});
    this.sub2 = this.canvasOptions.StrokeWidth.subscribe((val) => this.StrokeSize = val);
  }

  public PlayerTypeService: PlayerTypeService = inject(PlayerTypeService);
  
  public PlayerType: typeof PlayerType = PlayerType;

  public Color: string;
  public StrokeSize: number;

  ngOnDestroy(): void 
  {
    this.sub1.unsubscribe();
    this.sub2.unsubscribe();
  }

  public OnSizeValueChanged(_: number) 
  {
    this.canvasOptions.StrokeWidth.next(this.StrokeSize);
  }

  public OnColorChanged(_: string) 
  {
    this.canvasOptions.StrokeColor.next(this.Color);
  }

  public OnCanvasResetClicked(_: MouseEvent) 
  {
    this.strokes.ResetStrokes();
  }

  public OnUndoButtonClicked(_: MouseEvent) 
  {
    this.strokes.Undo();
  }
}