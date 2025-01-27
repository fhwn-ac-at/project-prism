import { Component, input, InputSignal, OnInit } from '@angular/core';
import { StrokesContainer } from '../../../services/canvas-state/StrokesContainer';
import { CanvasComponent } from "../../canvas/view/component/canvas.component";
import { CanvasOptionsComponent } from "../../canvas-options/component/canvas-options.component";
import { MatCardModule } from '@angular/material/card';

@Component({
  selector: 'app-drawable-canvas',
  imports: [CanvasComponent, CanvasOptionsComponent, MatCardModule],
  templateUrl: './drawable-canvas.component.html',
  styleUrl: './drawable-canvas.component.css',
})
export class DrawableCanvasComponent
{

}
