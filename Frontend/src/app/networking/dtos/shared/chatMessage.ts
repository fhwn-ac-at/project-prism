import { DateTime } from "luxon";
import { Header } from "./header";
import { User } from "./User";

export interface ChatMessage
{
    header: Header,
    body: {
        text: string,
        user: User,
    }
}

export function BuildChatMessage(text: string, user: User): ChatMessage
{
    return {
        header: 
        {
            type: "chatMessage", 
            timestamp: DateTime.now()
        }, 
        body: 
        {
            text: text,
            user: user
        }
    };
}