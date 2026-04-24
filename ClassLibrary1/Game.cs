using Svg;
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
            public Player FirstPlayer { get; private set; }

            public Player SecondPlayer { get; private set; }

            public int LastPlayerId { get; private set; }

            public Field GameField { get; private set; }

            public System.Timers.Timer Timer { get; private set; }

            public int TimeTicks { get; private set; }

            public int TimeSeconds => TimeTicks / 10;

            public GameSession(int fieldWidth, int fieldHeight)
            {
                LastPlayerId = 0;
                FirstPlayer = new Player(this, "User", ++LastPlayerId);
                SecondPlayer = new Player(this, "AI", ++LastPlayerId);
                GameField = new Field(fieldWidth, fieldHeight);
                var startCell1 = GameField.Map[fieldWidth / 2, fieldHeight - 1];
                var startCell2 = GameField.Map[fieldWidth / 2, 0];
                startCell1.PutEntity(new Castle(startCell1, FirstPlayer));
                startCell2.PutEntity(new Castle(startCell2, SecondPlayer));
                TimeTicks = 0;
                Timer = new(100);
                Timer.Elapsed += (s, e) => OnTick?.Invoke();
                Timer.AutoReset = true;
                Timer.Start();
                OnTick += () => TimeTicks++;
            }

            public event Action OnTick;
        }
    }

    public enum EntityType
    {
        None,
        Human,
        Warrior,
        Crossbowman,
        Cannon,
        Farm,
        Mine,
        Castle,
        Barracks,
        CrossbowWorkshop,
        CannonFactory
    }
}
