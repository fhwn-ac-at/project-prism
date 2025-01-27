import { Component, ElementRef, inject, input, InputSignal, OnChanges, OnInit, SimpleChanges, untracked, viewChild } from '@angular/core';
import { CanvasStateService } from "../../../../services/canvas-state/canvas-state.service";
import { StrokeManager } from '../StrokeManager';
import { CanvasRenderer } from '../CanvasRenderer';
import { MatCardModule } from '@angular/material/card';
import { CountdownService } from '../../../../services/countdown/countdown.service';
import { PlayerDataService } from '../../../../services/player-data/player-data.service';
import { CanDrawService } from '../../../../services/can-draw/can-draw.service';

@Component
({
  selector: 'app-canvas',
  imports: [MatCardModule],
  templateUrl: './canvas.component.html',
  styleUrl: './canvas.component.css',
})
export class CanvasComponent implements OnInit, OnChanges
{
  // view
  private ctx!: CanvasRenderingContext2D;
  private strokeManager!: StrokeManager;

  // vm
  private canvasState: CanvasStateService = inject(CanvasStateService);
  private canDraw: CanDrawService = inject(CanDrawService);

  // input
  public CanvasWidth: InputSignal<number> = input.required();
  public CanvasHeight: InputSignal<number> = input.required();

  // view
  public Canvas = viewChild.required<ElementRef<HTMLCanvasElement>>("drawingCanvas");

  // cd
  public ngOnInit() 
  {
    this.ctx = this.Canvas().nativeElement.getContext("2d")!;
    
    this.strokeManager = new StrokeManager(this.canvasState,this.canDraw, this.ctx);

    this.canvasState.SubscribeStrokesEvent
    (
      {next: (_) => {CanvasRenderer.DrawCanvas(this.ctx, this.canvasState.Strokes)}}
    );
  }

  ngOnChanges(changes: SimpleChanges): void {
    this.Canvas().nativeElement.width = this.CanvasWidth();
    this.Canvas().nativeElement.height = this.CanvasHeight();

    // TODO redraw
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