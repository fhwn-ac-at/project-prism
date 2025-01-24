import { Position2d } from "../../../../../lib/Position2d";
import { Header } from "../../shared/header";
import { HexColor } from "../../shared/hexColor";
import { RelativePoint } from "../../shared/relativePoint";

export interface LineTo
{
    header: Header,
    body: 
    {
        point: RelativePoint,
        color: HexColor,
    }
}

export function BuildLineTo(point: Position2d, color: string): LineTo
{
    return {
        header: 
        {
            type: "lineTo", 
            timestamp: Date.now()
        }, 
        body: 
        {
            point: point,
            color: {hexString: color}
        }
    };
}