using MTCGame.Database;
using System;

namespace MTCGame.Battlelogic.Tests
{
    public class LobbyTests
    {
        [Test]
        public void BattleTest()
        {
            // Arrange
            var battle = new Battle("StrongMonster", "WeakMonster");

            // Assert
            Assert.That(battle.Answer, Is.EqualTo("StrongMonster"));
        }
    }
}