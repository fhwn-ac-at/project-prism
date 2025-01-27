import { CanDrawService } from "../../../services/can-draw/can-draw.service";
import { CoordinateConverter } from "../StrokeConverter/CoordinateConverter";
import { StrokesService } from "../../../services/canvas-state/strokes.service";
import { CanvasOptionsService } from "../../../services/canvas-options/canvas-options.service";

export class StrokeManager
{
    private canDrawService: CanDrawService;
    private ctx: CanvasRenderingContext2D;

    private strokesService: StrokesService;
    private canvasOptions: CanvasOptionsService;

    public constructor
    (
        strokesService: StrokesService,
        canvasOptions: CanvasOptionsService,
        canDrawService: CanDrawService,
        ctx: CanvasRenderingContext2D
    )
    {
        this.strokesService = strokesService;
        this.canvasOptions = canvasOptions;

        this.canDrawService = canDrawService;
        this.ctx = ctx;
    }

    public TryStartLine(posX: number, posY: number)
    {
        if (!this.canDrawService.CanUseCanvas()) return;
        
        this.strokesService.StartStroke
        (
            CoordinateConverter.ConvertToUnitSpace({x: posX, y: posY}, this.ctx.canvas.width, this.ctx.canvas.height),
            this.canvasOptions.StrokeWidth.value,
            this.canvasOptions.StrokeColor.value
        );       

        // this.ctx.lineCap = "round";
        // this.ctx.strokeStyle = this.canvasOptions.StrokeColor.value;
        // this.ctx.lineWidth = this.canvasOptions.StrokeWidth.value *  this.ctx.canvas.width;
        // this.ctx.beginPath();
        // this.ctx.moveTo(posX, posY);
    }

    public TryMoveLine(posX: number, posY: number) : void
    {
        if (!this.canDrawService.IsCountdownRunning())
        {
            this.TrEndLine(); 
            return;
        }

        const successfullyMoved = this.strokesService.MoveStroke
        (
            CoordinateConverter.ConvertToUnitSpace({x: posX, y: posY}, this.ctx.canvas.width, this.ctx.canvas.height),
            this.canvasOptions.StrokeColor.value
        );    

        // if (!successfullyMoved) return;
    
        // this.ctx.lineTo(posX, posY);
        // this.ctx.stroke();
    }

    public TrEndLine() : void
    {
        this.strokesService.EndStroke();
    }
}