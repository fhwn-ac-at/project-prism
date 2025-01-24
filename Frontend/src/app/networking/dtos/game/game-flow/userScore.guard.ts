/*
 * Generated type guards for "userScore.ts".
 * WARNING: Do not manually change this file.
 */
import { isHeader } from "../../shared/header.guard";
import { isUser } from "../../shared/User.guard";
import { UserScore } from "./userScore";

export function isUserScore(obj: unknown): obj is UserScore {
    const typedObj = obj as UserScore
    return (
        (typedObj !== null &&
            typeof typedObj === "object" ||
            typeof typedObj === "function") &&
        isHeader(typedObj["header"]) as boolean &&
        (typedObj["body"] !== null &&
            typeof typedObj["body"] === "object" ||
            typeof typedObj["body"] === "function") &&
        typeof typedObj["body"]["score"] === "number" &&
        isUser(typedObj["body"]["user"]) as boolean &&
        typedObj.header.type === "userScore"
    )
}
