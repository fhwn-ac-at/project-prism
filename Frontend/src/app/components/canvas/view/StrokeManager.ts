import { CanvasStateService } from "../../../services/canvas-state/canvas-state.service";
import { CanDrawService } from "../../../services/can-draw/can-draw.service";
import { CoordinateConverter } from "../StrokeConverter/CoordinateConverter";

export class StrokeManager
{
    private canvasService: CanvasStateService;
    private canDrawService: CanDrawService;
    private ctx: CanvasRenderingContext2D;

    public constructor
    (
        canvasService: CanvasStateService, 
        canDrawService: CanDrawService,
        ctx: CanvasRenderingContext2D
    )
    {
        this.canvasService = canvasService;
        this.canDrawService = canDrawService;
        this.ctx = ctx;
    }

    public TryStartLine(posX: number, posY: number) : void
    {
        if (!this.canDrawService.CanUseCanvas()) return;
        
        const successfullyStarted = this.canvasService.StartStroke
        (
            CoordinateConverter.ConvertToUnitSpace({x: posX, y: posY}, this.ctx.canvas.width, this.ctx.canvas.height)
        );       
        if (!successfullyStarted) return;

        this.ctx.lineCap = "round";
        this.ctx.strokeStyle = this.canvasService.StrokeColor.value;
        this.ctx.lineWidth = this.canvasService.StrokeWidth.value *  this.ctx.canvas.width;
        this.ctx.beginPath();
        this.ctx.moveTo(posX, posY);
    }

    public TryMoveLine(posX: number, posY: number) : void
    {
        if (!this.canDrawService.IsCountdownRunning())
        {
            this.TrEndLine(); 
            return;
        }

        const successfullyMoved = this.canvasService.MoveStroke
        (
            CoordinateConverter.ConvertToUnitSpace({x: posX, y: posY}, this.ctx.canvas.width, this.ctx.canvas.height)
        );    

        if (!successfullyMoved) return;
    
        this.ctx.lineTo(posX, posY);
        this.ctx.stroke();
    }

    public TrEndLine() : void
    {
        this.canvasService.EndStroke();
    }
}