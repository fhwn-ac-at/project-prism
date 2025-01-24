/*
 * Generated type guards for "userDisconnected.ts".
 * WARNING: Do not manually change this file.
 */
import { isHeader } from "./header.guard";
import { isUser } from "./User.guard";
import { UserDisconnected } from "./userDisconnected";

export function isUserDisconnected(obj: unknown): obj is UserDisconnected {
    const typedObj = obj as UserDisconnected
    return (
        (typedObj !== null &&
            typeof typedObj === "object" ||
            typeof typedObj === "function") &&
        isHeader(typedObj["header"]) as boolean &&
        (typedObj["body"] !== null &&
            typeof typedObj["body"] === "object" ||
            typeof typedObj["body"] === "function") &&
        isUser(typedObj["body"]["user"]) as boolean &&
        typedObj.header.type === "userDisconnected"
    )
}
