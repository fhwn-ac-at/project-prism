export interface ChatMessage
{
    Username: string,
    Message: string,
}

export function IsChatMessage(obj: any): obj is ChatMessage
{
    return obj != undefined &&
           typeof obj === "object" &&
           typeof obj.Username === "string" &&
           typeof obj.Message === "string"
}