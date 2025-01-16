import { StrokeConverter } from "../StrokeConverter/StrokeConverter";
import { StrokeVM } from "../../../services/canvas-state/StrokeVM";
import { StrokeView } from "./StrokeView";

export class CanvasRenderer
{
    public DrawCanvas(context: CanvasRenderingContext2D, strokes: StrokeVM[])
    {
        context.reset();
        context.lineCap = "round";

        const strokeViews: StrokeView[] = strokes.map
        (
            (strokeVM => StrokeConverter.ConvertToViewStroke(strokeVM, context.canvas.width, context.canvas.height))
        )

        strokeViews.forEach(stroke => 
        {
            this.DrawStroke(context, stroke);
        });
    }

    private DrawStroke(context: CanvasRenderingContext2D, stroke: StrokeView)
    {
        context.strokeStyle = stroke.Color;
        context.lineWidth = stroke.StrokeSize;
        context.stroke(stroke.PathData);
    }
}