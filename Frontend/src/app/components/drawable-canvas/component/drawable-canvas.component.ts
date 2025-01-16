import { Component, input, InputSignal, OnInit } from '@angular/core';
import { CanvasStateService } from '../../../services/canvas-state/canvas-state.service';
import { CanvasComponent } from "../../canvas/view/component/canvas.component";
import { CanvasOptionsComponent } from "../../canvas-options/component/canvas-options.component";
import { MatCardModule } from '@angular/material/card';

@Component({
  selector: 'app-drawable-canvas',
  imports: [CanvasComponent, CanvasOptionsComponent, MatCardModule],
  templateUrl: './drawable-canvas.component.html',
  styleUrl: './drawable-canvas.component.css',
  providers: [{provide: CanvasStateService , useClass: CanvasStateService}]
})
export class DrawableCanvasComponent
{
  private canvasStateService: CanvasStateService;

  public constructor(canvasStateService: CanvasStateService)
  {
   this.canvasStateService = canvasStateService;
  }

}
