import { ChangeDetectionStrategy, Component, input, InputSignal, OnChanges, OnInit, SimpleChanges } from '@angular/core';
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
export class DrawableCanvasComponent implements OnChanges
{
  public DrawableCanvasWidth: InputSignal<number> = input.required();
  public DrawableCanvasHeight: InputSignal<number> = input.required();

  private canvasStateService: CanvasStateService;

  public CalculateCanvasWidth = 0;
  public CalculateCanvasHeight = 0;

  public constructor(canvasStateService: CanvasStateService)
  {
   this.canvasStateService = canvasStateService;
  }

  ngOnChanges(changes: SimpleChanges): void {
    this.CalculateCanvasWidth=this.DrawableCanvasWidth() * 0.98;
    this.CalculateCanvasHeight=this.DrawableCanvasHeight() * 0.80;
  }
}
