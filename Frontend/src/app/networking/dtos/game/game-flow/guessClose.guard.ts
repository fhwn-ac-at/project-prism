/*
 * Generated type guards for "gameEnded.ts".
 * WARNING: Do not manually change this file.
 */
import { isHeader } from "../../shared/header.guard"
import { GuessClose } from "./guessClose"

export function isGuessClose(obj: unknown): obj is GuessClose {
    const typedObj = obj as GuessClose
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
        typeof typedObj.body.guess === "string" &&
        typeof typedObj.body.distance === "number" &&
        typedObj.header.type === "guessClose"
    )
}