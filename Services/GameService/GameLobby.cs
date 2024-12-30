namespace GameService
{
    using FrenziedMarmot.DependencyInjection;
    using GameLib;
    using MessageLib;
    using MessageLib.Game;
    using MessageLib.Joined;
    using MessageLib.Lobby;
    using MessageLib.SharedObjects;

    [Injectable(Lifetime = ServiceLifetime.Transient)]
    public class GameLobby : IDisposable
    {
        private readonly Lobby lobby;
        private Game? game;
        private readonly MessageDistributor messageDistributor;
        private readonly GameServiceMessageBroker messageBroker;

        private readonly ILogger<GameLobby>? logger;
        private bool disposedValue;

        public GameLobby(string id, GameServiceMessageBroker messageBroker, MessageDistributor messageDistributor, IServiceProvider serviceProvider, ILogger<GameLobby>? logger = null)
        {
            this.messageBroker=messageBroker;
            this.messageDistributor=messageDistributor;
            this.logger=logger;
            this.lobby=new(id);
        }

        public int UserCount => this.lobby.UserCount;

        public void AddUser(User user)
        {
            this.messageBroker.ConnectToQueueAsync(user.Id, this.ConnectMessageDistributor(user.Id));
            this.game?.AddUser(user.Id);
            this.lobby.AddUser(user.Id);
            this.DistributeMessage(user.Id, new UserJoinedMessage(new UserJoinedMessageBody(user)));
        }

        public void RemoveUser(User user)
        {
            this.game?.RemoveUser(user.Id);
            this.lobby.RemoveUser(user.Id);
            this.DistributeMessage(user.Id, new UserDisconnectedMessage(new UserDisconnectedMessageBody(user)));
        }

        public void StartGame()
        {
            if (this.UserCount>1)
            {
                this.game = this.lobby.StartGame();
            }
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
            this.SaveAndDistributeMessage(key, new PointMessage(message));
        }

        private void ReceivedMoveToMessage(string key, MoveToMessageBody message)
        {
            this.SaveAndDistributeMessage(key, new MoveToMessage(message));
        }

        private void ReceivedLineToMessage(string key, LineToMessageBody message)
        {
            this.SaveAndDistributeMessage(key, new LineToMessage(message));
        }

        private void ReceivedDrawingSizeChangedMessage(string key, DrawingSizeChangedMessageBody message)
        {
            this.SaveAndDistributeMessage(key, new DrawingSizeChangedMessage(message));
        }

        private void ReceivedClosePathMessage(string key, EmptyMessageBody message)
        {
            this.SaveAndDistributeMessage(key, new ClosePathMessage());
        }

        private void ReceivedBackgroundColorMessage(string key, BackgroundColorMessageBody message)
        {
            this.SaveAndDistributeMessage(key, new BackgroundColorMessage(message));
        }

        private void ReceivedClearMessage(string key, EmptyMessageBody message)
        {
            if (this.game==null)
            {
                this.logger?.LogError("Game not started! sender: {}", key);
                return;
            }
            this.game.ClearDrawing();
            this.DistributeMessage(key, new ClearMessage());
        }

        private void ReceivedUndoMessage(string key, EmptyMessageBody message)
        {
            if (this.game==null)
            {
                this.logger?.LogError("Game not started! sender: {}", key);
                return;
            }
            this.game.Undo();
            this.DistributeMessage(key, new UndoMessage());
        }

        private void ReceivedUserScoreMessage(string key, UserScoreMessageBody message)
        {
            // should not get it
            throw new NotImplementedException();
        }

        private void ReceivedUserJoinedMessage(string key, UserJoinedMessageBody message)
        {
            // should not get it
            throw new NotImplementedException();
        }

        private void ReceivedUserDisconnectedMessage(string key, UserDisconnectedMessageBody message)
        {
            // should not get it
            throw new NotImplementedException();
        }

        private void ReceivedSetNotDrawerMessage(string key, EmptyMessageBody message)
        {
            // should not get it
            throw new NotImplementedException();
        }

        private void ReceivedSetDrawerMessage(string key, SetDrawerMessageBody message)
        {
            // should not get it
            throw new NotImplementedException();
        }

        private void ReceivedSelectWordMessage(string key, SelectWordMessageBody message)
        {
            if (game==null)
            {
                this.logger?.LogError("Game not started! sender: {}", key);
                return;
            }

            if (!this.game.SelectWord(message.Word))
            {
                this.logger?.LogError("Selected word that didn't exist. Word: {}", message.Word);
                return;
            }

            this.DistributeMessage(key, new SelectWordMessage(message));
        }

        private void ReceivedRoundDurationChangedMessage(string key, RoundDurationChangedMessageBody message)
        {
            if (this.game != null)
            {
                this.logger?.LogWarning("Received round duration message during game. sender: {}", key);
            }

            if (message.Duration<=0)
            {
                this.logger?.LogError("Received round duration smaller than 0. duration: {}", message.Duration);
                return;
            }

            this.lobby.RoundDuration=message.Duration;
            this.DistributeMessage(key, new RoundDurationChangedMessage(message));
        }

        private void ReceivedRoundAmountChangedMessage(string key, RoundAmountChangedMessageBody message)
        {
            if (this.game!=null)
            {
                this.logger?.LogError("Received round amount message during game. sender: {}", key);
                return;
            }

            if (message.Rounds<=0)
            {
                this.logger?.LogError("Received round amount smaller than 0. amount: {}", message.Rounds);
                return;
            }

            this.lobby.RoundAmount=message.Rounds;
            this.DistributeMessage(key, new RoundAmountChangedMessage(message));
        }

        private void ReceivedGameEndedMessage(string key, EmptyMessageBody message)
        {
            // should not get it
            throw new NotImplementedException();
        }

        private void ReceivedSearchedWordMessage(string key, SearchedWordMessageBody message)
        {
            // should not get it
            throw new NotImplementedException();
        }

        private void ReceivedNextRoundMessage(string key, EmptyMessageBody message)
        {
            // should not get it
            throw new NotImplementedException();
        }

        private void ReceivedChatMessageMessage(string key, ChatMessageMessageBody message)
        {
            if (this.game != null && this.game.GuessWord(message.Text, key))
            {
                this.logger?.LogDebug("User: {} has correctly guessed the word", key);
                return;
            }

            this.DistributeMessage(key, new ChatMessageMessage(message));
        }

        private void SaveAndDistributeMessage<T>(string sender, Message<T> e) where T : IMessageBody
        {
            if (game == null)
            {
                this.logger?.LogError("Game not started! sender: {}", sender);
                return;
            }

            if (sender!= this.game.DrawerId)
            {
                this.logger?.LogError("Drawing message sent not by drawer: {} sender: {}", this.game.DrawerId, sender);
                return;
            }

            if (!this.game.Running)
            {
                this.logger?.LogError("Game not running but drawing message sent. sender: {}", sender);
                return;
            }

            this.game.AddToDrawing(e);
            this.DistributeMessage(sender, e);
        }

        private void DistributeMessage<T>(string sender, Message<T> e) where T : IMessageBody
        {
            var message = System.Text.Encoding.UTF8.GetBytes(e.SerializeToJson());

            double ttl;
            if (this.game == null||this.game.RoundStartTime==null)
            {
                // brauch ich das überhaupt? kann ich die queue als speicher verwenden? weil dann fehlt ja der erste teil der zeichnung...
                ttl=TimeSpan.FromMinutes(5).TotalMilliseconds;
            }
            else
            {
                ttl=DateTime.Now.Subtract(this.game.RoundStartTime.Value).Subtract(TimeSpan.FromSeconds(this.game.RoundDuration)).TotalMilliseconds;
            }

            var uintTTl = Convert.ToUInt32(ttl);

            foreach (var id in this.lobby.Users)
            {
                if (id==sender)
                {
                    continue;
                }

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
        ~GameLobby()
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
