import { DateTime } from "luxon";
import { Position2d } from "../../../../../lib/Position2d";
import { Header } from "../../shared/header";
import { HexColor } from "../../shared/hexColor";
import { RelativePoint } from "../../shared/relativePoint";

export interface Point
{
    header: Header,
    body: 
    {
        point: RelativePoint,
        radius: number,
        color: HexColor
    }
}

export function BuildPoint(point: Position2d, radius: number, color: string): Point
{
    return {
        header: 
        {
            type: "point", 
            timestamp: DateTime.now()
        }, 
        body: 
        {
            point: point,     
            radius: radius,
            color: {hexString: color}     
        }
    };
}