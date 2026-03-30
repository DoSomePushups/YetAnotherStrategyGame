namespace ClassLibrary1
{
    public class Game
    {
        public State GameState { get; private set; }

        public Player FirstPlayer { get; private set; }

        public Player SecondPlayer { get; private set; }

        public int LastPlayerId { get; private set; } = 0;

        public Game()
        {
            GameState = State.MainScreen;
            FirstPlayer = new Player("User", ++LastPlayerId);
            SecondPlayer = new Player("AI", ++LastPlayerId);
        }

        public void ChangeGameState(State newState)
        {
            GameState = newState;
            StateChanged?.Invoke(this, newState);
        }

        public event EventHandler<State> StateChanged;
    }
}
