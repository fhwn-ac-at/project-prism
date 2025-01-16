import { Component, ElementRef, input, InputSignal, OnInit, untracked, viewChild } from '@angular/core';
import { CanvasStateService } from "../../../../services/canvas-state/canvas-state.service";
import { StrokeManager } from '../StrokeManager';
import { CanvasRenderer } from '../CanvasRenderer';
import { MatCardModule } from '@angular/material/card';
import { CountdownService } from '../../../../services/countdown/countdown.service';
import { PlayerDataService } from '../../../../services/player-data/player-data.service';

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
  private canvasRenderer: CanvasRenderer;
  private ctx!: CanvasRenderingContext2D;
  private strokeManager!: StrokeManager;

  // vm
  private canvasState: CanvasStateService;
  private countdown: CountdownService;
  private playerData: PlayerDataService;

  public constructor
  (
    canvasViewService: CanvasStateService,
    countdownService: CountdownService,
    playerData: PlayerDataService
  )
  {
    this.canvasState = canvasViewService;
    this.countdown = countdownService;
    this.playerData = playerData;

    this.canvasRenderer = new CanvasRenderer();
  }

  // input
  public CanvasWidth: InputSignal<number> = input.required();
  public CanvasHeight: InputSignal<number> = input.required();

  // view
  public Canvas = viewChild.required<ElementRef<HTMLCanvasElement>>("drawingCanvas");

  // cd
  public ngOnInit() 
  {
    this.ctx = this.Canvas().nativeElement.getContext("2d")!;
    
    this.strokeManager = new StrokeManager(this.canvasState, this.countdown, this.playerData, this.ctx);

    this.canvasState.SubscribeToStrokes
    (
      {next: (_) => {this.canvasRenderer.DrawCanvas(this.ctx, this.canvasState.GetStrokes())}}
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