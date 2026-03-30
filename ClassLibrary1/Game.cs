namespace ClassLibrary1
{
    public class Game
    {
        public GameState GameState { get; private set; }

        public Player FirstPlayer { get; private set; }

        public Player SecondPlayer { get; private set; }

        public int LastPlayerId { get; private set; } = 0;

        public Game()
        {
            GameState = GameState.MainScreen;
            FirstPlayer = new Player("User", ++LastPlayerId);
            SecondPlayer = new Player("AI", ++LastPlayerId);
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
