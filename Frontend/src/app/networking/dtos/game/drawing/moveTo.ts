import { DateTime } from "luxon";
import { Position2d } from "../../../../../lib/Position2d";
import { Header } from "../../shared/header";
import { RelativePoint } from "../../shared/relativePoint";

export interface MoveTo
{
    header: Header,
    body: 
    {
        point: RelativePoint
    }
}

export function BuildMoveTo(point: Position2d): MoveTo
{
    return {
        header: 
        {
            type: "moveTo", 
            timestamp: DateTime.now().toISO()
        }, 
        body: 
        {
            point: point,         
        }
    };
}