using System.Timers;

namespace Model
{
    public class Game
    {
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

            public int Time { get; private set; }

            public GameSession(int fieldWidth, int fieldHeight)
            {
                LastPlayerId = 0;
                FirstPlayer = new Player("User", ++LastPlayerId);
                SecondPlayer = new Player("AI", ++LastPlayerId);
                GameField = new Field(fieldWidth, fieldHeight);
                var startCell1 = GameField.Map[fieldWidth / 2, fieldHeight - 1];
                var startCell2 = GameField.Map[fieldWidth / 2, 0];
                startCell1.PutEntity(new Castle(startCell1, FirstPlayer));
                startCell2.PutEntity(new Castle(startCell2, SecondPlayer));
                Time = 0;
                Timer = new(100);
                Timer.Elapsed += (s, e) => OnTick?.Invoke();
                Timer.AutoReset = true;
                Timer.Start();
                OnTick += () => Time++;
            }

            public event Action OnTick;
        }
    }
}
