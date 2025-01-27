/*
 * Generated type guards for "gameEnded.ts".
 * WARNING: Do not manually change this file.
 */
import { isHeader } from "../../shared/header.guard";
import { GameEnded } from "./gameEnded";

export function isGameEnded(obj: unknown): obj is GameEnded {
    const typedObj = obj as GameEnded
    return (
        (   
            typedObj !== null &&
            typeof typedObj === "object" ||
            typeof typedObj === "function"
        ) &&
        isHeader(typedObj["header"]) as boolean &&
        (
            typeof typedObj.body === "object" ||
            typeof typedObj.body === "function"
        ) &&
        typeof typedObj.body.score === "number" &&
        typeof typedObj.body.word === "string" &&
        typedObj.header.type === "gameEnded"
    )
}
