import { Position2d } from "../../../../lib/Position2d";

export class CoordinateConverter
{
    public static ConvertToUnitSpace(pos: Position2d, width: number, height: number) : Position2d
    {
        return {
            x: pos.x / width,
            y: pos.y / height,
        }
    }

    public static ConvertToViewSpace(pos: Position2d, width: number, height: number) : Position2d
    {
        return {
            x: pos.x * width,
            y: pos.y * height,
        }
    }
}