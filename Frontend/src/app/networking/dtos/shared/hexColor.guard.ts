/*
 * Generated type guards for "hexColor.ts".
 * WARNING: Do not manually change this file.
 */
import { HexColor } from "./hexColor";

export function isHexColor(obj: unknown): obj is HexColor {
    const typedObj = obj as HexColor
    return (
        (typedObj !== null &&
            typeof typedObj === "object" ||
            typeof typedObj === "function") &&
        typeof typedObj["hexString"] === "string"
    )
}
