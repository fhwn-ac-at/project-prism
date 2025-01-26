/*
 * Generated type guards for "header.ts".
 * WARNING: Do not manually change this file.
 */
import { Header } from "./header";

export function isHeader(obj: unknown): obj is Header {
    const typedObj = obj as Header
    return (
        (typedObj !== null &&
            typeof typedObj === "object" ||
            typeof typedObj === "function") &&
        typeof typedObj["type"] === "string" &&
        typeof typedObj["timestamp"] === "object"
    )
}
