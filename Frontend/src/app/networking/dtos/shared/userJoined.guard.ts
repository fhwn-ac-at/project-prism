/*
 * Generated type guards for "userJoined.ts".
 * WARNING: Do not manually change this file.
 */
import { isHeader } from "./header.guard";
import { isUser } from "./User.guard";
import { UserJoined } from "./userJoined";

export function isUserJoined(obj: unknown): obj is UserJoined {
    const typedObj = obj as UserJoined
    return (
        (typedObj !== null &&
            typeof typedObj === "object" ||
            typeof typedObj === "function") &&
        isHeader(typedObj["header"]) as boolean &&
        (typedObj["body"] !== null &&
            typeof typedObj["body"] === "object" ||
            typeof typedObj["body"] === "function") &&
        isUser(typedObj["body"]["user"]) as boolean &&
        typedObj.header.type === "userJoined"
    )
}
