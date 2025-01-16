import { StrokeView } from "../view/StrokeView";
import { Position2d } from "../../../../lib/Position2d";
import { StrokeVM } from "../../../services/canvas-state/StrokeVM";
import { CoordinateConverter } from "./CoordinateConverter";

export class StrokeConverter
{
    public static ConvertToUnitSpaceStroke(strokeVM: StrokeVM, width: number, height: number): StrokeVM
    {
        return {
            Color: strokeVM.Color,
            StrokeWidth: strokeVM.StrokeWidth / width,
            PathData: strokeVM.PathData.map((pos) => CoordinateConverter.ConvertToUnitSpace(pos, width, height)),
        }
    }

    public static ConvertToViewStroke(strokeVM: StrokeVM, width: number, height: number): StrokeView
    {
        return {
            Color: strokeVM.Color,
            StrokeSize: strokeVM.StrokeWidth * width,
            PathData: this.ConvertPositionsToPath2d(strokeVM.PathData.map((pos) => CoordinateConverter.ConvertToViewSpace(pos, width, height))),
        }
    }

    private static ConvertPositionsToPath2d(positions: Position2d[]): Path2D
    {
        const path: Path2D = new Path2D();

        if(positions.length == 0) return path;

        path.moveTo(positions[0].x, positions[0].y)

        for(let i = 1; i < positions.length; i++)
        {
            path.lineTo(positions[i].x, positions[i].y);
        }

        return path;
    }
}