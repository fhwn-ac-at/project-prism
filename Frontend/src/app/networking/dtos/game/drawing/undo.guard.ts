/*
 * Generated type guards for "undo.ts".
 * WARNING: Do not manually change this file.
 */
import { isHeader } from "../../shared/header.guard";
import { Undo } from "./undo";

export function isUndo(obj: unknown): obj is Undo {
    const typedObj = obj as Undo
    return (
        (typedObj !== null &&
            typeof typedObj === "object" ||
            typeof typedObj === "function") &&
        isHeader(typedObj["header"]) as boolean &&
        typedObj.header.type === "undo"
    )
}
