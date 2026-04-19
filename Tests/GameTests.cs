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
            Game.Start(11, 13);
            var session = Game.Session;
            var firstPlayer = session.FirstPlayer;
            var secondPlayer = session.SecondPlayer;
            var field = session.GameField;
            Assert.That(Game.State, Is.EqualTo(GameState.MainScreen));

            Assert.That(session.FirstPlayer.Name, Is.EqualTo("User"));
            Assert.That(firstPlayer.Id, Is.EqualTo(1));
            Assert.That(firstPlayer.Team, Is.EqualTo(Team.First));
            Assert.That(firstPlayer.Color, Is.EqualTo(TeamColor.Blue));
            Assert.That(firstPlayer.OwnedBuildings, Is.Empty);
            Assert.That(firstPlayer.OwnedUnits, Is.Empty);
            Assert.That(firstPlayer.Food, Is.EqualTo(50));
            Assert.That(firstPlayer.Gold, Is.EqualTo(100));

            Assert.That(secondPlayer.Name, Is.EqualTo("AI"));
            Assert.That(secondPlayer.Id, Is.EqualTo(2));
            Assert.That(secondPlayer.Team, Is.EqualTo(Team.Second));
            Assert.That(secondPlayer.Color, Is.EqualTo(TeamColor.Red));
            Assert.That(secondPlayer.OwnedBuildings, Is.Empty);
            Assert.That(secondPlayer.OwnedUnits, Is.Empty);
            Assert.That(secondPlayer.Food, Is.EqualTo(50));
            Assert.That(secondPlayer.Gold, Is.EqualTo(100));

            Assert.That(field.Map[10, 12].X, Is.EqualTo(10));
            Assert.That(field.Map[10, 12].Y, Is.EqualTo(12));
            Assert.That(field.Map[10, 12].Entity, Is.Null);

            Assert.That(field.Map[5, 0].Entity is Castle);
            Assert.That(field.Map[5, 12].Entity is Castle);
            Assert.That(field.Map[5, 0].Entity.Owner, Is.EqualTo(secondPlayer));
            Assert.That(field.Map[5, 12].Entity.Owner, Is.EqualTo(firstPlayer));

            Assert.That(session.LastPlayerId, Is.EqualTo(2));
        }

        [Test]
        public void GameStateChanges()
        {
            Game.ChangeGameState(GameState.PlayOptionScreen);
            Assert.That(Game.State, Is.EqualTo(GameState.PlayOptionScreen));
            Game.ChangeGameState(GameState.GameScreen);
            Assert.That(Game.State, Is.EqualTo(GameState.GameScreen));
        }

        [Test]
        public void CanPlaceMine()
        {
            Game.Start(11, 13);
            var player = Game.Session.FirstPlayer;
            var map = Game.Session.GameField.Map;
            player.TryBuild(map[4, 12]);
            Assert.That(map[4, 12].Entity is Farm);
            Assert.That(map[4, 12].Entity.Owner, Is.EqualTo(player));
        }
    }
}
