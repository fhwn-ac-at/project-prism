namespace GameLib
{
    using MessageLib;
    using MessageLib.SharedObjects;
    using Microsoft.Extensions.Options;
    using System;
    using System.Linq;
    using System.Threading;

    public class Game
    {
        private readonly Dictionary<string, UserGameState> users;
        // Users that where connected once
        private readonly Dictionary<string, UserGameState> zombieUsers = [];

        private readonly Dictionary<string, uint> drawingRoundScore = [];

        private readonly LinkedList<Message<IMessageBody>> drawing = [];
        private readonly WordList wordList;
        private readonly Random random;

        private readonly int totalRoundAmount;
        private readonly int drawingDuration; //s

        private readonly uint wordSelectionCount = 3; 
        private readonly uint selectionDuration = 60; 
        private readonly uint maxScore = 500; 
        private readonly uint minScore = 50;
        private readonly uint maxDrawerScoreTimeScore = 500;
        private readonly uint minDrawerScoreTimeScore = 250;
        private readonly double minFactor = 0.4;
        private readonly double orderReduction = 0.1; 
        private readonly double orderFactorStartPoint = 1; 
        private readonly double amountGuessedMaxFactor;
        private readonly double easyWordFactor = 0.5;
        private readonly double midWordFactor = 1.0;
        private readonly double hardWordFactor = 1.5;

        private int roundAmount;
        private int drawerCounter;
        private int guessedCounter;
        private WordListItem[]? selectableWords;
        private WordListItem? selectedWord;
        private bool started;
        private string? drawerId;

        private readonly CancellationTokenSource selectionTimerCancellationToken = new();
        private readonly CancellationTokenSource drawingCancellationToken = new();

        private readonly object startedLock = new object();
        private readonly object selectableWordsLock = new object();
        private readonly object drawerIdLock = new object();

        public event EventHandler<WordSelectionEventArgs>? WordSelection;
        public event EventHandler<WordListItem>? SelectedWord;
        public event EventHandler<DrawingEndedEventArgs>? DrawingEnded;
        public event EventHandler<Dictionary<string, uint>>? GameEnded;
        public event EventHandler<UserScoredEventArgs>? UserScored;

        internal Game(HashSet<string> users, int roundAmount, int drawingDuration, WordList wordList, IOptions<GameOptions> gameOptions)
        {
            this.users = users.ToDictionary((key) => key, (_) => new UserGameState());
            this.roundAmount = roundAmount;
            this.totalRoundAmount=roundAmount;
            this.drawingDuration=drawingDuration;
            this.wordList = wordList;
            this.wordSelectionCount=gameOptions.Value.WordSelectionCount;
            this.selectionDuration=gameOptions.Value.SelectionDuration;
            this.maxScore=gameOptions.Value.MaxScore;
            this.minScore=gameOptions.Value.MinScore;
            this.minFactor=gameOptions.Value.MinFactor;
            this.orderReduction=gameOptions.Value.OrderReduction;
            this.orderFactorStartPoint=gameOptions.Value.OrderFactorStartPoint;
            this.maxDrawerScoreTimeScore=gameOptions.Value.MaxDrawerScoreTimeScore;
            this.minDrawerScoreTimeScore=gameOptions.Value.MinDrawerScoreTimeScore;
            this.amountGuessedMaxFactor=gameOptions.Value.AmountGuessedMaxFactor;
            this.easyWordFactor=gameOptions.Value.EasyWordFactor;
            this.midWordFactor=gameOptions.Value.MidWordFactor;
            this.hardWordFactor=gameOptions.Value.HardWordFactor;
            this.random = new Random();
        }

        public DateTime? RoundStartTime { get; internal set; }
        public int DrawingDuration => this.drawingDuration;

        public IEnumerable<Message<IMessageBody>> CurrentDrawing => this.drawing;

        public void Start()
        {
            lock (this.startedLock)
            {
                if (started)
                {
                    return;
                }
                started=true;
            }

            this.FireWordSelectionEvent();
        }

        public void AddUser(string key)
        {
            if (!this.zombieUsers.TryGetValue(key, out var user))
            {
                this.users.Add(key, new());
                return;
            }

            this.zombieUsers.Remove(key);
            this.users.Add(key, new(user.Score));   
        }

        public void RemoveUser(string key)
        {
            if (!this.users.TryGetValue(key, out var user))
            {
                return;
            }

            lock (this.drawerIdLock)
            {
                //TODO verify if user is drawer
                if (this.drawerId == key)
                {
                    this.drawerCounter--;
                    this.drawingCancellationToken.Cancel();
                }
            }

            // TODO add user handle lock
            this.zombieUsers.Add(key, new(user.Score));
            this.users.Remove(key);
        }

        public bool AddToDrawing<T>(string userId, Message<T> e) where T : IMessageBody
        {
            if (userId == null || userId != drawerId)
            {
                return false;
            }

            this.drawing.AddLast(new LinkedListNode<Message<IMessageBody>>(new Message<IMessageBody>(e.MessageBody, e.MessageHeader)));
            return true;
        }

        public void ClearDrawing()
        {
            this.drawing.Clear();
        }

        public bool GuessWord(string text, string key)
        {
            if (!this.users.TryGetValue(key, out var user))
            {
                return false;
            }

            lock (user)
            {
                if (user.Guessed)
                {
                    return false;
                }
            }

            bool guessed;
            lock (this.selectableWordsLock)
            {
                guessed = this.selectedWord != null && string.Equals(text, this.selectedWord.Word, StringComparison.OrdinalIgnoreCase);
            }

            if (guessed)
            {
                lock (user)
                {
                    if (user.Guessed)
                    {
                        return false;
                    }

                    user.Guessed=true;
                    this.guessedCounter++;
                }
               
                var score = this.CalculateScore();

                this.drawingRoundScore.Add(key, score);
                this.users[key].Score+=score;
                this.FireUserScoredEvent(key, score);

                if (this.HaveAllGuessed())
                {
                    this.drawingCancellationToken.Cancel();
                }
            }

            return guessed;
        }

        public bool SelectWord(string word)
        {
            lock (this.selectableWordsLock)
            {
                if (this.selectableWords==null)
                {
                    return false;
                }

                this.selectionTimerCancellationToken.Cancel();
                this.selectedWord=this.selectableWords.FirstOrDefault(item => item.Word==word);
                this.FireWordSelectedEvent();

                if (selectedWord==null)
                {
                    return false;
                }

                this.selectableWords=null;
            }
           
            return true;
        }

        public void Undo()
        {
            // TODO implement as on the frontend!!!
            this.drawing.RemoveLast();
        }

        private bool HaveAllGuessed()
        {
            return this.users.Values.All(x => x.Guessed);
        }

        private void FireUserScoredEvent(string key, uint score)
        {
            this.UserScored?.Invoke(this, new UserScoredEventArgs(key, score));
        }

        private void FireWordSelectionEvent()
        {
            lock (this.selectableWordsLock)
            {
                lock (this.drawerIdLock)
                {
                    if (this.WordSelection==null||this.selectableWords!=null)
                    {
                        return;
                    }

                    this.drawerId=this.users.Keys.ElementAt(this.users.Keys.Count-1-this.drawerCounter);

                    HashSet<int> uniqueIndices = new HashSet<int>();
                    while (uniqueIndices.Count<this.wordSelectionCount)
                    {
                        uniqueIndices.Add(this.random.Next(0, this.wordList.Words.Length));
                    }

                    this.selectableWords=uniqueIndices.Select(i => this.wordList.Words[i]).ToArray();

                    Task.Run(() =>
                    {
                        this.WordSelection.Invoke(this, new WordSelectionEventArgs(this.selectableWords, this.drawerId));
                        this.StartWordSelectionTimer();
                    });
                }
            }
        }

        private void StartWordSelectionTimer()
        {
            Task.Run(async () =>
            {
                await Task.Delay(TimeSpan.FromSeconds(this.selectionDuration), this.selectionTimerCancellationToken.Token);

                if (this.selectionTimerCancellationToken.IsCancellationRequested)
                {
                    return;
                }

                lock (this.selectableWordsLock)
                {
                    if (this.selectedWord == null && this.selectableWords != null)
                    {
                        this.selectedWord=this.selectableWords[random.Next(0, this.selectableWords.Length)];
                        this.selectableWords=null;
                        this.FireWordSelectedEvent();
                    }
                }
            });
        }

        private void FireWordSelectedEvent()
        {
            if (this.selectedWord == null || this.SelectedWord == null)
            {
                return;
            }

            this.SelectedWord.Invoke(this, this.selectedWord);

            this.StartDrawing();
        }

        private void StartDrawing()
        {
            this.RoundStartTime = DateTime.Now;
            Task.Run(async () =>
            {
                await Task.Delay(TimeSpan.FromSeconds(this.DrawingDuration), this.drawingCancellationToken.Token);                
                this.FireDrawingEnded();
            });
        }

        private void FireDrawingEnded()
        {
            if (this.drawerId==null)
            {
                return;
            }

            lock (this.selectableWordsLock)
            {
                this.selectedWord=null;
            }

            var drawerScore = this.CalculateDrawerScore();
            this.drawingRoundScore.Add(this.drawerId, drawerScore);
            this.users[this.drawerId].Score+=drawerScore;
            this.FireUserScoredEvent(this.drawerId, drawerScore);

            this.drawerCounter++;
            this.guessedCounter=0;
            this.ClearDrawing();

            if (this.drawerCounter >= this.users.Count)
            {
                this.roundAmount--;
                this.drawerCounter=0;
            }

            if (this.roundAmount <= 0)
            {
                this.FireGameEndedEvent();
                return;
            }

            this.DrawingEnded?.Invoke(this, new DrawingEndedEventArgs(this.totalRoundAmount - this.roundAmount + 1, this.drawingRoundScore));
            this.drawingRoundScore.Clear();
            // TODO maby delay for showing scores
            this.FireWordSelectionEvent();
        }

        private void FireGameEndedEvent()
        {
            this.GameEnded?.Invoke(this, this.users.ToDictionary(pair => pair.Key, pair => pair.Value.Score));
        }

        private uint CalculateScore()
        {
            if (this.RoundStartTime == null)
            {
                return 0;
            }

            double timeTaken = DateTime.Now.Subtract(this.RoundStartTime.Value).TotalSeconds;
           

            // Ensure timeTaken is within bounds
            if (timeTaken > this.drawingDuration)
            {
                return 0; // No points for guesses after time is up
            }

            // Calculate remaining time as a fraction
            double timeFactor = (this.drawingDuration - timeTaken) / this.drawingDuration;

            double orderFactor = this.orderFactorStartPoint-(this.guessedCounter-1)*this.orderReduction; 
            orderFactor=Math.Max(orderFactor, this.minFactor);         

            // Calculate the score
            uint score = Convert.ToUInt32(maxScore*timeFactor*orderFactor);

            // Ensure the score is at least the minimum
            return Math.Max(score, this.minScore);
        }

        private uint CalculateDrawerScore()
        {
            if (this.RoundStartTime==null || this.selectedWord==null)
            {
                return 0;
            }

            double timeTaken = DateTime.Now.Subtract(this.RoundStartTime.Value).TotalSeconds;

            // Calculate remaining time as a fraction
            double timeFactor = (this.drawingDuration - timeTaken) / this.drawingDuration;

            double timeScore = ((this.maxDrawerScoreTimeScore-this.minDrawerScoreTimeScore)*timeFactor)+this.minDrawerScoreTimeScore;

            // Correct Guess Factor
            double correctGuessFactor = this.amountGuessedMaxFactor * ((double)this.guessedCounter / this.users.Count);

            double difficultyMultiplier = this.selectedWord.Difficulty switch {
                0 => this.easyWordFactor,
                2 => this.hardWordFactor,
                _ => this.midWordFactor
            };

            // Final Score
            return Convert.ToUInt32(timeScore*correctGuessFactor*difficultyMultiplier);
        }
    }
}
