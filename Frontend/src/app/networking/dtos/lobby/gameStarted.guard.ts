/*
 * Generated type guards for "gameStarted.ts".
 * WARNING: Do not manually change this file.
 */
import { isHeader } from "../shared/header.guard";
import { GameStarted } from "./gameStarted";

export function isGameStarted(obj: unknown): obj is GameStarted {
    const typedObj = obj as GameStarted
    return (
        (typedObj !== null &&
            typeof typedObj === "object" ||
            typeof typedObj === "function") &&
        isHeader(typedObj["header"]) as boolean && 
        typedObj.header.type === "gameStarted"
    )
}
