using NUnit.Framework;
using WeaselKeeper.Analysis;
using WeaselKeeper.Identifiers;

namespace WeaselKeeper.Tests
{
    [TestFixture]
    public class The_documentation
    {

        [Test]
        public void DoSomeStuf()
        {
            const string comments = "// Customer: The Customer to change\n" +
                                    "// Points: The customer's points";
            var dict = Documentation.ParseExplanations(comments);

            Assert.That(dict["Customer"], Is.EqualTo("The Customer to change"));
            Assert.That(dict["Points"], Is.EqualTo("The customer's points"));
        }

        [Test]
        public void is_generated_from_comments()
        {
            var map = new ReplacementMap();
            map.Remember("Customer", "Cus");
            map.Remember("Points", "Pts");
            const string comments = "// Customer: The Customer to change\n" +
                                    "// Points: The customer's points";

            const string expected = "// Cus: The Customer to change\n" +
                                    "// Pts: The customer's points";

            string documentation = Documentation.From(comments, map);

            Assert.That(documentation, Is.EqualTo(expected));
        }

        [Test]
        public void defaults_to_the_normal_condition_in_case_the_identifier_was_uncommented()
        {
            var map = new ReplacementMap();
            map.Remember("Customer", "Cus");
            map.Remember("Points", "Pts");

            const string comments = "// Customer: The Customer to change";

            const string expected = "// Cus: The Customer to change\n" +
                                    "// Pts: Points";

            string documentation = Documentation.From(comments, map);

            Assert.That(documentation, Is.EqualTo(expected));
        }
    }
}