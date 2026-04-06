namespace Model
{
    public class Game
    {
        public GameState GameState { get; private set; }

        public Player FirstPlayer { get; private set; }

        public Player SecondPlayer { get; private set; }

        public int LastPlayerId { get; private set; }

        public Field GameField { get; private set; }

        public Game()
        {
            var fieldWidth = 11;
            var fieldHeight = 13;    
            GameState = GameState.MainScreen;
            LastPlayerId = 0;
            FirstPlayer = new Player("User", ++LastPlayerId);
            SecondPlayer = new Player("AI", ++LastPlayerId);
            GameField = new Field(fieldWidth, fieldHeight);
            var startCell1 = GameField.Map[fieldWidth / 2, fieldHeight - 1];
            var startCell2 = GameField.Map[fieldWidth / 2, 0];
            startCell1.PutEntity(new Castle(startCell1, FirstPlayer));
            startCell2.PutEntity(new Castle(startCell2, SecondPlayer));
        }

        public Game(int fieldWidth, int fieldHeight)
        {
            GameState = GameState.MainScreen;
            LastPlayerId = 0;
            FirstPlayer = new Player("User", ++LastPlayerId);
            SecondPlayer = new Player("AI", ++LastPlayerId);
            GameField = new Field(fieldWidth, fieldHeight);
            var startCell1 = GameField.Map[fieldWidth / 2, fieldHeight - 1];
            var startCell2 = GameField.Map[fieldWidth / 2, 0];
            startCell1.PutEntity(new Castle(startCell1, FirstPlayer));
            startCell2.PutEntity(new Castle(startCell2, SecondPlayer));
        }

        public void ChangeGameState(GameState newState)
        {
            GameState = newState;
            StateChanged?.Invoke(GameState);
        }

        // Событие, уведомляющее об изменении состояния игры
        public event Action<GameState> StateChanged;
    }
}
