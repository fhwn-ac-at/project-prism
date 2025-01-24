export interface ConnectToLobbyResponse
{
    lobbyId: string, 
    username: string, 
    userId: string
}

export function isConnectToLobbyResponse(obj: any): obj is ConnectToLobbyResponse 
{
    return typeof obj === 'object' &&
        obj !== null &&
        typeof obj.lobbyId === 'string' &&
        typeof obj.username === 'string' &&
        typeof obj.userId === 'string';
}