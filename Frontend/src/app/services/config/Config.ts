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
    },
    api:
    {
        base: string,
        lobby:
        {
            base: string,
            connect: string,
            startGame: string
        }
        websocket: string,
        websocketListen: string,
        websocketSend: string
    }
}