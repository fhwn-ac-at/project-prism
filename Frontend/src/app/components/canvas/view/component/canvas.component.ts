import { Component, ElementRef, inject, input, InputSignal, OnInit, untracked, viewChild } from '@angular/core';
import { StrokesContainer } from "../../../../services/canvas-state/StrokesContainer";
import { StrokeManager } from '../StrokeManager';
import { CanvasRenderer } from '../CanvasRenderer';
import { MatCardModule } from '@angular/material/card';
import { CountdownService } from '../../../../services/countdown/countdown.service';
import { PlayerDataService } from '../../../../services/player-data/player-data.service';
import { CanDrawService } from '../../../../services/can-draw/can-draw.service';
import { StrokesService } from '../../../../services/canvas-state/strokes.service';
import { CanvasOptionsService } from '../../../../services/canvas-options/canvas-options.service';

@Component
({
  selector: 'app-canvas',
  imports: [MatCardModule],
  templateUrl: './canvas.component.html',
  styleUrl: './canvas.component.css',
})
export class CanvasComponent implements OnInit
{
  // view
  private ctx!: CanvasRenderingContext2D;
  private strokeManager!: StrokeManager;

  // vm
  private strokesService: StrokesService = inject(StrokesService);
  private canDraw: CanDrawService = inject(CanDrawService);
  private canvasOptions: CanvasOptionsService = inject(CanvasOptionsService)

  // input
  public CanvasWidth: InputSignal<number> = input.required();
  public CanvasHeight: InputSignal<number> = input.required();

  // view
  public Canvas = viewChild.required<ElementRef<HTMLCanvasElement>>("drawingCanvas");

  // cd
  public ngOnInit() 
  {
    this.ctx = this.Canvas().nativeElement.getContext("2d")!;
    
    this.strokeManager = new StrokeManager(this.strokesService, this.canvasOptions, this.canDraw, this.ctx);

    this.strokesService.ObserveStrokesEvent().subscribe
    (
      (_) => CanvasRenderer.DrawCanvas(this.ctx, this.strokesService.Strokes)
    );
  }
  
  // events
  public OnMouseDown(event: MouseEvent) 
  {
    this.strokeManager.TryStartLine(event.offsetX, event.offsetY)
  }

  public OnMouseMove(event: MouseEvent) 
  {
    this.strokeManager.TryMoveLine(event.offsetX, event.offsetY)
  }

  public OnMouseUp(_: MouseEvent) 
  {
    this.strokeManager.TrEndLine();
  } 

  public OnMouseLeave(_: MouseEvent) 
  {
    this.strokeManager.TrEndLine();
  }
}