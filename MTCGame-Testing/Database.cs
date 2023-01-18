using System;
using System.Diagnostics;
using Npgsql;
using NUnit.Framework.Internal;
using NUnit.Framework;

namespace MTCGame.Database.Tests
{
    public class checkCardifExistsTests
    {
        [Test]
        public void ToggleTestWorks()
        {
            // Arrange
            var readbio = new ReadBio("kienboec");

            // Act
            readbio.ToogleTestMode();

            // Assert
            Assert.That(readbio._DBname, Is.EqualTo("MTCGTests"));
        }

        [Test]
        public void RetrieveBio()
        {
            // Arrange
            var readbio = new ReadBio("kienboec", true);

            // Assert
            Assert.That(readbio.Answer.Contains("200|Data successfully retrieved|"));
        }

        [Test]
        public void RetrieveBioFail()
        {
            // Arrange
            var readbio = new ReadBio("kienbo", true);

            // Assert
            Assert.That(readbio.Answer, Is.EqualTo("200|Data successfully retrieved|]"));
        }

        [Test]
        public void LoginUserTest()
        {
            // Arrange
            var loguser = new LoginUser("kienboec", "daniel", true);

            // Assert
            Assert.That(loguser.Answer, Is.EqualTo("200|User login successful|kienboec-mtcgToken"));
        }

        [Test]
        public void LoginUserTestFail()
        {
            // Arrange
            var loguser = new LoginUser("kienbo", "daniel", true);

            // Assert
            Assert.That(loguser.Answer, Is.Null);
        }

        [Test]
        public void LoginUserTestPwFail()
        {
            // Arrange
            var loguser = new LoginUser("kienboec", "dani", true);

            // Assert
            Assert.That(loguser.Answer, Is.EqualTo("401|Invalid username/password provided|Password not correct!"));
        }

        [Test]
        public void LoginUserTestBothFail()
        {
            // Arrange
            var loguser = new LoginUser("kien", "dan", true);

            // Assert
            Assert.That(loguser.Answer, Is.Null);
        }

        [Test]
        public void PrintDeckempty()
        {
            // Arrange
            var cruser = new CreateUser("phil", "geeee" );
            var loguser = new PrintDeck("kienboec", true);

            // Assert
            Assert.That(loguser.Answer.Contains("200|The deck has cards, the response contains these"));
        }

        [Test]
        public void PrintDeckEmptyFail()
        {
            // Arrange
            var cruser = new CreateUser("phil", "geeee");
            var loguser = new PrintDeck("k1enboe", true);

            // Assert
            Assert.That(loguser.Answer.Contains("200|The deck has cards, the response contains these"));
        }

        [Test]
        public void PrintDeckEmpty()
        {
            // Arrange
            //var cruser = new CreateUser("phil", "geeee", true);
            var loguser = new PrintDeck("phil", true);

            // Assert
            Assert.That(loguser.Answer, Is.EqualTo("200|The deck has cards, the response contains these|[{null},{null},{null},{null}]"));
        }

        [Test]
        public void PrintDeckEmptyFailToo()
        {
            // Arrange
            //var cruser = new CreateUser("phil", "geeee", true);
            var loguser = new PrintDeck("phil", true);

            // Assert
            Assert.That(loguser.Answer, Is.EqualTo("200|The deck has cards, the response contains these|[{null},{null},{null},{null}]"));
        }

        [Test]
        public void ConfigureDeckTest()
        {
            // Arrange

            var loguser = new ConfigureDeck("altenhof", "[\"845f0dc7-37d0-426e-994e-43fc3ac83c08\", \"e85e3976-7c86-4d06-9a80-641c2019a79f\", \"171f6076-4eb5-4a7d-b3f2-2d650cc3d237\"]", true);

            // Assert
            Assert.That(loguser.Answer.Contains("400|The provided deck did not include the required"));
        }

        [Test]
        public void ConfigureDeckTestToFew()
        {
            // Arrange

            var loguser = new ConfigureDeck("altnhof", "[\"845f0dc7-37d0-426e-994e-43fc3ac83c08\", \"e85e3976-7c86-4d06-9a80-641c2019a79f\", \"171f6076-4eb5-4a7d-b3f2-2d650cc3d237\"]", true);

            // Assert
            Assert.That(loguser.Answer.Contains("400|The provided deck did not include the required"));
        }

        [Test]
        public void ConfigureDeckTestWorks()
        {
            // Arrange

            var loguser = new ConfigureDeck("altenhof", "[\"845f0dc7-37d0-426e-994e-43fc3ac83c08\", \"99f8f8dc-e25e-4a95-aa2c-782823f36e2a\", \"e85e3976-7c86-4d06-9a80-641c2019a79f\", \"171f6076-4eb5-4a7d-b3f2-2d650cc3d237\"]", true);

            // Assert
            Assert.That(loguser.Answer, Is.EqualTo("201|Package and cards successfully created|Package added!"));
        }

        [Test]
        public void ConfigureDeckTestFail()
        {
            // Arrange

            var loguser = new ConfigureDeck("nico", "[\"845f0dc7-37d0-426e-994e-43fc3ac83c08\", \"99f8f8dc-e25e-4a95-aa2c-782823f36e2a\", \"e85e3976-7c86-4d06-9a80-641c2019a79f\", \"171f6076-4eb5-4a7d-b3f2-2d650cc3d237\"]", true);

            // Assert
            Assert.That(loguser.Answer.Contains("999|Problem occurred adding package"));
        }

        [Test]
        public void CreatePackageTestFailCardAlready()
        {
            // Arrange

            var loguser = new CreatePackage("{\"Id\":\"845f0dc7-37d0-426e-994e-43fc3ac83c08\", \"Name\":\"WaterGoblin\", \"Damage\": 10.0}, {\"Id\":\"99f8f8dc-e25e-4a95-aa2c-782823f36e2a\", \"Name\":\"Dragon\", \"Damage\": 50.0}, {\"Id\":\"e85e3976-7c86-4d06-9a80-641c2019a79f\", \"Name\":\"WaterSpell\", \"Damage\": 20.0}, {\"Id\":\"1cb6ab86-bdb2-47e5-b6e4-68c5ab389334\", \"Name\":\"Ork\", \"Damage\": 45.0}, {\"Id\":\"dfdd758f-649c-40f9-ba3a-8657f4b3439f\", \"Name\":\"FireSpell\",    \"Damage\": 25.0}", true);

            // Assert
            Assert.That(loguser.Answer, Is.EqualTo("409|At least one card in the packages already exists|Corrupt Package, at least one Card is already in the database!"));
        }

        [Test]
        public void UpdateBioWorks()
        {
            // Arrange

            var loguser = new UpdateBio("altenhof", "null", true);

            // Assert
            Assert.That(loguser.Answer, Is.EqualTo("201|Package and cards successfully created|Package added!"));
        }

        [Test]
        public void UpdateBioFail()
        {
            // Arrange
            var loguser = new UpdateBio("ltenhof", "null", true);

            // Assert
            Assert.That(loguser.Answer, Is.EqualTo("201|Package and cards successfully created|Package added!"));
        }

        [Test]
        public void UpdateBioCrash()
        {
            // Arrange
            var loguser = new UpdateBio("altenhof", null, true);

            // Assert
            Assert.That(loguser.Answer, Is.EqualTo("201|Package and cards successfully created|Package added!"));
        }
    }
}