using Svg;
using System.Collections.Concurrent;
using System.Drawing;
using System.Reflection;
using System.Timers;

namespace Model
{
    public partial class Game
    {
        public GameState State { get; private set; }

        public GameSession? Session { get; private set; }

        public bool IsAIMode { get; private set; }

        public Game()
        {
            State = GameState.MainScreen;
        }

        public void Start(int width, int height)
        {
            if (IsAIMode)
                Session = new(width, height, 1);
            else
                Session = new(width, height);
        }

        public void End()
        {
            Session = null;
        }

        public void ChangeAIMode(bool mode)
        {
            IsAIMode = mode;
        }

        public void ChangeGameState(GameState newState)
        {
            State = newState;
            StateChanged?.Invoke(State);
        }

        // Событие, уведомляющее об изменении состояния игры
        public event Action<GameState> StateChanged;

        public class GameSession
        {
            public HashSet<Player> Players { get; private set; }

            public Player FirstPlayer { get; private set; }

            public Player SecondPlayer { get; private set; }

            public int LastPlayerId { get; private set; }

            public Field GameField { get; private set; }

            public System.Timers.Timer Timer { get; private set; }

            public int TimeTicks { get; private set; }

            public int TimeSeconds => TimeTicks / (1000 / TickInterval);

            public const int TickInterval = 200; //200

            // Потокобезопасная очередь
            private ConcurrentQueue<Action> ActionQueue = new ConcurrentQueue<Action>();

            public void EnqueueAction(Action action)
            {
                ActionQueue.Enqueue(action);
            }

            public GameSession(int fieldWidth, int fieldHeight, int playerAI = -1)
            {
                LastPlayerId = 0;
                FirstPlayer = new Player(this, "User", ++LastPlayerId, playerAI);
                SecondPlayer = new Player(this, "AI", ++LastPlayerId, 0);
                Players = new HashSet<Player>() { FirstPlayer, SecondPlayer };
                GameField = new Field(fieldWidth, fieldHeight);
                var startCell1 = GameField.Map[fieldWidth / 2, fieldHeight - 1];
                var startCell2 = GameField.Map[fieldWidth / 2, 0];
                var castle1 = new Castle(startCell1, FirstPlayer);
                FirstPlayer.OwnedEntities.Add(castle1);
                var castle2 = new Castle(startCell2, SecondPlayer);
                SecondPlayer.OwnedEntities.Add(castle2);
                startCell1.PutEntity(castle1);
                startCell2.PutEntity(castle2);
                TimeTicks = 0;
                Timer = new(TickInterval);
                Timer.Elapsed += (s, e) =>
                {
                    while (ActionQueue.TryDequeue(out var action))
                        action();
                    OnTick?.Invoke();
                };
                Timer.AutoReset = true;
                Timer.Start();
                OnTick += () => TimeTicks++;
                OnTick += castle1.HandleTick;
                OnTick += castle2.HandleTick;
                OnTick += SecondPlayer.Robot.MakeMove;
                if (FirstPlayer.Robot != null)
                    OnTick += FirstPlayer.Robot.MakeMove;
            }

            public event Action OnTick;
        }
    }
}
