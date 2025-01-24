/*
 * Generated type guards for "chatMessage.ts".
 * WARNING: Do not manually change this file.
 */
import { isHeader } from "./header.guard";
import { isUser } from "./User.guard";
import { ChatMessage } from "./chatMessage";

export function isChatMessage(obj: unknown): obj is ChatMessage {
    const typedObj = obj as ChatMessage
    return (
        (typedObj !== null &&
            typeof typedObj === "object" ||
            typeof typedObj === "function") &&
        isHeader(typedObj["header"]) as boolean &&
        (typedObj["body"] !== null &&
            typeof typedObj["body"] === "object" ||
            typeof typedObj["body"] === "function") &&
        typeof typedObj["body"]["text"] === "string" &&
        isUser(typedObj["body"]["user"]) as boolean &&
        typedObj.header.type === "chatMessage"
    )
}
