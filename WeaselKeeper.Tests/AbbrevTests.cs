using NUnit.Framework;
using WeaselKeeper.Identifiers;

namespace WeaselKeeper.Tests
{
    [TestFixture]
    public class AbbrevTests
    {
        [Test]
        public void AbbreviationKeepsFirstCharacterIntact()
        {
            const string identifier = "CustomerPoints";
            const string expected = "Cst";
            var threeCharacters = new ThreeCharacters();
            string abbreviated = threeCharacters.Replace(identifier);

            Assert.That(abbreviated, Is.EqualTo(expected));
        }

        [Test]
        public void AbbreviationsAreChosenAlphabetically()
        {
            const string identifier = "CustomerPoints";
            var singleCharacter = new SingleCharacter();
            string abbreviated = singleCharacter.Replace(identifier);
            Assert.That(abbreviated, Is.EqualTo("a"));
        }

        [Test]
        public void KeepCapitalizationIntactAtBeginningOfStringButNotInTheMiddle()
        {
            const string identifier = "dueDate";
            var threeCharacters = new ThreeCharacters();
            string abbreviated = threeCharacters.Replace(identifier);
            Assert.That(abbreviated, Is.EqualTo("ddt"));
        }

        [Test]
        public void KeepCapitalizationIntacte()
        {
            const string identifier = "Date";
            var threeCharacters = new ThreeCharacters();
            string abbreviated = threeCharacters.Replace(identifier);
            Assert.That(abbreviated, Is.EqualTo("Dtt"));
        }

        [Test]
        public void ShiftingOffsetKeepsFirstCharacterIntact()
        {
            var threeCharacters = new ThreeCharacters();
            string a1 = threeCharacters.Replace("Customer");
            string a2 = threeCharacters.Replace("Customer");
            string a3 = threeCharacters.Replace("Customer");
            string a4 = threeCharacters.Replace("Customer");
            string a5 = threeCharacters.Replace("Customer");

            Assert.That(a1, Is.EqualTo("Cst"));
            Assert.That(a2, Is.EqualTo("Ctm"));
            Assert.That(a3, Is.EqualTo("Cmr"));
            Assert.That(a4, Is.EqualTo("Crs"));
            Assert.That(a5, Is.EqualTo("Cst"));

            string b1 = threeCharacters.Replace("Points");
            string b2 = threeCharacters.Replace("Points");
            Assert.That(b1, Is.EqualTo("Pnt"));
            Assert.That(b2, Is.EqualTo("Pts"));
        }
    }
}