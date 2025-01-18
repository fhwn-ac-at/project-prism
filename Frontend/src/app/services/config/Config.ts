export interface Config
{
    canvasOptions: 
    {
        strokeWidth: number,
        strokeColor: string,
    },
    keycloak:
    {
        url: string,
        realm: string,
        clientId: string,
    },
    lobbyDefaults:
    {
        roundAmount: number,
        roundDuration: number
    }
}

export function isConfig(obj: any): obj is Config 
{
    return  typeof obj === 'object' &&
            obj !== null &&
            typeof obj.canvasOptions === 'object' &&
            typeof obj.canvasOptions.strokeWidth === 'number' &&
            typeof obj.canvasOptions.strokeColor === 'string' &&
            typeof obj.keycloak === 'object' &&
            typeof obj.keycloak.url === 'number' &&
            typeof obj.keycloak.realm === 'string' &&
            typeof obj.keycloak.clientId === 'string' &&
            typeof obj.lobbyDefaults === 'object' &&
            typeof obj.lobbyDefaults.roundAmount === "number" &&
            typeof obj.lobbyDefaults.roundDuration === "number"
}
