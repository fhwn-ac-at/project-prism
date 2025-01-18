export interface ChatMessage
{
    Username: string,
    Message: string,
    Color: string,
}

export function IsChatMessage(obj: any): obj is ChatMessage
{
    return obj != undefined &&
           typeof obj === "object" &&
           typeof obj.Username === "string" &&
           typeof obj.Message === "string" &&
           typeof obj.Color === 'string';
}