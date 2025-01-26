
namespace MessageLib
{
    using FrenziedMarmot.DependencyInjection;
    using MessageLib.Game;
    using MessageLib.Joined;
    using MessageLib.Lobby;
    using MessageLib.SharedObjects;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;

    [Injectable(Lifetime = ServiceLifetime.Transient, TargetType = typeof(IMessageDistributor))]
    [Injectable(Lifetime = ServiceLifetime.Transient, TargetType = typeof(MessageDistributor))]
    public class MessageDistributor(Validator validator, Deserializer deserializer, ILogger<MessageDistributor>? logger = null): IMessageDistributor
    {
        private readonly ILogger<MessageDistributor>? logger = logger;
        private readonly Deserializer deserializer = deserializer;
        private readonly Validator validator = validator;

        public event EventHandler<RoundAmountChangedMessageBody>? ReceivedRoundAmountChangedMessage;
        public event EventHandler<RoundDurationChangedMessageBody>? ReceivedRoundDurationChangedMessage;

        public event EventHandler<ChatMessageMessageBody>? ReceivedChatMessageMessage;
        public event EventHandler<UserDisconnectedMessageBody>? ReceivedUserDisconnectedMessage;
        public event EventHandler<UserJoinedMessageBody>? ReceivedUserJoinedMessage;

        public event EventHandler<BackgroundColorMessageBody>? ReceivedBackgroundColorMessage;
        public event EventHandler<EmptyMessageBody>? ReceivedClearMessage;
        public event EventHandler<EmptyMessageBody>? ReceivedClosePathMessage;
        public event EventHandler<DrawingSizeChangedMessageBody>? ReceivedDrawingSizeChangedMessage;
        public event EventHandler<GameEndedMessageBody>? ReceivedGameEndedMessage;
        public event EventHandler<EmptyMessageBody>? ReceivedGameStartedMessage;
        public event EventHandler<LineToMessageBody>? ReceivedLineToMessage;
        public event EventHandler<MoveToMessageBody>? ReceivedMoveToMessage;
        public event EventHandler<NextRoundMessageBody>? ReceivedNextRoundMessage;
        public event EventHandler<PointMessageBody>? ReceivedPointMessage;
        public event EventHandler<SearchedWordMessageBody>? ReceivedSearchedWordMessage;
        public event EventHandler<SelectWordMessageBody>? ReceivedSelectWordMessage;
        public event EventHandler<SetDrawerMessageBody>? ReceivedSetDrawerMessage;
        public event EventHandler<EmptyMessageBody>? ReceivedSetNotDrawerMessage;
        public event EventHandler<EmptyMessageBody>? ReceivedUndoMessage;
        public event EventHandler<UserScoreMessageBody>? ReceivedUserScoreMessage;

