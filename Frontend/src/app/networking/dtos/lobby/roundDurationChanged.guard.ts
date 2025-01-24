/*
 * Generated type guards for "roundDurationChanged.ts".
 * WARNING: Do not manually change this file.
 */
import { isHeader } from "../shared/header.guard";
import { RoundDurationChanged } from "./roundDurationChanged";

export function isRoundDurationChanged(obj: unknown): obj is RoundDurationChanged {
    const typedObj = obj as RoundDurationChanged
    return (
        (typedObj !== null &&
            typeof typedObj === "object" ||
            typeof typedObj === "function") &&
        isHeader(typedObj["header"]) as boolean &&
        (typedObj["body"] !== null &&
            typeof typedObj["body"] === "object" ||
            typeof typedObj["body"] === "function") &&
        typeof typedObj["body"]["duration"] === "number" &&
        typedObj.header.type === "roundDurationChanged"
    )
}
