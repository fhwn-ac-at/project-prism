import { Position2d } from "../../../lib/Position2d";

export interface StrokeVM
{
    PathData: Position2d[];
    StrokeWidth: number,
    Color: string,
}