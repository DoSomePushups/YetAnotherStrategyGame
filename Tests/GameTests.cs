using ClassLibrary1;
using NUnit.Framework;
using FluentAssertions;

namespace Tests
{
    [TestFixture]
    public class GameTests
    {
        private Game Game;
        [SetUp]
        public void Setup()
        {
            Game = new Game();
        }

        [Test]
        public void DefaultGameInicialization()
        {
            Assert.That(Game.GameState, Is.EqualTo(State.MainScreen));

            Assert.That(Game.FirstPlayer.Name, Is.EqualTo("User"));
            Assert.That(Game.FirstPlayer.Id, Is.EqualTo(1));
            Assert.That(Game.FirstPlayer.Team, Is.EqualTo(Team.First));
            Assert.That(Game.FirstPlayer.Color, Is.EqualTo(Color.Blue));
            Assert.That(Game.FirstPlayer.OwnedBuildings, Is.Empty);
            Assert.That(Game.FirstPlayer.OwnedUnits, Is.Empty);
            Assert.That(Game.FirstPlayer.Food, Is.EqualTo(0));
            Assert.That(Game.FirstPlayer.Gold, Is.EqualTo(0));

            Assert.That(Game.SecondPlayer.Name, Is.EqualTo("AI"));
            Assert.That(Game.SecondPlayer.Id, Is.EqualTo(2));
            Assert.That(Game.SecondPlayer.Team, Is.EqualTo(Team.Second));
            Assert.That(Game.SecondPlayer.Color, Is.EqualTo(Color.Red));
            Assert.That(Game.SecondPlayer.OwnedBuildings, Is.Empty);
            Assert.That(Game.SecondPlayer.OwnedUnits, Is.Empty);
            Assert.That(Game.SecondPlayer.Food, Is.EqualTo(0));
            Assert.That(Game.SecondPlayer.Gold, Is.EqualTo(0));

            Assert.That(Game.LastPlayerId, Is.EqualTo(2));
        }

        [Test]
        public void GameStateChanges()
        {
            Game.ChangeGameState(State.PlayOptionScreen);
            Assert.That(Game.GameState, Is.EqualTo(State.PlayOptionScreen));
            Game.ChangeGameState(State.GameScreen);
            Assert.That(Game.GameState, Is.EqualTo(State.GameScreen));
        }
    }
}
