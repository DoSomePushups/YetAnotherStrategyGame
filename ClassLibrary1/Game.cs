using Svg;
using System.Collections.Concurrent;
using System.Drawing;
using System.Timers;

namespace Model
{
    public class Game
    {
        public static readonly Dictionary<EntityType, Image> SvgImages = new()
        {
            [EntityType.Human] = SvgDocument.Open(Path.Combine(AppContext.BaseDirectory, "Views", "SVGs", "Human.svg")).Draw(80, 80),
            [EntityType.Warrior] = SvgDocument.Open(Path.Combine(AppContext.BaseDirectory, "Views", "SVGs", "warrior.svg")).Draw(80, 80),
            [EntityType.Crossbowman] = SvgDocument.Open(Path.Combine(AppContext.BaseDirectory, "Views", "SVGs", "crossbowman.svg")).Draw(80, 80),
            [EntityType.Cannon] = SvgDocument.Open(Path.Combine(AppContext.BaseDirectory, "Views", "SVGs", "cannon_unit.svg")).Draw(80, 80),
            [EntityType.Farm] = SvgDocument.Open(Path.Combine(AppContext.BaseDirectory, "Views", "SVGs", "farm.svg")).Draw(80, 80),
            [EntityType.Mine] = SvgDocument.Open(Path.Combine(AppContext.BaseDirectory, "Views", "SVGs", "GoldMine.svg")).Draw(80, 80),
            [EntityType.Castle] = SvgDocument.Open(Path.Combine(AppContext.BaseDirectory, "Views", "SVGs", "castle.svg")).Draw(80, 80),
            [EntityType.Barracks] = SvgDocument.Open(Path.Combine(AppContext.BaseDirectory, "Views", "SVGs", "SwordBarracks.svg")).Draw(80, 80),
            [EntityType.CrossbowWorkshop] = SvgDocument.Open(Path.Combine(AppContext.BaseDirectory, "Views", "SVGs", "CrossbowBarracks.svg")).Draw(80, 80),
            [EntityType.CannonFactory] = SvgDocument.Open(Path.Combine(AppContext.BaseDirectory, "Views", "SVGs", "CannonFactory.svg")).Draw(80, 80),
        };

        public GameState State { get; private set; }

        public GameSession Session { get; private set; }

        public Game()
        {
            State = GameState.MainScreen;
        }

        public void Start(int width, int height)
        {
            Session = new(width, height);
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

            public GameSession(int fieldWidth, int fieldHeight)
            {
                LastPlayerId = 0;
                FirstPlayer = new Player(this, "User", ++LastPlayerId, false);
                SecondPlayer = new Player(this, "AI", ++LastPlayerId, true);
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
            }

            public event Action OnTick;
        }
    }
}
