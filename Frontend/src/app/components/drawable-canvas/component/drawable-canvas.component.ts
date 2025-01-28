import { ChangeDetectionStrategy, Component, input, InputSignal, OnChanges, OnInit, SimpleChanges } from '@angular/core';
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
export class DrawableCanvasComponent implements OnChanges
{
  public DrawableCanvasWidth: InputSignal<number> = input.required();
  public DrawableCanvasHeight: InputSignal<number> = input.required();

  public CalculateCanvasWidth = 0;
  public CalculateCanvasHeight = 0;

  public constructor()
  {
  }

  ngOnChanges(changes: SimpleChanges): void {
    this.CalculateCanvasWidth=this.DrawableCanvasWidth() * 0.98;
    this.CalculateCanvasHeight=this.DrawableCanvasHeight() * 0.80;
  }
}
