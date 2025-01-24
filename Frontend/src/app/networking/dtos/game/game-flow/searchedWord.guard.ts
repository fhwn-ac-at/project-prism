/*
 * Generated type guards for "searchedWord.ts".
 * WARNING: Do not manually change this file.
 */
import { isHeader } from "../../shared/header.guard";
import { SearchedWord } from "./searchedWord";

export function isSearchedWord(obj: unknown): obj is SearchedWord {
    const typedObj = obj as SearchedWord
    return (
        (typedObj !== null &&
            typeof typedObj === "object" ||
            typeof typedObj === "function") &&
        isHeader(typedObj["header"]) as boolean &&
        (typedObj["body"] !== null &&
            typeof typedObj["body"] === "object" ||
            typeof typedObj["body"] === "function") &&
        typeof typedObj["body"]["word"] === "string" && 
        typedObj.header.type === "searchedWord"
    )
}
