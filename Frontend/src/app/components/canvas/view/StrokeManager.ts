import { CanvasStateService } from "../../../services/canvas-state/canvas-state.service";
import { Position2d } from "../../../../lib/Position2d";
import { StrokeVM } from "../../../services/canvas-state/StrokeVM";
import { StrokeConverter } from "../StrokeConverter/StrokeConverter";
import { CountdownService } from "../../../services/countdown/countdown.service";
import { PlayerDataService } from "../../../services/player-data/player-data.service";
import { PlayerType } from "../../../services/player-data/PlayerType";

export class StrokeManager
{
    private currentPath: Position2d[] | null = null;

    private canvasService: CanvasStateService;
    private countdownService: CountdownService;
    private playerDataService: PlayerDataService;
    private ctx: CanvasRenderingContext2D;

    public constructor
    (
        canvasService: CanvasStateService, 
        countdownService: CountdownService, 
        playerDataService: PlayerDataService,
        ctx: CanvasRenderingContext2D
    )
    {
        this.countdownService = countdownService;
        this.canvasService = canvasService;
        this.playerDataService = playerDataService;
        this.ctx = ctx;
    }

    public TryStartLine(posX: number, posY: number) : void
    {
        if (!this.CanStartStroke()) return;
        
        this.currentPath = [];
        this.currentPath.push({x: posX, y: posY});

        this.ctx.lineCap = "round";
        this.ctx.strokeStyle = this.canvasService.StrokeColor.value;
        this.ctx.lineWidth = this.canvasService.StrokeWidth.value *  this.ctx.canvas.width;
        this.ctx.beginPath();
        this.ctx.moveTo(posX, posY);
    }

    public TryMoveLine(posX: number, posY: number) : void
    {
        if (this.currentPath == null) return;

        if (!this.countdownService.IsRunning())
        {
            this.TrEndLine(); 
            return;
        }

        this.currentPath.push({x: posX, y: posY});
    
        this.ctx.lineTo(posX, posY);
        this.ctx.stroke();
    }

    public TrEndLine() : void
    {
        if (this.currentPath == null) return;

        let newStroke: StrokeVM = {
          StrokeWidth: this.ctx.lineWidth,
          Color: this.ctx.strokeStyle as string,
          PathData: this.currentPath
        };   

        let convertedStroke: StrokeVM = StrokeConverter.ConvertToUnitSpaceStroke
            (newStroke, this.ctx.canvas.width, this.ctx.canvas.height)

        this.canvasService.PushStroke(convertedStroke);

        // reset
        this.currentPath = null;
    }

    private CanStartStroke(): boolean
    {
        if (this.playerDataService.PlayerData.value.isNone()) return false;

        if (this.playerDataService.PlayerData.value.value.Role != PlayerType.Drawer || !this.countdownService.IsRunning()) 
        {
            return false;
        }

        return true;
    }
}