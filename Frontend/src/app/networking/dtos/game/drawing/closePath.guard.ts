/*
 * Generated type guards for "closePath.ts".
 * WARNING: Do not manually change this file.
 */
import { isHeader } from "../../shared/header.guard";
import { ClosePath } from "./closePath";

export function isClosePath(obj: unknown): obj is ClosePath {
    const typedObj = obj as ClosePath
    return (
        (typedObj !== null &&
            typeof typedObj === "object" ||
            typeof typedObj === "function") &&
        isHeader(typedObj["header"]) as boolean &&
        typedObj.header.type === "closePath"
    )
}
