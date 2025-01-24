/*
 * Generated type guards for "lineTo.ts".
 * WARNING: Do not manually change this file.
 */
import { isHeader } from "../../shared/header.guard";
import { isRelativePoint } from "../../shared/relativePoint.guard";
import { isHexColor } from "../../shared/hexColor.guard";
import { LineTo } from "./lineTo";

export function isLineTo(obj: unknown): obj is LineTo {
    const typedObj = obj as LineTo
    return (
        (typedObj !== null &&
            typeof typedObj === "object" ||
            typeof typedObj === "function") &&
        isHeader(typedObj["header"]) as boolean &&
        (typedObj["body"] !== null &&
            typeof typedObj["body"] === "object" ||
            typeof typedObj["body"] === "function") &&
        isRelativePoint(typedObj["body"]["point"]) as boolean &&
        isHexColor(typedObj["body"]["color"]) as boolean &&
        typedObj.header.type === "lineTo"
    )
}
