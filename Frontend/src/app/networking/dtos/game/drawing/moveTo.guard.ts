/*
 * Generated type guards for "moveTo.ts".
 * WARNING: Do not manually change this file.
 */
import { isHeader } from "../../shared/header.guard";
import { isRelativePoint } from "../../shared/relativePoint.guard";
import { MoveTo } from "./moveTo";

export function isMoveTo(obj: unknown): obj is MoveTo {
    const typedObj = obj as MoveTo
    return (
        (typedObj !== null &&
            typeof typedObj === "object" ||
            typeof typedObj === "function") &&
        isHeader(typedObj["header"]) as boolean &&
        (typedObj["body"] !== null &&
            typeof typedObj["body"] === "object" ||
            typeof typedObj["body"] === "function") &&
        isRelativePoint(typedObj["body"]["point"]) as boolean &&
        typedObj.header.type === "moveTo"
    )
}
