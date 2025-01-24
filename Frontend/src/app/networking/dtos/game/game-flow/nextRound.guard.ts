/*
 * Generated type guards for "nextRound.ts".
 * WARNING: Do not manually change this file.
 */
import { isHeader } from "../../shared/header.guard";
import { NextRound } from "./nextRound";

export function isNextRound(obj: unknown): obj is NextRound {
    const typedObj = obj as NextRound
    return (
        (typedObj !== null &&
            typeof typedObj === "object" ||
            typeof typedObj === "function") &&
        isHeader(typedObj["header"]) as boolean &&
        typedObj.header.type === "nextRound"
    )
}
