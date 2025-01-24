/*
 * Generated type guards for "point.ts".
 * WARNING: Do not manually change this file.
 */
import { isHeader } from "../../shared/header.guard";
import { isRelativePoint } from "../../shared/relativePoint.guard";
import { isHexColor } from "../../shared/hexColor.guard";
import { Point } from "./point";

export function isPoint(obj: unknown): obj is Point {
    const typedObj = obj as Point
    return (
        (typedObj !== null &&
            typeof typedObj === "object" ||
            typeof typedObj === "function") &&
        isHeader(typedObj["header"]) as boolean &&
        (typedObj["body"] !== null &&
            typeof typedObj["body"] === "object" ||
            typeof typedObj["body"] === "function") &&
        isRelativePoint(typedObj["body"]["point"]) as boolean &&
        typeof typedObj["body"]["radius"] === "number" &&
        isHexColor(typedObj["body"]["color"]) as boolean &&
        typedObj.header.type === "point"
    )
}
