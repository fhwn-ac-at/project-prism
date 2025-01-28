/*
 * Generated type guards for "backgroundColor.ts".
 * WARNING: Do not manually change this file.
 */
import { isHeader } from "../../shared/header.guard";
import { isHexColor } from "../../shared/hexColor.guard";
import { BackgroundColor } from "./backgroundColor";

export function isBackgroundColor(obj: unknown): obj is BackgroundColor {
    const typedObj = obj as BackgroundColor
    return (
        (typedObj !== null &&
            typeof typedObj === "object" ||
            typeof typedObj === "function") &&
        isHeader(typedObj["header"]) as boolean &&
        (typedObj["body"] !== null &&
            typeof typedObj["body"] === "object" ||
            typeof typedObj["body"] === "function") &&
        isHexColor(typedObj["body"]["color"]) as boolean
        && typedObj.header.type === "backgroundColor"
    )
}