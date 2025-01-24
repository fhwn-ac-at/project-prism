/*
 * Generated type guards for "roundAmountChanged.ts".
 * WARNING: Do not manually change this file.
 */
import { isHeader } from "../shared/header.guard";
import { RoundAmountChanged } from "./roundAmountChanged";

export function isRoundAmountChanged(obj: unknown): obj is RoundAmountChanged {
    const typedObj = obj as RoundAmountChanged
    return (
        (typedObj !== null &&
            typeof typedObj === "object" ||
            typeof typedObj === "function") &&
        isHeader(typedObj["header"]) as boolean &&
        (typedObj["body"] !== null &&
            typeof typedObj["body"] === "object" ||
            typeof typedObj["body"] === "function") &&
        typeof typedObj["body"]["rounds"] === "number" &&
        typedObj.header.type === "roundAmountChanged"
    )
}