        /// <summary>
        /// Tries to validate, parse and distribute a json message defined in <see cref="MessageLib"/>
        /// </summary>
        /// <param name="message">The json message as string</param>
        /// <returns>True if this handler distributed the message otherwise false</returns>
        public bool HandleMessage(string message)
        {
            this.logger?.LogTrace("Got Message: {}", message);

            if (!this.validator.Validate(message, out MessageType? messageType))
            {
                this.logger?.LogInformation("Message not valid! Message: {}", message);
                return false;
            }

            MessageHeader? header = this.deserializer.DeserializeHeader(message);

            if (header==null)
            {
                this.logger?.LogError("No Header in valid Message message: {}", message);
                return false;
            }

            var headerDiffMillis = DateTime.Now.Subtract(header.Timestamp.ToLocalTime()).TotalMilliseconds;

            if (headerDiffMillis<0||headerDiffMillis>TimeSpan.FromMinutes(1).TotalMilliseconds)
            {
                this.logger?.LogWarning("Message to old! Message: {}", message);
                return false;
            }

            switch (messageType)
            {
                case MessageType.roundAmountChanged:
                    this.FireEvent<RoundAmountChangedMessage, RoundAmountChangedMessageBody>(this.ReceivedRoundAmountChangedMessage, message);
                    break;
                case MessageType.roundDurationChanged:
                    this.FireEvent<RoundDurationChangedMessage, RoundDurationChangedMessageBody>(this.ReceivedRoundDurationChangedMessage, message);
                    break;

                case MessageType.chatMessage:
                    this.FireEvent<ChatMessageMessage, ChatMessageMessageBody>(this.ReceivedChatMessageMessage, message);
                    break;
                case MessageType.userDisconnected:
                    this.FireEvent<UserDisconnectedMessage, UserDisconnectedMessageBody>(this.ReceivedUserDisconnectedMessage, message);
                    break;
                case MessageType.userJoined:
                    this.FireEvent<UserJoinedMessage, UserJoinedMessageBody>(this.ReceivedUserJoinedMessage, message);
                    break;

                case MessageType.backgroundColor:
                    this.FireEvent<BackgroundColorMessage, BackgroundColorMessageBody>(this.ReceivedBackgroundColorMessage, message);
                    break;
                case MessageType.clear:
                    this.FireEvent<ClearMessage, EmptyMessageBody>(this.ReceivedClearMessage, message);
                    break;
                case MessageType.closePath:
                    this.FireEvent<ClosePathMessage, EmptyMessageBody>(this.ReceivedClosePathMessage, message);
                    break;
                case MessageType.drawingSizeChanged:
                    this.FireEvent<DrawingSizeChangedMessage, DrawingSizeChangedMessageBody>(this.ReceivedDrawingSizeChangedMessage, message);
                    break;
                case MessageType.gameEnded:
                    this.FireEvent<GameEndedMessage, GameEndedMessageBody>(this.ReceivedGameEndedMessage, message);
                    break;
                case MessageType.gameStarted:
                    this.FireEvent<GameStartedMessage, EmptyMessageBody>(this.ReceivedGameStartedMessage, message);
                    break;
                case MessageType.lineTo:
                    this.FireEvent<LineToMessage, LineToMessageBody>(this.ReceivedLineToMessage, message);
                    break;
                case MessageType.moveTo:
                    this.FireEvent<MoveToMessage, MoveToMessageBody>(this.ReceivedMoveToMessage, message);
                    break;
                case MessageType.nextRound:
                    this.FireEvent<NextRoundMessage, NextRoundMessageBody>(this.ReceivedNextRoundMessage, message);
                    break;
                case MessageType.point:
                    this.FireEvent<PointMessage, PointMessageBody>(this.ReceivedPointMessage, message);
                    break;
                case MessageType.searchedWord:
                    this.FireEvent<SearchedWordMessage, SearchedWordMessageBody>(this.ReceivedSearchedWordMessage, message);
                    break;
                case MessageType.selectWord:
                    this.FireEvent<SelectWordMessage, SelectWordMessageBody>(this.ReceivedSelectWordMessage, message);
                    break;
                case MessageType.setDrawer:
                    this.FireEvent<SetDrawerMessage, SetDrawerMessageBody>(this.ReceivedSetDrawerMessage, message);
                    break;
                case MessageType.setNotDrawer:
                    this.FireEvent<SetNotDrawerMessage, EmptyMessageBody>(this.ReceivedSetNotDrawerMessage, message);
                    break;
                case MessageType.undo:
                    this.FireEvent<UndoMessage, EmptyMessageBody>(this.ReceivedUndoMessage, message);
                    break;
                case MessageType.userScore:
                    this.FireEvent<UserScoreMessage, UserScoreMessageBody>(this.ReceivedUserScoreMessage, message);
                    break;


                default:
                    return false;
            }

            return true;
        }

        // TODO handle errors
        private void FireEvent<T, U>(EventHandler<U>? eventHandler, string message) where T : Message<U> where U : IMessageBody
        {
            if (eventHandler==null)
            {
                return;
            }

            T? parsedMessage = this.deserializer.DeserializeTo<T>(message);

            if (parsedMessage==null)
            {
                return;
            }

            eventHandler.Invoke(this, parsedMessage.MessageBody);
        }
    }
}
