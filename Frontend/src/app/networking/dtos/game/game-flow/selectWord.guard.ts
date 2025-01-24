/*
 * Generated type guards for "selectWord.ts".
 * WARNING: Do not manually change this file.
 */
import { isHeader } from "../../shared/header.guard";
import { SelectWord } from "./selectWord";

export function isSelectWord(obj: unknown): obj is SelectWord {
    const typedObj = obj as SelectWord
    return (
        (typedObj !== null &&
            typeof typedObj === "object" ||
            typeof typedObj === "function") &&
        isHeader(typedObj["header"]) as boolean &&
        (typedObj["body"] !== null &&
            typeof typedObj["body"] === "object" ||
            typeof typedObj["body"] === "function") &&
        typeof typedObj["body"]["word"] === "string" &&
        typedObj.header.type === "selectWord"
    )
}
