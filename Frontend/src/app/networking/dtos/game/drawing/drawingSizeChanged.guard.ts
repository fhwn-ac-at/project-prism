/*
 * Generated type guards for "drawingSizeChanged.ts".
 * WARNING: Do not manually change this file.
 */
import { isHeader } from "../../shared/header.guard";
import { DrawingSizeChanged } from "./drawingSizeChanged";

export function isDrawingSizeChanged(obj: unknown): obj is DrawingSizeChanged {
    const typedObj = obj as DrawingSizeChanged
    return (
        (typedObj !== null &&
            typeof typedObj === "object" ||
            typeof typedObj === "function") &&
        isHeader(typedObj["header"]) as boolean &&
        (typedObj["body"] !== null &&
            typeof typedObj["body"] === "object" ||
            typeof typedObj["body"] === "function") &&
        typeof typedObj["body"]["size"] === "number" &&
        typedObj.header.type === "drawingSizeChanged"
    )
}
