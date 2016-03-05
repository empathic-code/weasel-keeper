using System.Linq;
using NUnit.Framework;
using WeaselKeeper.Analysis;
using WeaselKeeper.Config;
using WeaselKeeper.Identifiers;

namespace WeaselKeeper.Tests
{
    [TestFixture]
    public class TestTryOuts
    {
        [SetUp]
        public void Init()
        {
            _blacklist = new Blacklist();
            _reverse = new Reverse();
            _weasel = new Condition(_reverse, _blacklist);
        }

        private class Reverse : IReplace
        {
            public string Replace(string identifier)
            {
                return string.Concat(identifier.Reverse());
            }
        }

        private Blacklist _blacklist;
        private IReplace _reverse;
        private Condition _weasel;

        public string Go(string code)
        {
            return Snippet.Parse(code).ReplaceIdentifiers(_weasel).ToString();
        }

        [Test]
        public void ShouldNotRenameBlacklistedIdentifiers()
        {
            _blacklist.BannedWords.Add("Main");
            const string source = "// hello: Hello\n// Main: Main\npublic void Main(int hello){ int hello = hello; }";
            string transformed = Go(source);
            string target = "// olleh: Hello\npublic void Main(int olleh){ int olleh = olleh; }";
            Assert.That(transformed, Is.EqualTo(target));
        }

        [Test]
        public void ShouldRenameAllIdentifiers()
        {
            string source = "// hello: Hello\n// Main: Main\npublic void Main(int hello){ int hello = hello; }";
            string transformed = Go(source);
            string target = "// niaM: Main\n// olleh: Hello\npublic void niaM(int olleh){ int olleh = olleh; }";
            Assert.That(transformed, Is.EqualTo(target));
        }
    }
}