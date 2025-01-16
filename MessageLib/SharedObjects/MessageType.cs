namespace MessageLib.SharedObjects
{
    public enum MessageType
    {
        roundAmountChanged,
        roundDurationChanged,

        chatMessage,
        userDisconnected,
        userJoined,

        backgroundColor,
        clear,
        closePath,
        drawingSizeChanged,
        gameEnded,
        lineTo,
        moveTo,
        nextRound,
        point,
        searchedWord,
        selectWord,
        setDrawer,
        setNotDrawer,
        undo,
        userScore,
        gameStarted
    }
}