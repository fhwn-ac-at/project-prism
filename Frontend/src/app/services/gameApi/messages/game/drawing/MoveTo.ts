import { Position2d } from "../../../../../../lib/Position2d";

export class MoveTo
{
    public constructor(point: Position2d)
    {
        this.Point = point;
    }
    
    public Point: Position2d;
}