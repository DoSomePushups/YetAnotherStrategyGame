using Model;
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
            Assert.That(Game.GameState, Is.EqualTo(GameState.MainScreen));

            Assert.That(Game.FirstPlayer.Name, Is.EqualTo("User"));
            Assert.That(Game.FirstPlayer.Id, Is.EqualTo(1));
            Assert.That(Game.FirstPlayer.Team, Is.EqualTo(Team.First));
            Assert.That(Game.FirstPlayer.Color, Is.EqualTo(TeamColor.Blue));
            Assert.That(Game.FirstPlayer.OwnedBuildings, Is.Empty);
            Assert.That(Game.FirstPlayer.OwnedUnits, Is.Empty);
            Assert.That(Game.FirstPlayer.Food, Is.EqualTo(0));
            Assert.That(Game.FirstPlayer.Gold, Is.EqualTo(0));

            Assert.That(Game.SecondPlayer.Name, Is.EqualTo("AI"));
            Assert.That(Game.SecondPlayer.Id, Is.EqualTo(2));
            Assert.That(Game.SecondPlayer.Team, Is.EqualTo(Team.Second));
            Assert.That(Game.SecondPlayer.Color, Is.EqualTo(TeamColor.Red));
            Assert.That(Game.SecondPlayer.OwnedBuildings, Is.Empty);
            Assert.That(Game.SecondPlayer.OwnedUnits, Is.Empty);
            Assert.That(Game.SecondPlayer.Food, Is.EqualTo(0));
            Assert.That(Game.SecondPlayer.Gold, Is.EqualTo(0));

            Assert.That(Game.GameField.Map[10, 12].X, Is.EqualTo(10));
            Assert.That(Game.GameField.Map[10, 12].Y, Is.EqualTo(12));
            Assert.That(Game.GameField.Map[10, 12].Entity, Is.Null);

            Assert.That(Game.GameField.Map[5, 0].Entity is Castle);
            Assert.That(Game.GameField.Map[5, 12].Entity is Castle);
            Assert.That(Game.GameField.Map[5, 0].Entity.Owner, Is.EqualTo(Game.SecondPlayer));
            Assert.That(Game.GameField.Map[5, 12].Entity.Owner, Is.EqualTo(Game.FirstPlayer));

            Assert.That(Game.LastPlayerId, Is.EqualTo(2));
        }

        [Test]
        public void GameStateChanges()
        {
            Game.ChangeGameState(GameState.PlayOptionScreen);
            Assert.That(Game.GameState, Is.EqualTo(GameState.PlayOptionScreen));
            Game.ChangeGameState(GameState.GameScreen);
            Assert.That(Game.GameState, Is.EqualTo(GameState.GameScreen));
        }

        [Test]
        public void CanPlaceMine()
        {
            var cell = Game.GameField.Map[]
            var castle = new Castle();
            Game.GameField.Map[0, 0].
        }
    }
}
