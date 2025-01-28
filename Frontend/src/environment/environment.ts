export const environment = {
    canvasOptions: 
    {
        strokeWidth: 0.05,
        strokeColor: "#000000"
    },
    keycloak:
    {
        url: "http://localhost:8180",
        realm: "prism",
        clientId: "Frontend"
    },
    lobbyDefaults:
    {
        roundAmount: 3,
        roundDuration: 120
    },
    api:
    {
        base: "http://localhost:5164",
        lobby:
        {
            base: "/api/lobby",
            connect: "/connect",
            startGame: "/startGame"
        },
        websocket: "/ws",
        websocketListen: "Frontend",
        websocketSend: "Backend"
    },
    game:
    {
        pickWordDialogDuration: 10000,
        showScoresDialogDuration: 2000,
    }
}