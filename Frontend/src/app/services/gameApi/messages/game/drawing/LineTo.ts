import { Position2d } from "../../../../../../lib/Position2d"

export class LineTo
{
    public constructor(point: Position2d, color: string)
    {
        this.Point = point;
        this.Color = color;
    }

    public Point: Position2d;
    public Color: string;
}