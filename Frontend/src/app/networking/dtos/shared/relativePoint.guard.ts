/*
 * Generated type guards for "relativePoint.ts".
 * WARNING: Do not manually change this file.
 */
import { RelativePoint } from "./relativePoint";

export function isRelativePoint(obj: unknown): obj is RelativePoint {
    const typedObj = obj as RelativePoint
    return (
        (typedObj !== null &&
            typeof typedObj === "object" ||
            typeof typedObj === "function") &&
        typeof typedObj["x"] === "number" &&
        typeof typedObj["y"] === "number"
    )
}
