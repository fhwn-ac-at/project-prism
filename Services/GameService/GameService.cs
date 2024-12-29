namespace GameService
{
    using FrenziedMarmot.DependencyInjection;
    using MessageLib;
    using MessageLib.Game;
    using MessageLib.Joined;
    using MessageLib.Lobby;
    using MessageLib.SharedObjects;

    [Injectable(Lifetime = ServiceLifetime.Transient)]
    public class GameService : IDisposable
    {
        private readonly HashSet<string> users = [];
        // Users that where connected once
        private readonly HashSet<string> zombieUsers = [];

        private MessageDistributor messageDistributor;
        private readonly GameServiceMessageBroker messageBroker;
        private readonly string Id;

        private readonly List<Message<IMessageBody>> currentDrawing = [];

        private string? drawerId;
        private bool isGameRunning;
        private DateTime? drawingStartTime;
        private TimeSpan? drawingDuration;

        private readonly ILogger<GameService>? logger;
        private bool disposedValue;

        public GameService(string Id, GameServiceMessageBroker messageBroker, MessageDistributor messageDistributor, ILogger<GameService>? logger = null)
        {
            this.Id = Id;
            this.messageBroker = messageBroker;
            this.messageDistributor = messageDistributor;
            this.logger = logger;
        }

        public int UserCount => this.users.Count;

        public bool AddUser(string key)
        {
            this.zombieUsers.Remove(key);
            this.messageBroker.ConnectToQueueAsync(this.Id, this.ConnectMessageDistributor(key));
            return this.users.Add(key);
        }

        public bool RemoveUser(string key)
        {
            this.zombieUsers.Add(key);
            return this.users.Remove(key);
        }

        private IMessageDistributor ConnectMessageDistributor(string key)
        {
            this.messageDistributor.ReceivedChatMessageMessage+=(_, message) => this.ReceivedChatMessageMessage(key, message);
            this.messageDistributor.ReceivedNextRoundMessage+=(_, message) => this.ReceivedNextRoundMessage(key, message);
            this.messageDistributor.ReceivedSearchedWordMessage+=(_, message) => this.ReceivedSearchedWordMessage(key, message);
            this.messageDistributor.ReceivedGameEndedMessage+=(_, message) => this.ReceivedGameEndedMessage(key, message);
            this.messageDistributor.ReceivedRoundAmountChangedMessage+=(_, message) => this.ReceivedRoundAmountChangedMessage(key, message);
            this.messageDistributor.ReceivedRoundDurationChangedMessage+=(_, message) => this.ReceivedRoundDurationChangedMessage(key, message);
            this.messageDistributor.ReceivedSelectWordMessage+=(_, message) => this.ReceivedSelectWordMessage(key, message);
            this.messageDistributor.ReceivedSetDrawerMessage+=(_, message) => this.ReceivedSetDrawerMessage(key, message);
            this.messageDistributor.ReceivedSetNotDrawerMessage+=(_, message) => this.ReceivedSetNotDrawerMessage(key, message);
            this.messageDistributor.ReceivedUserDisconnectedMessage+=(_, message) => this.ReceivedUserDisconnectedMessage(key, message);
            this.messageDistributor.ReceivedUserJoinedMessage+=(_, message) => this.ReceivedUserJoinedMessage(key, message);
            this.messageDistributor.ReceivedUserScoreMessage+=(_, message) => this.ReceivedUserScoreMessage(key, message);
            this.messageDistributor.ReceivedUndoMessage+=(_, message) => this.ReceivedUndoMessage(key, message);
            this.messageDistributor.ReceivedClearMessage+=(_, message) => this.ReceivedClearMessage(key, message);


            this.messageDistributor.ReceivedBackgroundColorMessage+=(_, message) => this.ReceivedBackgroundColorMessage(key, message);
            this.messageDistributor.ReceivedClosePathMessage+=(_, message) => this.ReceivedClosePathMessage(key, message);
            this.messageDistributor.ReceivedDrawingSizeChangedMessage+=(_, message) => this.ReceivedDrawingSizeChangedMessage(key, message);
            this.messageDistributor.ReceivedLineToMessage+=(_, message) => this.ReceivedLineToMessage(key, message);
            this.messageDistributor.ReceivedMoveToMessage+=(_, message) => this.ReceivedMoveToMessage(key, message);
            this.messageDistributor.ReceivedPointMessage+=(_, message) => this.ReceivedPointMessage(key, message);

            return this.messageDistributor;
        }

        private void ReceivedPointMessage(string key, PointMessageBody message)
        {
            throw new NotImplementedException();
        }

        private void ReceivedMoveToMessage(string key, MoveToMessageBody message)
        {
            throw new NotImplementedException();
        }

        private void ReceivedLineToMessage(string key, LineToMessageBody message)
        {
            throw new NotImplementedException();
        }

        private void ReceivedDrawingSizeChangedMessage(string key, DrawingSizeChangedMessageBody message)
        {
            throw new NotImplementedException();
        }

        private void ReceivedClosePathMessage(string key, EmptyMessageBody message)
        {
            throw new NotImplementedException();
        }

        private void ReceivedBackgroundColorMessage(string key, BackgroundColorMessageBody message)
        {
            throw new NotImplementedException();
        }

        private void ReceivedClearMessage(string key, EmptyMessageBody message)
        {
            this.currentDrawing.Clear();
            this.DistributeMessage(key, new ClearMessage());
        }

        private void ReceivedUndoMessage(string key, EmptyMessageBody message)
        {
            this.currentDrawing.RemoveAt(this.currentDrawing.Count-1);
            this.DistributeMessage(key, new UndoMessage());
        }

        private void ReceivedUserScoreMessage(string key, UserScoreMessageBody message)
        {
            throw new NotImplementedException();
        }

        private void ReceivedUserJoinedMessage(string key, UserJoinedMessageBody message)
        {
            throw new NotImplementedException();
        }

        private void ReceivedUserDisconnectedMessage(string key, UserDisconnectedMessageBody message)
        {
            throw new NotImplementedException();
        }

        private void ReceivedSetNotDrawerMessage(string key, EmptyMessageBody message)
        {
            throw new NotImplementedException();
        }

        private void ReceivedSetDrawerMessage(string key, SetDrawerMessageBody message)
        {
            throw new NotImplementedException();
        }

        private void ReceivedSelectWordMessage(string key, SelectWordMessageBody message)
        {
            throw new NotImplementedException();
        }

        private void ReceivedRoundDurationChangedMessage(string key, RoundDurationChangedMessageBody message)
        {
            throw new NotImplementedException();
        }

        private void ReceivedRoundAmountChangedMessage(string key, RoundAmountChangedMessageBody message)
        {
            throw new NotImplementedException();
        }

        private void ReceivedGameEndedMessage(string key, EmptyMessageBody message)
        {
            throw new NotImplementedException();
        }

        private void ReceivedSearchedWordMessage(string key, SearchedWordMessageBody message)
        {
            throw new NotImplementedException();
        }

        private void ReceivedNextRoundMessage(string key, EmptyMessageBody message)
        {
            throw new NotImplementedException();
        }

        private void ReceivedChatMessageMessage(string key, ChatMessageMessageBody message)
        {
            throw new NotImplementedException();
        }

        private void SaveAndDistributeMessage<T>(string sender, Message<T> e) where T: IMessageBody
        {
            if (sender != this.drawerId)
            {
                this.logger?.LogError("Drawing message sent not by drawer: {} sender: {}", drawerId, sender);
                return;
            }

            if (!this.isGameRunning)
            {
                this.logger?.LogError("Game not running but drawing message sent. sender: {}", sender);
                return;
            }

            this.currentDrawing.Add(e);
            this.DistributeMessage(sender, e);
        }

        private void DistributeMessage<T>(string sender, Message<T> e) where T : IMessageBody
        {
            var message = System.Text.Encoding.UTF8.GetBytes(e.SerializeToJson());

            double ttl;
            if (!isGameRunning || drawingDuration == null || this.drawingStartTime == null)
            {
                // brauch ich das überhaupt? kann ich die queue als speicher verwenden? weil dann fehlt ja der erste teil der zeichnung...
                ttl=TimeSpan.FromMinutes(5).TotalMilliseconds;
            }
            else
            {
                ttl = this.drawingDuration.Value.Subtract(DateTime.Now.Subtract(this.drawingStartTime.Value)).TotalMilliseconds;
            }

            var uintTTl = Convert.ToUInt32(ttl);

            foreach (var id in this.users)
            {
                if (id == sender)
                {
                    continue;
                }

                this.messageBroker.SendMessageAsync(id, message, uintTTl);
            }

            foreach (var id in this.zombieUsers)
            {
                this.messageBroker.SendMessageAsync(id, message, uintTTl);
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects)
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                disposedValue=true;
            }
        }

        // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        ~GameService()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: false);
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
