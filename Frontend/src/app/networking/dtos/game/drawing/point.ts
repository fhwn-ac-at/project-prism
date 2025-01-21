import { HexColor } from "../../shared/hexColor";
import { RelativePoint } from "../../shared/relativePoint";

export interface point
{
    point: RelativePoint,
    radius: number,
    color: HexColor
}