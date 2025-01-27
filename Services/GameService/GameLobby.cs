namespace GameService
{
    using AMQPLib;
    using GameLib;
    using MessageLib;
    using MessageLib.Game;
    using MessageLib.Joined;
    using MessageLib.Lobby;
    using MessageLib.SharedObjects;
    using Microsoft.Extensions.Options;

    public class GameLobby : IDisposable
    {
        private readonly Lobby lobby;
        private readonly Dictionary<string, User> users = [];
        private Game? game;
        private readonly IAMQPBroker messageBroker;
        private readonly IServiceProvider serviceProvider;

        private readonly ILogger<GameLobby>? logger;
        private bool disposedValue;
        private readonly object gameLock = new object();

        public GameLobby(IAMQPBroker messageBroker, IServiceProvider serviceProvider, IOptions<LobbyOptions> lobbyOptions, ILogger<GameLobby>? logger = null)
        {
            this.messageBroker=messageBroker;
            this.logger=logger;
            this.lobby=new(serviceProvider, lobbyOptions);
            this.serviceProvider=serviceProvider;
        }

        public int UserCount => this.lobby.UserCount;

        public void AddUser(User user)
        {
            lock(this.users)
            {
                if (this.users.ContainsKey(user.Id))
                {
                    return;
                }

                this.messageBroker.ConnectToQueueAsync(user.Id, this.ConnectMessageDistributor(user.Id));

                foreach (var knownUser in this.users)
                {
                    this.SendMessage(user.Id, new UserJoinedMessage(new UserJoinedMessageBody(knownUser.Value)));
                }

                this.users.Add(user.Id, user);
                this.game?.AddUser(user.Id);
                this.lobby.AddUser(user.Id);
                this.DistributeMessage(user.Id, new UserJoinedMessage(new UserJoinedMessageBody(user)));

                if (this.game != null)
                {
                    this.SendMessage(user.Id, new ClearMessage());
                    this.SendMessage(user.Id, new BackgroundColorMessage(new BackgroundColorMessageBody(new HexColor("#FFF"))));
                    foreach (var message in this.game.CurrentDrawing)
                    {
                        this.SendMessage(user.Id, message);
                    }
                }
            }            
        }

        public void RemoveUser(string userId)
        {
            lock (this.users)
            {
                if (!this.users.TryGetValue(userId, out var user))
                {
                    return;
                }

                this.game?.RemoveUser(userId);
                this.lobby.RemoveUser(userId);
                this.users.Remove(userId);
                this.DistributeMessage(userId, new UserDisconnectedMessage(new UserDisconnectedMessageBody(user)));
            }
        }

        public bool StartGame()
        {
            lock(gameLock)
            {

                if (this.game!=null)
                {
                    return false;
                }
            }

            if (this.UserCount<2)
            {
                return false;
            }

            this.DistributeMessage(null, new GameStartedMessage());
            // Discard to not have a warning
            _ = this.StartGameAndConnectEvents();
            return true;
        }

        private async Task StartGameAndConnectEvents()
        {
            lock (gameLock)
            {
                this.game=this.lobby.StartGame();
            }

            this.game.WordSelection+=this.ReceivedWordSelectionEvent;
            this.game.WordSelected+=this.ReceivedWordSelectedEvent;
            this.game.Hint+=this.ReceivedHintEvent;
            this.game.DrawingEnded+=this.ReceivedDrawingEndedEvent;
            this.game.GameEnded+=this.ReceivedGameEndedEvent;
            this.game.UserScored+=this.ReceivedUserScoredEvent;
            this.game.GuessClose+=this.ReceivedGuessCloseEvent;

            await Task.Delay(100);
            this.game.Start();
        }

        private void ReceivedGuessCloseEvent(object? sender, GuessCloseEventArgs e)
        {
            this.SendMessage(e.User, new GuessCloseMessage(new GuessCloseMessageBody(e.Guess, e.Distance)));
        }

        private void ReceivedUserScoredEvent(object? sender, UserScoredEventArgs e)
        {
            User? user;
            lock (this.users)
            {
                if (!this.users.TryGetValue(e.UserId, out user))
                {
                    this.logger?.LogError("Unknown User scored. Íd: {}", e.UserId);
                    return;
                }
            }

            this.DistributeMessage(null, new UserScoreMessage(new UserScoreMessageBody(user, e.Score)));
            this.SendMessage(e.UserId, new SearchedWordMessage(new SearchedWordMessageBody(e.SearchedWord)));
        }

        private void ReceivedGameEndedEvent(object? sender, GameEndedEventArgs e)
        {
            this.DistributeMessage(null, new GameEndedMessage(new GameEndedMessageBody(e.SearchedWord, e.DrawingRoundScore)));
            this.game=null;
        }

        private void ReceivedDrawingEndedEvent(object? sender, DrawingEndedEventArgs e)
        {
            this.DistributeMessage(null, new NextRoundMessage(new NextRoundMessageBody(e.SearchedWord, e.Round, e.DrawingRoundScore)));
        }

        private void ReceivedWordSelectedEvent(object? sender, WordSelectedEventArgs e)
        {
            this.DistributeMessage(e.Drawer, new SearchedWordMessage(new SearchedWordMessageBody(e.SelectedWord)));
            this.SendMessage(e.Drawer, new SearchedWordMessage(new SearchedWordMessageBody(e.TextSelectedWord)));
        }
        private void ReceivedHintEvent(object? sender, HintEventArgs e)
        {
            foreach (var user in e.Users)
            {
                this.SendMessage(user, new SearchedWordMessage(new SearchedWordMessageBody(e.Hint)));
            }
        }

        private void ReceivedWordSelectionEvent(object? sender, WordSelectionEventArgs e)
        {
            this.SendMessage(e.Drawer, new SetDrawerMessage(new SetDrawerMessageBody(e.WordListItems.Select(item => new SelectWordItem(item.Word, item.Difficulty)).ToList())));
            this.DistributeMessage(e.Drawer, new SetNotDrawerMessage());
        }

        private IMessageDistributor ConnectMessageDistributor(string key)
        {
            var messageDistributor = this.serviceProvider.GetRequiredService<MessageDistributor>();

            messageDistributor.ReceivedChatMessageMessage+=(_, message) => this.ReceivedChatMessageMessage(key, message);
            messageDistributor.ReceivedNextRoundMessage+=(_, message) => this.ReceivedNextRoundMessage(key, message);
            messageDistributor.ReceivedSearchedWordMessage+=(_, message) => this.ReceivedSearchedWordMessage(key, message);
            messageDistributor.ReceivedGameEndedMessage+=(_, message) => this.ReceivedGameEndedMessage(key, message);
            messageDistributor.ReceivedGameStartedMessage+=(_, message) => this.ReceivedGameStartedMessage(key, message);
            messageDistributor.ReceivedGuessCloseMessage+=(_, message) => this.ReceivedGuessCloseMessage(key, message);
            messageDistributor.ReceivedRoundAmountChangedMessage+=(_, message) => this.ReceivedRoundAmountChangedMessage(key, message);
            messageDistributor.ReceivedRoundDurationChangedMessage+=(_, message) => this.ReceivedRoundDurationChangedMessage(key, message);
            messageDistributor.ReceivedSelectWordMessage+=(_, message) => this.ReceivedSelectWordMessage(key, message);
            messageDistributor.ReceivedSetDrawerMessage+=(_, message) => this.ReceivedSetDrawerMessage(key, message);
            messageDistributor.ReceivedSetNotDrawerMessage+=(_, message) => this.ReceivedSetNotDrawerMessage(key, message);
            messageDistributor.ReceivedUserDisconnectedMessage+=(_, message) => this.ReceivedUserDisconnectedMessage(key, message);
            messageDistributor.ReceivedUserJoinedMessage+=(_, message) => this.ReceivedUserJoinedMessage(key, message);
            messageDistributor.ReceivedUserScoreMessage+=(_, message) => this.ReceivedUserScoreMessage(key, message);
            messageDistributor.ReceivedUndoMessage+=(_, message) => this.ReceivedUndoMessage(key, message);
            messageDistributor.ReceivedClearMessage+=(_, message) => this.ReceivedClearMessage(key, message);


            messageDistributor.ReceivedBackgroundColorMessage+=(_, message) => this.ReceivedBackgroundColorMessage(key, message);
            messageDistributor.ReceivedClosePathMessage+=(_, message) => this.ReceivedClosePathMessage(key, message);
            messageDistributor.ReceivedDrawingSizeChangedMessage+=(_, message) => this.ReceivedDrawingSizeChangedMessage(key, message);
            messageDistributor.ReceivedLineToMessage+=(_, message) => this.ReceivedLineToMessage(key, message);
            messageDistributor.ReceivedMoveToMessage+=(_, message) => this.ReceivedMoveToMessage(key, message);
            messageDistributor.ReceivedPointMessage+=(_, message) => this.ReceivedPointMessage(key, message);

            return messageDistributor;
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
            this.SaveAndDistributeMessage(key, new UndoMessage());
        }

        private void ReceivedGuessCloseMessage(string key, GuessCloseMessageBody message)
        {
            // should not get it
            this.logger?.LogWarning("Got Guess Close Message from sender: {}", key);
        }

        private void ReceivedGameStartedMessage(string key, EmptyMessageBody message)
        {
            // should not get it
            this.logger?.LogWarning("Got Game Started Message from sender: {}", key);
        }

        private void ReceivedUserScoreMessage(string key, UserScoreMessageBody message)
        {
            // should not get it
            this.logger?.LogWarning("Got User Score Message from sender: {}", key);
        }

        private void ReceivedUserJoinedMessage(string key, UserJoinedMessageBody message)
        {
            // should not get it
            this.logger?.LogWarning("Got User Joined Message from sender: {}", key);
        }

        private void ReceivedUserDisconnectedMessage(string key, UserDisconnectedMessageBody message)
        {
            // should not get it
            this.logger?.LogWarning("Got User Disconnected Message from sender: {}", key);
        }

        private void ReceivedSetNotDrawerMessage(string key, EmptyMessageBody message)
        {
            // should not get it
            this.logger?.LogWarning("Got Set Not Drawer Message from sender: {}", key);
        }

        private void ReceivedSetDrawerMessage(string key, SetDrawerMessageBody message)
        {
            // should not get it
            this.logger?.LogWarning("Got Set Drawer Message from sender: {}", key);
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

            this.game.SelectWord(message.Word);
// this.DistributeMessage(key, new SelectWordMessage(message));
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

        private void ReceivedGameEndedMessage(string key, GameEndedMessageBody message)
        {
            // should not get it
            this.logger?.LogWarning("Got Game Ended Message from sender: {}", key);
        }

        private void ReceivedSearchedWordMessage(string key, SearchedWordMessageBody message)
        {
            // should not get it
            this.logger?.LogWarning("Got Searched Word Message from sender: {}", key);
        }

        private void ReceivedNextRoundMessage(string key, NextRoundMessageBody message)
        {
            // should not get it
            this.logger?.LogWarning("Got Next Round Message from sender: {}", key);
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

            if (!this.game.AddToDrawing(sender, e))
            {
                this.logger?.LogError("Drawing message sent not by sender: {}", sender);
                return;
            }

            this.DistributeMessage(sender, e);
        }

        private void DistributeMessage<T>(string? sender, Message<T> message) where T : IMessageBody
        {
            foreach (var id in this.lobby.Users)
            {
                if (id==sender)
                {
                    continue;
                }

                this.SendMessage(id, message);
            }
        }

        private void SendMessage<T>(string receiver, Message<T> message) where T : IMessageBody
        {
            var byteMessage = System.Text.Encoding.UTF8.GetBytes(message.SerializeToJson());
            this.messageBroker.SendMessageAsync(receiver, byteMessage);
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
