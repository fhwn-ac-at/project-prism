import { Component } from '@angular/core';
import { CanvasStateService } from "../../../services/canvas-state/canvas-state.service";
import {ColorPickerModule} from "ngx-color-picker";
import {MatSliderModule} from "@angular/material/slider";
import {MatButtonModule} from "@angular/material/button";
import { MatCardModule } from '@angular/material/card';
import { MatDividerModule } from '@angular/material/divider';

@Component({
  selector: 'app-canvas-options',
  imports: [ColorPickerModule, MatSliderModule, MatButtonModule, MatCardModule, MatDividerModule],
  templateUrl: './canvas-options.component.html',
  styleUrl: './canvas-options.component.css',
})
export class CanvasOptionsComponent
{
  private canvasState: CanvasStateService;

  public constructor(canvasService: CanvasStateService)
  {
    this.canvasState = canvasService;

    this.Color = this.canvasState.StrokeColor.value;

    this.StrokeSize = this.canvasState.StrokeWidth.value;
  }

  public Color: string;

  public StrokeSize: number;

  public OnSizeValueChanged(_: number) 
  {
    this.canvasState.StrokeWidth.next(this.StrokeSize);
  }

  public OnColorChanged(_: string) 
  {
    this.canvasState.StrokeColor.next(this.Color);
  }

  public OnCanvasResetClicked(_: MouseEvent) 
  {
    this.canvasState.ResetStrokes();
  }
}