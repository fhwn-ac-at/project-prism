/*
 * Generated type guards for "clear.ts".
 * WARNING: Do not manually change this file.
 */
import { isHeader } from "../../shared/header.guard";
import { Clear } from "./clear";

export function isClear(obj: unknown): obj is Clear {
    const typedObj = obj as Clear
    return (
        (typedObj !== null &&
            typeof typedObj === "object" ||
            typeof typedObj === "function") &&
        isHeader(typedObj["header"]) as boolean &&
        typedObj.header.type === "clear"
    )
}
